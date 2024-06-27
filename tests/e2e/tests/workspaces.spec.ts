import { test } from "@playwright/test";
import { useToken } from "../utils/utils";
import { expect } from "../utils/matchers";
import {
  createWorkspace,
  createWorkspaces,
  getPersonalWorkspace,
  getWorkspace, getWorkspaceOverview,
  getWorkspaces
} from "../utils/workspaces.utils";
import { PersonalWorkspaceName } from "../utils/constants";
import { deleteUser, registerUser } from "../utils/users.utils";
import CreateWorkspaceRequest from "../types/requests/workspace/CreateWorkspaceRequest";
import UpdateWorkspaceRequest from "../types/requests/workspace/UpdateWorkspaceRequest";
import UpdateOverviewWorkspaceRequest from "../types/requests/workspace/UpdateOverviewWorkspaceRequest";

test.describe('should allow operations on the workspace entity', () => {
  const EMAIL = "michaelj@gmail.com"

  test.beforeEach('should register and create token', async ({ request }) => {
    process.env.API_TOKEN = (await registerUser(request, EMAIL)).token
  })

  test.afterEach('should delete user', async ({ request }) => {
    await deleteUser(request)
  })

  test('should get workspace', async ({ request }) => {
    const workspace = await getPersonalWorkspace(request);

    const response = await request.get(`/api/workspaces/${workspace.id}`, useToken())

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toStrictEqual({
      name: workspace.name,
      isPersonal: true,
      isFavorite: false,
      owner: {
        id: expect.any(String),
        name: expect.any(String),
        email: EMAIL
      }
    })
  })

  test('should get workspace overview', async ({ request }) => {
    const workspace = await getPersonalWorkspace(request);

    const response = await request.get(`/api/workspaces/overview/${workspace.id}`, useToken())

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toStrictEqual({
      description: workspace.description,
    })
  })

  test('should get collaborators', async ({ request }) => {
    const workspace = await getPersonalWorkspace(request);

    const response = await request.get(`/api/workspaces/${workspace.id}/collaborators`, useToken())

    expect(response.ok()).toBeTruthy()

    const json = await response.json()
    expect(json.collaborators).toHaveLength(1)
    expect(json.collaborators).toStrictEqual([{
      id: expect.any(String),
      name: expect.any(String),
      email: EMAIL,
    }])
  })

  test.describe('should allow to get collection of workspaces', async () => {
    test('should get collection of workspaces', async ({ request }) => {
      const createWorkspaceRequest: CreateWorkspaceRequest = {
        name: "Some Workspace",
      }
      await createWorkspace(request, createWorkspaceRequest)

      const response = await request.get('/api/workspaces', useToken())

      expect(response.ok()).toBeTruthy()

      const json = await response.json()
      expect(json).toStrictEqual({
        workspaces: expect.arrayContaining([
          {
            id: expect.any(String),
            name: PersonalWorkspaceName,
            description: expect.any(String),
            isFavorite: false,
            isPersonal: true,
            owner: expect.any(Object)
          },
          {
            id: expect.any(String),
            name: createWorkspaceRequest.name,
            description: "",
            isFavorite: false,
            isPersonal: false,
            owner: expect.any(Object)
          }
        ]),
        totalCount: 2
      })
      expect(json.workspaces).toHaveLength(2)
    })

    test('should get collection of workspaces with the personal workspace on top', async ({ request }) => {
      const createWorkspaceRequest: CreateWorkspaceRequest = {
        name: "Some Workspace",
      }
      await createWorkspace(request, createWorkspaceRequest)

      const response = await request.get('/api/workspaces?isPersonalWorkspaceFirst=true', useToken())

      expect(response.ok()).toBeTruthy()

      const json = await response.json()
      expect(json).toStrictEqual({
        workspaces: [
          {
            id: expect.any(String),
            name: PersonalWorkspaceName,
            description: expect.any(String),
            isFavorite: false,
            isPersonal: true,
            owner: expect.any(Object)
          },
          {
            id: expect.any(String),
            name: createWorkspaceRequest.name,
            description: "",
            isFavorite: false,
            isPersonal: false,
            owner: expect.any(Object)
          }
        ],
        totalCount: 2
      })
      expect(json.workspaces).toHaveLength(2)
    })

    test('should get collection of workspaces ordered by create date time descending', async ({ request }) => {
      const createWorkspaceRequests = await createWorkspaces(request, 2)

      const response = await request.get('/api/workspaces?orderBy=created_date_time.desc', useToken())

      expect(response.ok()).toBeTruthy()

      const json = await response.json()
      expect(json).toStrictEqual({
        workspaces: [
          {
            id: expect.any(String),
            name: createWorkspaceRequests[1].name,
            description: "",
            isFavorite: false,
            isPersonal: false,
            owner: expect.any(Object)
          },
          {
            id: expect.any(String),
            name: createWorkspaceRequests[0].name,
            description: "",
            isFavorite: false,
            isPersonal: false,
            owner: expect.any(Object)
          },
          {
            id: expect.any(String),
            name: PersonalWorkspaceName,
            description: expect.any(String),
            isFavorite: false,
            isPersonal: true,
            owner: expect.any(Object)
          }
        ],
        totalCount: 3
      })
      expect(json.workspaces).toHaveLength(3)
    })

    test('should get collection of workspaces filtered by name', async ({ request }) => {
      const createWorkspaceRequest: CreateWorkspaceRequest = {
        name: "Some Workspace",
      }
      await createWorkspace(request, createWorkspaceRequest)

      const response = await request.get('/api/workspaces?search=name ct some works', useToken())

      expect(response.ok()).toBeTruthy()

      const json = await response.json()
      expect(json).toStrictEqual({
        workspaces: [
          {
            id: expect.any(String),
            name: createWorkspaceRequest.name,
            description: "",
            isFavorite: false,
            isPersonal: false,
            owner: expect.any(Object)
          }
        ],
        totalCount: 1
      })
      expect(json.workspaces).toHaveLength(1)
    })

    test('should get collection of workspaces paginated and ordered by name ascending', async ({ request }) => {
      const totalCount = 5
      const createWorkspaceRequests = await createWorkspaces(request, totalCount - 1)

      const pageSize = 6
      const currentPage = 1
      const response = await request.get(`/api/workspaces?orderBy=name.asc&pageSize=${pageSize}&currentPage=${currentPage}`, useToken())

      expect(response.ok()).toBeTruthy()

      const start = pageSize * (currentPage - 1)
      const expectedWorkspaces: any[] = createWorkspaceRequests
        .slice(start, start + pageSize)
        .map(r => {
          return {
            id: expect.any(String),
            name: r.name,
            description: "",
            isFavorite: false,
            isPersonal: false,
            owner: expect.any(Object)
          }
        })
      expectedWorkspaces.unshift({
        id: expect.any(String),
        name: PersonalWorkspaceName,
        description: expect.any(String),
        isFavorite: false,
        isPersonal: true,
        owner: expect.any(Object)
      })

      const json = await response.json()
      expect(json).toStrictEqual({
        workspaces: expectedWorkspaces,
        totalCount: totalCount
      })
      expect(json.workspaces).toHaveLength(totalCount < pageSize ? totalCount : pageSize)
    })
  })

  test('should create workspace', async ({ request }) => {
    const createWorkspaceRequest: CreateWorkspaceRequest = {
      name: "New Workspace"
    }
    const response = await request.post('/api/workspaces', {
      ...useToken(),
      data: createWorkspaceRequest
    })

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toStrictEqual({
      message: expect.any(String)
    })

    const workspaces = await getWorkspaces(request)
    expect(workspaces).toStrictEqual(expect.arrayContaining([
      {
        id: expect.any(String),
        name: createWorkspaceRequest.name,
        description: "",
        isFavorite: false,
        isPersonal: false,
        owner: expect.any(Object)
      }
    ]))
  })

  test('should update workspace', async ({ request }) => {
    const oldBody: CreateWorkspaceRequest = {
      name: "Some workspace",
    }
    const workspace = await createWorkspace(request, oldBody)

    const body: UpdateWorkspaceRequest = {
      id: workspace.id,
      name: "New Workspace Name",
      isFavorite: true
    }
    const response = await request.put('/api/workspaces', {
      ...useToken(),
      data: body
    })

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toStrictEqual({
      message: expect.any(String)
    })

    const newWorkspace = await getWorkspace(request, body.id)
    expect(newWorkspace).toStrictEqual({
      name: body.name,
      isFavorite: body.isFavorite,
      isPersonal: false,
      owner: expect.any(Object)
    })
  })

  test('should update workspace overview', async ({ request }) => {
    const oldBody: CreateWorkspaceRequest = {
      name: "Some workspace",
    }
    const workspace = await createWorkspace(request, oldBody)

    const body: UpdateOverviewWorkspaceRequest = {
      id: workspace.id,
      description: "This is a new workspace description"
    }
    const response = await request.put('/api/workspaces/overview', {
      ...useToken(),
      data: body
    })

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toStrictEqual({
      message: expect.any(String)
    })

    const newWorkspace = await getWorkspaceOverview(request, body.id)
    expect(newWorkspace).toStrictEqual({
      description: body.description,
    })
  })

  test('should delete workspace', async ({ request }) => {
    const body: CreateWorkspaceRequest = {
      name: "New Team",
    }
    const workspace = await createWorkspace(request, body)

    const response = await request.delete(`/api/workspaces/${workspace.id}`, useToken())

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toStrictEqual({
      message: expect.any(String)
    })

    const newWorkspaces = await getWorkspaces(request)
    expect(newWorkspaces).not.toStrictEqual(expect.arrayContaining([workspace]))
  })
})