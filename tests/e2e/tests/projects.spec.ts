import { test } from "@playwright/test";
import { useToken } from "../utils/utils";
import { expect } from "../utils/matchers";
import { deleteUser, registerUser } from "../utils/users.utils";
import { getPersonalWorkspace } from "../utils/workspaces.utils";
import {
  create2ProjectsIn2DifferentWorkspaces,
  createProject,
  createProjects,
  getProjects
} from "../utils/projects.utils";
import CreateProjectRequest from "../types/requests/project/CreateProjectRequest";
import { ProjectSprintName } from "../utils/constants";

test.describe('should allow operations on the project entity', () => {
  let WORKSPACE_ID: string

  test.beforeEach('should register user and get workspace id', async ({ request }) => {
    process.env.API_TOKEN = (await registerUser(request)).token
    WORKSPACE_ID = (await getPersonalWorkspace(request)).id
  })

  test.afterEach('should delete user', async ({ request }) => {
    await deleteUser(request)
  })

  test('should get project', async ({ request }) => {
    const createRequest = {
      workspaceId: WORKSPACE_ID,
      name: "Project",
      description: "",
      isPrivate: true
    }
    const project = await createProject(request, createRequest);

    const response = await request.get(
      `/api/projects/${project.id}`,
      useToken()
    )

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toStrictEqual({
      name: project.name,
      description: createRequest.description,
      isPrivate: createRequest.isPrivate,
      isFavorite: expect.any(Boolean),
      sprints: [
        {
          id: expect.any(String),
          name: ProjectSprintName,
          startDate: null,
          endDate: null,
        }
      ]
    })
  })

  test.describe('should allow to get collection of projects', () => {
    test('should get collection of projects', async ({ request }) => {
      const createProjectRequest: CreateProjectRequest = {
        workspaceId: WORKSPACE_ID,
        name: "Project",
        description: "Some Project",
        isPrivate: true
      }
      await createProject(request, createProjectRequest)

      const response = await request.get(`/api/projects`, useToken())

      expect(response.ok()).toBeTruthy()

      const json = await response.json()
      expect(json).toStrictEqual({
        projects: [
          {
            id: expect.any(String),
            name: createProjectRequest.name,
            description: createProjectRequest.description,
            isPrivate: createProjectRequest.isPrivate,
            isFavorite: false,
          }
        ],
        totalCount: 1,
        totalPages: null
      })
      expect(json.projects).toHaveLength(1)
    })

    test('should get collection of projects filtered by workspace', async ({ request }) => {
      const { createProjectRequest1 } = await create2ProjectsIn2DifferentWorkspaces(request)

      const response = await request.get(
        `/api/projects?workspaceId=${createProjectRequest1.workspaceId}`,
        useToken()
      )

      expect(response.ok()).toBeTruthy()

      const json = await response.json()
      expect(json).toStrictEqual({
        projects: [
          {
            id: expect.any(String),
            name: createProjectRequest1.name,
            description: createProjectRequest1.description,
            isPrivate: createProjectRequest1.isPrivate,
            isFavorite: false,
          }
        ],
        totalCount: 1,
        totalPages: null
      })
      expect(json.projects).toHaveLength(1)
    })

    test('should get collection of projects filtered by name', async ({ request }) => {
      const createProjectRequest1: CreateProjectRequest = {
        workspaceId: WORKSPACE_ID,
        name: "Project 1",
        description: "This is some project",
        isPrivate: true
      }
      await createProject(request, createProjectRequest1)

      const createProjectRequest2 = {
        ...createProjectRequest1,
        name: "Project 2"
      }
      await createProject(request, createProjectRequest2)

      const response = await request.get(
        `/api/projects?name=proj`,
        useToken()
      )

      expect(response.ok()).toBeTruthy()

      const json = await response.json()
      expect(json).toStrictEqual({
        projects: expect.arrayContaining([
          {
            id: expect.any(String),
            name: createProjectRequest1.name,
            description: createProjectRequest1.description,
            isPrivate: createProjectRequest1.isPrivate,
            isFavorite: false,
          },
          {
            id: expect.any(String),
            name: createProjectRequest2.name,
            description: createProjectRequest2.description,
            isPrivate: createProjectRequest2.isPrivate,
            isFavorite: false,
          },
        ]),
        totalCount: 2,
        totalPages: null
      })
      expect(json.projects).toHaveLength(2)
    })

    test('should get collection of projects ordered by name ascending', async ({ request }) => {
      const totalCount = 2
      const createProjectRequests = await createProjects(request, totalCount)

      const response = await request.get(
        `/api/projects?order=name.asc`,
        useToken()
      )

      expect(response.ok()).toBeTruthy()

      const json = await response.json()
      expect(json).toStrictEqual({
        projects: [
          {
            id: expect.any(String),
            name: createProjectRequests[0].name,
            description: createProjectRequests[0].description,
            isPrivate: createProjectRequests[0].isPrivate,
            isFavorite: false,
          },
          {
            id: expect.any(String),
            name: createProjectRequests[1].name,
            description: createProjectRequests[1].description,
            isPrivate: createProjectRequests[1].isPrivate,
            isFavorite: false,
          },
        ],
        totalCount: totalCount,
        totalPages: null
      })
      expect(json.projects).toHaveLength(totalCount)
    })

    test('should get collection of projects paginated and ordered by name ascending', async ({ request }) => {
      const totalCount = 5
      const createProjectRequests = await createProjects(request, totalCount)

      const pageSize = 2
      const currentPage = 1
      const response = await request.get(
        `/api/projects?order=name.asc&pageSize=${pageSize}&currentPage=${currentPage}`,
        useToken()
      )

      expect(response.ok()).toBeTruthy()


      const start = pageSize * (currentPage - 1)
      const expectedProjects = createProjectRequests
        .slice(start, start + pageSize)
        .map(r => {
          return {
            id: expect.any(String),
            name: r.name,
            description: r.description,
            isPrivate: r.isPrivate,
            isFavorite: false
          }
        })

      const json = await response.json()
      expect(json).toStrictEqual({
        projects: expectedProjects,
        totalCount: totalCount,
        totalPages: Math.ceil(totalCount / pageSize),
      })
      expect(json.projects).toHaveLength(totalCount < pageSize ? totalCount : pageSize)
    })
  })

  test('should get collection of projects for user', async ({ request }) => {
    const {
      createProjectRequest1,
      createProjectRequest2
    } = await create2ProjectsIn2DifferentWorkspaces(request)

    const response = await request.get(`/api/projects/user`, useToken())

    expect(response.ok()).toBeTruthy()

    const json = await response.json()
    expect(json).toStrictEqual({
      projects: expect.arrayContaining([
        {
          id: expect.any(String),
          name: createProjectRequest1.name
        },
        {
          id: expect.any(String),
          name: createProjectRequest2.name
        }
      ])
    })
    expect(json.projects).toHaveLength(2)
  })

  test('should create project', async ({ request }) => {
    const body: CreateProjectRequest = {
      workspaceId: WORKSPACE_ID,
      name: "New Project",
      description: "This is a new project description",
      isPrivate: false
    }
    const response = await request.post('/api/projects', {
      ...useToken(),
      data: body
    });

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toStrictEqual({
      message: expect.any(String)
    })

    const projects = await getProjects(request)
    expect(projects).toHaveLength(1)
    expect(projects).toStrictEqual(expect.arrayContaining([
      {
        id: expect.any(String),
        name: body.name
      }
    ]))
  })

  test('should delete project', async ({ request }) => {
    const project = await createProject(request, {
      workspaceId: WORKSPACE_ID,
      name: "Project name",
      description: "",
      isPrivate: true
    })

    const response = await request.delete(`/api/projects/${project.id}`, useToken());

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toStrictEqual({
      message: expect.any(String)
    })

    const projects = await getProjects(request)
    expect(projects).toHaveLength(0)
  })
})