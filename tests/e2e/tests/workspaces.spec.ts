import { test } from "@playwright/test";
import { useToken } from "../utils/utils";
import { expect } from "../utils/matchers";
import { createWorkspace, createWorkspaces, getWorkspaces } from "../utils/workspaces.utils";
import { PersonalWorkspaceName } from "../utils/constants";
import { deleteUser, registerUser } from "../utils/users.utils";
import CreateWorkspaceRequest from "../types/requests/workspace/CreateWorkspaceRequest";
import UpdateWorkspaceRequest from "../types/requests/workspace/UpdateWorkspaceRequest";

test.describe('should allow operations on the workspace entity', () => {

  test.beforeEach('should register and create token', async ({ request }) => {
    process.env.API_TOKEN = (await registerUser(request)).token
  })

  test.afterEach('should delete user', async ({ request }) => {
    await deleteUser(request)
  })

  test('should get workspace', async ({ request }) => {
    const workspaces = await getWorkspaces(request);
    const workspace = workspaces[0]

    const response = await request.get(`/api/workspaces/${workspace.id}`, useToken())

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toStrictEqual({
      name: workspace.name,
      description: workspace.description,
    })
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
            description: expect.any(String)
          },
          {
            id: expect.any(String),
            name: createWorkspaceRequest.name,
            description: ""
          }
        ]),
        totalCount: 2,
        totalPages: null
      })
      expect(json.workspaces).toHaveLength(2)
    })

    test('should get collection of workspaces ordered by create date time descending', async ({ request }) => {
      const createWorkspaceRequests = await createWorkspaces(request, 2)

      const response = await request.get('/api/workspaces?order=created_date_time.desc', useToken())

      expect(response.ok()).toBeTruthy()

      const json = await response.json()
      expect(json).toStrictEqual({
        workspaces: [
          {
            id: expect.any(String),
            name: createWorkspaceRequests[1].name,
            description: ""
          },
          {
            id: expect.any(String),
            name: createWorkspaceRequests[0].name,
            description: ""
          },
          {
            id: expect.any(String),
            name: PersonalWorkspaceName,
            description: expect.any(String)
          }
        ],
        totalCount: 3,
        totalPages: null
      })
      expect(json.workspaces).toHaveLength(3)
    })

    test('should get collection of workspaces filtered by name', async ({ request }) => {
      const createWorkspaceRequest: CreateWorkspaceRequest = {
        name: "Some Workspace",
      }
      await createWorkspace(request, createWorkspaceRequest)

      const response = await request.get('/api/workspaces?name=some works', useToken())

      expect(response.ok()).toBeTruthy()

      const json = await response.json()
      expect(json).toStrictEqual({
        workspaces: [
          {
            id: expect.any(String),
            name: createWorkspaceRequest.name,
            description: ""
          }
        ],
        totalCount: 1,
        totalPages: null
      })
      expect(json.workspaces).toHaveLength(1)
    })

    test('should get collection of workspaces paginated and ordered by name ascending', async ({ request }) => {
      const totalCount = 5
      const createWorkspaceRequests = await createWorkspaces(request, totalCount - 1)

      const pageSize = 6
      const currentPage = 1
      const response = await request.get(`/api/workspaces?order=name.asc&pageSize=${pageSize}&currentPage=${currentPage}`, useToken())

      expect(response.ok()).toBeTruthy()

      const start = pageSize * (currentPage - 1)
      const expectedWorkspaces: any[] = createWorkspaceRequests
        .slice(start, start + pageSize)
        .map(r => {
          return {
            id: expect.any(String),
            name: r.name,
            description: "",
          }
        })
      expectedWorkspaces.unshift({
        id: expect.any(String),
        name: PersonalWorkspaceName,
        description: expect.any(String)
      })

      const json = await response.json()
      expect(json).toStrictEqual({
        workspaces: expectedWorkspaces,
        totalCount: totalCount,
        totalPages: Math.ceil(totalCount / pageSize),
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
        description: ""
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
      description: "This is a new workspace description"
    }
    const response = await request.put('/api/workspaces', {
      ...useToken(),
      data: body
    })

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toStrictEqual({
      message: expect.any(String)
    })

    const newWorkspaces = await getWorkspaces(request)
    expect(newWorkspaces).not.toStrictEqual(expect.arrayContaining([workspace]))
    expect(newWorkspaces).toStrictEqual(expect.arrayContaining([
      {
        id: body.id,
        name: body.name,
        description: body.description
      }
    ]))
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