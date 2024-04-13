import { test } from "@playwright/test";
import { useToken } from "../utils/utils";
import { expect } from "../utils/matchers";
import { createWorkspace, getWorkspaces } from "../utils/workspaces.utils";
import { deleteUser, registerUser } from "../utils/users.utils";
import CreateWorkspaceRequest from "../types/requests/workspace/CreateWorkspaceRequest";
import UpdateWorkspaceRequest from "../types/requests/project/UpdateWorkspaceRequest";

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

    expect(await response.json()).toEqual({
      name: workspace.name,
      description: workspace.description,
    })
  })

  test('should get collection of workspaces', async ({ request }) => {
    const response = await request.get('/api/workspaces', useToken())

    expect(response.ok()).toBeTruthy()

    const json = await response.json()
    expect(json).toEqual({
      workspaces: expect.arrayContaining([
        {
          id: expect.any(String),
          name: expect.any(String),
          description: expect.any(String)
        }
      ]),
    })
    expect(json.workspaces).toHaveLength(1)
  })

  test('should create workspace', async ({ request }) => {
    const body = {
      name: "New Workspace",
      description: "This is a new workspace"
    }
    const response = await request.post('/api/workspaces', {
      ...useToken(),
      data: body
    })

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toEqual({
      message: expect.any(String)
    })

    const workspaces = await getWorkspaces(request)
    expect(workspaces).toEqual(expect.arrayContaining([
      {
        id: expect.any(String),
        name: body.name,
        description: body.description
      }
    ]))
  })

  test('should update workspace', async ({ request }) => {
    const oldBody: CreateWorkspaceRequest = {
      name: "Some workspace",
      description: "This was the workspace"
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

    expect(await response.json()).toEqual({
      message: expect.any(String)
    })

    const newWorkspaces = await getWorkspaces(request)
    expect(newWorkspaces).not.toEqual(expect.arrayContaining([workspace]))
    expect(newWorkspaces).toEqual(expect.arrayContaining([
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
      description: "This is a new team"
    }
    const workspace = await createWorkspace(request, body)

    const response = await request.delete(`/api/workspaces/${workspace}`, useToken())

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toEqual({
      message: expect.any(String)
    })

    const newWorkspaces = await getWorkspaces(request)
    expect(newWorkspaces).not.toEqual(expect.arrayContaining([workspace]))
  })

})