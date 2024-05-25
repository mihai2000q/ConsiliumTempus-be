import { test } from "@playwright/test";
import { useToken } from "../utils/utils";
import { expect } from "../utils/matchers";
import { deleteUser, registerUser } from "../utils/users.utils";
import { getPersonalWorkspace } from "../utils/workspaces.utils";
import {
  create2ProjectsIn2DifferentWorkspaces,
  createProject,
  createProjects,
  getProject,
  getProjectOverview,
  getProjects
} from "../utils/projects.utils";
import CreateProjectRequest from "../types/requests/project/CreateProjectRequest";
import UpdateProjectRequest from "../types/requests/project/UpdateProjectRequest";
import UpdateOverviewProjectRequest from "../types/requests/project/UpdateOverviewProjectRequest";

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
    const createRequest: CreateProjectRequest = {
      workspaceId: WORKSPACE_ID,
      name: "Project",
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
      isPrivate: createRequest.isPrivate,
      isFavorite: expect.any(Boolean)
    })
  })

  test('should get overview project', async ({ request }) => {
    const createRequest: CreateProjectRequest = {
      workspaceId: WORKSPACE_ID,
      name: "Project",
      isPrivate: true
    }
    const project = await createProject(request, createRequest);

    const response = await request.get(
      `/api/projects/overview/${project.id}`,
      useToken()
    )

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toStrictEqual({
      description: "",
    })
  })

  test.describe('should allow to get collection of projects', () => {
    test('should get collection of projects', async ({ request }) => {
      const createProjectRequest: CreateProjectRequest = {
        workspaceId: WORKSPACE_ID,
        name: "Project",
        isPrivate: true
      }
      await createProject(request, createProjectRequest)

      const response = await request.get(`/api/projects`, useToken())

      expect(response.ok()).toBeTruthy()

      const json = await response.json()
      expect(json.projects).toHaveLength(1)
      expect(json).toStrictEqual({
        projects: [
          {
            id: expect.any(String),
            name: createProjectRequest.name,
            description: "",
            isPrivate: createProjectRequest.isPrivate,
            isFavorite: false,
          }
        ],
        totalCount: 1,
        totalPages: null
      })
    })

    test('should get collection of projects filtered by workspace', async ({ request }) => {
      const { createProjectRequest1 } = await create2ProjectsIn2DifferentWorkspaces(request)

      const response = await request.get(
        `/api/projects?workspaceId=${createProjectRequest1.workspaceId}`,
        useToken()
      )

      expect(response.ok()).toBeTruthy()

      const json = await response.json()
      expect(json.projects).toHaveLength(1)
      expect(json).toStrictEqual({
        projects: [
          {
            id: expect.any(String),
            name: createProjectRequest1.name,
            description: "",
            isPrivate: createProjectRequest1.isPrivate,
            isFavorite: false,
          }
        ],
        totalCount: 1,
        totalPages: null
      })
    })

    test('should get collection of projects filtered by name', async ({ request }) => {
      const createProjectRequest1: CreateProjectRequest = {
        workspaceId: WORKSPACE_ID,
        name: "Project 1",
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
      expect(json.projects).toHaveLength(2)
      expect(json).toStrictEqual({
        projects: expect.arrayContaining([
          {
            id: expect.any(String),
            name: createProjectRequest1.name,
            description: "",
            isPrivate: createProjectRequest1.isPrivate,
            isFavorite: false,
          },
          {
            id: expect.any(String),
            name: createProjectRequest2.name,
            description: "",
            isPrivate: createProjectRequest2.isPrivate,
            isFavorite: false,
          },
        ]),
        totalCount: 2,
        totalPages: null
      })
    })

    test('should get collection of projects ordered by name ascending', async ({ request }) => {
      const totalCount = 2
      const createProjectRequests = await createProjects(request, totalCount)

      const response = await request.get(
        `/api/projects?orders=name.asc`,
        useToken()
      )

      expect(response.ok()).toBeTruthy()

      const json = await response.json()
      expect(json.projects).toHaveLength(totalCount)
      expect(json).toStrictEqual({
        projects: [
          {
            id: expect.any(String),
            name: createProjectRequests[0].name,
            description: "",
            isPrivate: createProjectRequests[0].isPrivate,
            isFavorite: false,
          },
          {
            id: expect.any(String),
            name: createProjectRequests[1].name,
            description: "",
            isPrivate: createProjectRequests[1].isPrivate,
            isFavorite: false,
          },
        ],
        totalCount: totalCount,
        totalPages: null
      })
    })

    test('should get collection of projects paginated and ordered by name ascending', async ({ request }) => {
      const totalCount = 5
      const createProjectRequests = await createProjects(request, totalCount)

      const pageSize = 2
      const currentPage = 1
      const response = await request.get(
        `/api/projects?orders=name.asc&pageSize=${pageSize}&currentPage=${currentPage}`,
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
            description: "",
            isPrivate: r.isPrivate,
            isFavorite: false
          }
        })

      const json = await response.json()
      expect(json.projects).toHaveLength(totalCount < pageSize ? totalCount : pageSize)
      expect(json).toStrictEqual({
        projects: expectedProjects,
        totalCount: totalCount,
        totalPages: Math.ceil(totalCount / pageSize),
      })
    })
  })

  test('should create project', async ({ request }) => {
    const body: CreateProjectRequest = {
      workspaceId: WORKSPACE_ID,
      name: "New Project",
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
    expect(projects).toStrictEqual([
      {
        id: expect.any(String),
        name: body.name,
        description: "",
        isPrivate: body.isPrivate,
        isFavorite: false
      }
    ])
  })

  test('should update project', async ({ request }) => {
    const createProjectRequest: CreateProjectRequest = {
      workspaceId: WORKSPACE_ID,
      name: "New Project",
      isPrivate: false
    }
    const project = await createProject(request, createProjectRequest)

    const updateProjectRequest: UpdateProjectRequest = {
      id: project.id,
      name: "This is my project now",
      isFavorite: true
    }
    const response = await request.put('/api/projects', {
      ...useToken(),
      data: updateProjectRequest
    })

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toStrictEqual({
      message: expect.any(String)
    })

    const projects = await getProject(request, project.id)
    expect(projects).toStrictEqual({
      name: updateProjectRequest.name,
      isFavorite: updateProjectRequest.isFavorite,
      isPrivate: createProjectRequest.isPrivate
    })
  })

  test('should update project overview', async ({ request }) => {
    const createProjectRequest: CreateProjectRequest = {
      workspaceId: WORKSPACE_ID,
      name: "New Project",
      isPrivate: false
    }
    const project = await createProject(request, createProjectRequest)

    const updateOverviewProjectRequest: UpdateOverviewProjectRequest = {
      id: project.id,
      description: "This is a new description"
    }
    const response = await request.put('/api/projects/overview', {
      ...useToken(),
      data: updateOverviewProjectRequest
    })

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toStrictEqual({
      message: expect.any(String)
    })

    const projectOverview = await getProjectOverview(request, project.id)
    expect(projectOverview).toStrictEqual({
      description: updateOverviewProjectRequest.description,
    })
  })

  test('should delete project', async ({ request }) => {
    const project = await createProject(request, {
      workspaceId: WORKSPACE_ID,
      name: "Project name",
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