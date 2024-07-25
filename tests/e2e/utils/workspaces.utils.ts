import { APIRequestContext, expect } from "@playwright/test";
import { useToken } from "./utils";
import CreateWorkspaceRequest from "../types/requests/workspace/CreateWorkspaceRequest";
import { registerUser } from "./users.utils";
import InviteCollaboratorToWorkspaceRequest from "../types/requests/workspace/InviteCollaboratorToWorkspaceRequest";
import AcceptInvitationToWorkspaceRequest from "../types/requests/workspace/AcceptInvitationToWorkspaceRequest";

export async function getPersonalWorkspace(request: APIRequestContext, token?: string | undefined) {
  const response = await request.get('/api/workspaces?isPersonalWorkspaceFirst=true', useToken(token))
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

export async function getWorkspaces(request: APIRequestContext, token?: string | undefined) {
  const response = await request.get('/api/workspaces', useToken(token))
  expect(response.ok()).toBeTruthy()
  return (await response.json()).workspaces
}

export async function createWorkspace(
  request: APIRequestContext,
  body: CreateWorkspaceRequest = {
    name: "Workspace name"
  },
  token?: string | undefined
) {
  const response = await request.post('/api/workspaces', {
    ...useToken(token),
    data: body
  })
  expect(response.ok()).toBeTruthy()

  return (await getWorkspaces(request, token)).filter((w: { name: string }) => w.name === body.name)[0]
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

export async function inviteCollaboratorWithRegister(
  request: APIRequestContext,
  email: string,
  firstName: string,
  lastName: string,
  workspaceId: string
) {
  await registerUser(
    request,
    email,
    "Some Password 123",
    firstName,
    lastName,
  )

  await inviteCollaborator(request, {
    id: workspaceId,
    email: email,
  })

  return (await getInvitationsByWorkspace(request, workspaceId))
    .filter((i: { collaborator: { email: string } }) => i.collaborator.email === email)[0]
}

export async function inviteCollaborator(
  request: APIRequestContext,
  inviteCollaboratorToWorkspaceRequest: InviteCollaboratorToWorkspaceRequest,
  token?: string | undefined
) {
  const response = await request.post(
    `/api/workspaces/invite-collaborator`, {
      ...useToken(token),
      data: inviteCollaboratorToWorkspaceRequest
    }
  )

  expect(response.ok()).toBeTruthy()
}

export async function getCollaborators(request: APIRequestContext, workspaceId: string) {
  const response = await request.get(
    `/api/workspaces/${workspaceId}/collaborators`,
    useToken()
  )
  expect(response.ok()).toBeTruthy()

  return (await response.json()).collaborators
}

export async function getInvitationsByWorkspace(request: APIRequestContext, workspaceId: string) {
  const response = await request.get(
    `/api/workspaces/invitations?workspaceId=${workspaceId}`,
    useToken()
  )
  expect(response.ok()).toBeTruthy()

  return (await response.json()).invitations
}

export async function getInvitations(request: APIRequestContext) {
  const response = await request.get(
    `/api/workspaces/invitations?isSender=false`,
    useToken()
  )
  expect(response.ok()).toBeTruthy()

  return (await response.json()).invitations
}

export async function getInvitation(request: APIRequestContext, workspaceId: string) {
  const response = await request.get(
    `/api/workspaces/invitations?isSender=false`,
    useToken()
  )
  expect(response.ok()).toBeTruthy()

  return (await response.json())
    .invitations
    .filter((i: { workspace: { id: string } }) => i.workspace.id === workspaceId)[0]
}

export async function inviteToWorkspace(
  request: APIRequestContext,
  senderEmail: string,
  collaboratorEmail: string
) {
  const token = (await registerUser(request, senderEmail)).token
  const workspace = await createWorkspace(request, { name: "yet another workspace" }, token)
  await inviteCollaborator(
    request,
    {
      id: workspace.id,
      email: collaboratorEmail
    },
    token
  )
  const invitation = (await getInvitations(request))
    .filter((i: { collaborator: { email: string } }) => i.collaborator.email === collaboratorEmail)[0]

  const acceptInvitationToWorkspaceRequest: AcceptInvitationToWorkspaceRequest = {
    id: workspace.id,
    invitationId: invitation.id,
  }
  const response = await request.post(`/api/workspaces/accept-invitation`, {
      ...useToken(),
      data: acceptInvitationToWorkspaceRequest
    }
  )

  expect(response.ok()).toBeTruthy()

  return workspace
}