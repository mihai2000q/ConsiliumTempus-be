import { test } from "@playwright/test";
import { useToken } from "../utils/utils";
import { expect } from "../utils/matchers";
import { deleteUser, registerUser } from "../utils/users.utils";
import { getPersonalWorkspace } from "../utils/workspaces.utils";
import {
  addProjectStatus,
  create2ProjectsIn2DifferentWorkspaces,
  createProject,
  createProjects,
  getProject,
  getProjectOverview,
  getProjects,
  getProjectStatus,
  getProjectStatuses
} from "../utils/projects.utils";
import CreateProjectRequest from "../types/requests/project/CreateProjectRequest";
import UpdateProjectRequest from "../types/requests/project/UpdateProjectRequest";
import UpdateOverviewProjectRequest from "../types/requests/project/UpdateOverviewProjectRequest";
import AddStatusToProjectRequest from "../types/requests/project/AddStatusToProjectRequest";
import UpdateStatusFromProjectRequest from "../types/requests/project/UpdateStatusFromProjectRequest";
import UpdateFavoritesProjectRequest from "../types/requests/project/UpdateFavoritesProjectRequest";

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
      isFavorite: false,
      lifecycle: 'Active',
      owner: expect.any(Object),
      isPrivate: createRequest.isPrivate,
      latestStatus: null,
      workspace: expect.any(Object)
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
            isFavorite: false,
            lifecycle: 'Active',
            owner: expect.any(Object),
            isPrivate: createProjectRequest.isPrivate,
            latestStatus: null,
            createdDateTime: expect.any(String),
          }
        ],
        totalCount: 1
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
            isFavorite: false,
            lifecycle: 'Active',
            owner: expect.any(Object),
            isPrivate: createProjectRequest1.isPrivate,
            latestStatus: null,
            createdDateTime: expect.any(String),
          }
        ],
        totalCount: 1
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
        `/api/projects?search=name ct proj`,
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
            isFavorite: false,
            lifecycle: 'Active',
            owner: expect.any(Object),
            isPrivate: createProjectRequest1.isPrivate,
            latestStatus: null,
            createdDateTime: expect.any(String),
          },
          {
            id: expect.any(String),
            name: createProjectRequest2.name,
            description: "",
            isFavorite: false,
            lifecycle: 'Active',
            owner: expect.any(Object),
            isPrivate: createProjectRequest2.isPrivate,
            latestStatus: null,
            createdDateTime: expect.any(String),
          },
        ]),
        totalCount: 2
      })
    })

    test('should get collection of projects ordered by name ascending', async ({ request }) => {
      const totalCount = 2
      const createProjectRequests = await createProjects(request, totalCount)

      const response = await request.get(
        `/api/projects?orderBy=name.asc`,
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
            isFavorite: false,
            lifecycle: 'Active',
            owner: expect.any(Object),
            isPrivate: createProjectRequests[0].isPrivate,
            latestStatus: null,
            createdDateTime: expect.any(String),
          },
          {
            id: expect.any(String),
            name: createProjectRequests[1].name,
            description: "",
            isFavorite: false,
            lifecycle: 'Active',
            owner: expect.any(Object),
            isPrivate: createProjectRequests[1].isPrivate,
            latestStatus: null,
            createdDateTime: expect.any(String),
          },
        ],
        totalCount: totalCount
      })
    })

    test('should get collection of projects paginated and ordered by name ascending', async ({ request }) => {
      const totalCount = 5
      const createProjectRequests = await createProjects(request, totalCount)

      const pageSize = 2
      const currentPage = 1
      const response = await request.get(
        `/api/projects?orderBy=name.asc&pageSize=${pageSize}&currentPage=${currentPage}`,
        useToken()
      )

      expect(response.ok()).toBeTruthy()

      const start = pageSize * (currentPage - 1)
      const expectedProjects = createProjectRequests
        .slice(start, start + pageSize)
        .map(request => {
          return {
            id: expect.any(String),
            name: request.name,
            description: "",
            isFavorite: false,
            lifecycle: 'Active',
            owner: expect.any(Object),
            isPrivate: request.isPrivate,
            latestStatus: null,
            createdDateTime: expect.any(String),
          }
        })

      const json = await response.json()
      expect(json.projects).toHaveLength(totalCount < pageSize ? totalCount : pageSize)
      expect(json).toStrictEqual({
        projects: expectedProjects,
        totalCount: totalCount
      })
    })
  })

  test('should get statuses from project', async ({ request }) => {
    const createProjectRequest: CreateProjectRequest = {
      workspaceId: WORKSPACE_ID,
      name: "Project",
      isPrivate: true
    }
    const project = await createProject(request, createProjectRequest)

    const AddStatusToProjectRequest: AddStatusToProjectRequest = {
      id: project.id,
      title: "New Project Status",
      status: "OnTrack",
      description: "This is the description of the new status"
    }
    const status = await addProjectStatus(request, AddStatusToProjectRequest)

    const response = await request.get(`/api/projects/${project.id}/statuses`, useToken())

    expect(response.ok()).toBeTruthy()

    const json = await response.json()
    expect(json.statuses).toHaveLength(1)
    expect(json).toStrictEqual({
      statuses: [
        {
          id: expect.any(String),
          title: status.title,
          status: status.status,
          description: status.description,
          createdBy: status.createdBy,
          createdDateTime: status.createdDateTime,
          updatedBy: status.updatedBy,
          updatedDateTime: status.updatedDateTime,
        }
      ],
      totalCount: 1
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
        isFavorite: false,
        lifecycle: 'Active',
        owner: expect.any(Object),
        isPrivate: body.isPrivate,
        latestStatus: null,
        createdDateTime: expect.any(String),
      }
    ])
  })

  test('should add status to project', async ({ request }) => {
    const createProjectRequest: CreateProjectRequest = {
      workspaceId: WORKSPACE_ID,
      name: "Project",
      isPrivate: true
    }
    const project = await createProject(request, createProjectRequest)

    const body: AddStatusToProjectRequest = {
      id: project.id,
      title: "New Project Status",
      status: "OnTrack",
      description: "This is the description of the new status"
    }
    const response = await request.post('/api/projects/add-status', {
      ...useToken(),
      data: body
    });

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toStrictEqual({
      message: expect.any(String)
    })

    const statuses = await getProjectStatuses(request, body.id)
    expect(statuses).toHaveLength(1)
    expect(statuses).toStrictEqual([
      {
        id: expect.any(String),
        title: body.title,
        status: body.status,
        description: body.description,
        createdBy: expect.any(Object),
        createdDateTime: expect.any(String),
        updatedBy: expect.any(Object),
        updatedDateTime: expect.any(String),
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

    const body: UpdateProjectRequest = {
      id: project.id,
      name: "This is my project now",
      lifecycle: 'Archived'
    }
    const response = await request.put('/api/projects', {
      ...useToken(),
      data: body
    })

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toStrictEqual({
      message: expect.any(String)
    })

    const projects = await getProject(request, project.id)
    expect(projects).toStrictEqual({
      name: body.name,
      isFavorite: false,
      lifecycle: body.lifecycle,
      owner: expect.any(Object),
      isPrivate: createProjectRequest.isPrivate,
      latestStatus: null,
      workspace: expect.any(Object)
    })
  })

  test('should update project favorites', async ({ request }) => {
    const createProjectRequest: CreateProjectRequest = {
      workspaceId: WORKSPACE_ID,
      name: "New Project",
      isPrivate: false
    }
    const project = await createProject(request, createProjectRequest)

    const body: UpdateFavoritesProjectRequest = {
      id: project.id,
      isFavorite: true
    }
    const response = await request.put('/api/projects/favorites', {
      ...useToken(),
      data: body
    })

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toStrictEqual({
      message: expect.any(String)
    })

    const projects = await getProject(request, project.id)
    expect(projects).toStrictEqual({
      name: createProjectRequest.name,
      isFavorite: body.isFavorite,
      lifecycle: 'Active',
      owner: expect.any(Object),
      isPrivate: createProjectRequest.isPrivate,
      latestStatus: null,
      workspace: expect.any(Object)
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

  test('should update status from project', async ({ request }) => {
    const createProjectRequest: CreateProjectRequest = {
      workspaceId: WORKSPACE_ID,
      name: "Project",
      isPrivate: true
    }
    const project = await createProject(request, createProjectRequest)

    const AddStatusToProjectRequest: AddStatusToProjectRequest = {
      id: project.id,
      title: "Project Status",
      status: "OnTrack",
      description: "This is the description of the new status"
    }
    const status = await addProjectStatus(request, AddStatusToProjectRequest);

    const body: UpdateStatusFromProjectRequest = {
      id: project.id,
      statusId: status.id,
      title: "New Project Status",
      status: "OffTrack",
      description: "We are officially doomed"
    }
    const response = await request.put('/api/projects/update-status', {
      ...useToken(),
      data: body
    });

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toStrictEqual({
      message: expect.any(String)
    })

    const statuses = await getProjectStatus(request, body.id, body.statusId)
    expect(statuses).toStrictEqual({
      id: expect.any(String),
      title: body.title,
      status: body.status,
      description: body.description,
      createdBy: status.createdBy,
      createdDateTime: status.createdDateTime,
      updatedBy: expect.any(Object),
      updatedDateTime: expect.any(String),
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

  test('should remove status from project', async ({ request }) => {
    const createProjectRequest: CreateProjectRequest = {
      workspaceId: WORKSPACE_ID,
      name: "Project",
      isPrivate: true
    }
    const project = await createProject(request, createProjectRequest)

    const AddStatusToProjectRequest: AddStatusToProjectRequest = {
      id: project.id,
      title: "Project Status",
      status: "OnTrack",
      description: "This is the description of the new status"
    }
    const status = await addProjectStatus(request, AddStatusToProjectRequest);

    const response = await request.delete(
      `/api/projects/${project.id}/remove-status/${status.id}`,
      useToken()
    )

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toStrictEqual({
      message: expect.any(String)
    })

    const statuses = await getProjectStatuses(request, project.id)
    expect(statuses).toHaveLength(0)
  })
})