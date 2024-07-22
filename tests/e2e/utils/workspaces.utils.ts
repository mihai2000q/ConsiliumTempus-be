import { APIRequestContext, expect } from "@playwright/test";
import { useToken } from "./utils";
import CreateWorkspaceRequest from "../types/requests/workspace/CreateWorkspaceRequest";
import { registerUser } from "./users.utils";
import InviteCollaboratorToWorkspaceRequest from "../types/requests/workspace/InviteCollaboratorToWorkspaceRequest";

export async function getPersonalWorkspace(request: APIRequestContext) {
  const response = await request.get('/api/workspaces?isPersonalWorkspaceFirst=true', useToken())
  expect(response.ok()).toBeTruthy()
  const json = await response.json()
  expect(json.workspaces.length).toBeGreaterThanOrEqual(1)
  return json.workspaces[0]
}

export async function getWorkspace(request: APIRequestContext, workspaceId: string) {
  const response = await request.get(`/api/workspaces/${workspaceId}`, useToken())
  expect(response.ok()).toBeTruthy()
  return await response.json()
}

export async function getWorkspaceOverview(request: APIRequestContext, workspaceId: string) {
  const response = await request.get(`/api/workspaces/overview/${workspaceId}`, useToken())
  expect(response.ok()).toBeTruthy()
  return await response.json()
}

export async function getWorkspaces(request: APIRequestContext) {
  const response = await request.get('/api/workspaces', useToken())
  expect(response.ok()).toBeTruthy()
  return (await response.json()).workspaces
}

export async function createWorkspace(
  request: APIRequestContext,
  body: CreateWorkspaceRequest = {
    name: "Workspace name"
  }
) {
  const response = await request.post('/api/workspaces', {
    ...useToken(),
    data: body
  })
  expect(response.ok()).toBeTruthy()

  return (await getWorkspaces(request)).filter((w: { name: string }) => w.name === body.name)[0]
}

export async function createWorkspaces(request: APIRequestContext, count: number) {
  const requests = []

  const createWorkspaceRequest1: CreateWorkspaceRequest = {
    name: "Workspace 1"
  }
  await createWorkspace(request, createWorkspaceRequest1)

  requests.push(createWorkspaceRequest1)
  for (let i = 2; i <= count; i++) {
    const createRequest = { ...createWorkspaceRequest1 }
    createRequest.name = "Workspace " + i
    await createWorkspace(request, createRequest)
    requests.push(createRequest)
  }

  return requests
}

export async function inviteCollaborator(
  request: APIRequestContext,
  email: string,
  firstName: string,
  lastName: string,
  workspaceId: string
) {
  const registerResponse = await registerUser(
    request,
    email,
    "Some Password 123",
    firstName,
    lastName,
  )
  expect(registerResponse.ok()).toBeTruthy()

  const inviteCollaboratorToWorkspaceRequest: InviteCollaboratorToWorkspaceRequest = {
    id: workspaceId,
    email: email,
  }
  const inviteResponse = await request.post(
    `/api/workspaces/invite-collaborator`, {
      ...useToken(),
      data: inviteCollaboratorToWorkspaceRequest
    }
  )
  expect(inviteResponse.ok()).toBeTruthy()
}