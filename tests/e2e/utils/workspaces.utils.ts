import { APIRequestContext, expect } from "@playwright/test";
import { useToken } from "./utils";
import CreateWorkspaceRequest from "../types/requests/workspace/CreateWorkspaceRequest";
import { getCurrentUser, registerUser } from "./users.utils";
import InviteCollaboratorToWorkspaceRequest from "../types/requests/workspace/InviteCollaboratorToWorkspaceRequest";
import AcceptInvitationToWorkspaceRequest from "../types/requests/workspace/AcceptInvitationToWorkspaceRequest";
import UpdateCollaboratorFromWorkspaceRequest from "../types/requests/workspace/UpdateCollaboratorFromWorkspaceRequest";

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
    `/api/workspaces/${workspaceId}/collaborators?orderBy=user_name.asc&orderBy=user_email.asc`,
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

export async function getInvitations(request: APIRequestContext, token?: string | undefined) {
  const response = await request.get(
    `/api/workspaces/invitations?isSender=false`,
    useToken(token)
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

export async function acceptInvitation(
  request: APIRequestContext,
  collaboratorEmail: string,
  workspaceId: string,
  token?: string | undefined
) {
  const invitation = (await getInvitations(request, token))
    .filter((i: { collaborator: { email: string } }) => i.collaborator.email === collaboratorEmail)[0]

  const acceptInvitationToWorkspaceRequest: AcceptInvitationToWorkspaceRequest = {
    id: workspaceId,
    invitationId: invitation.id,
  }
  const response = await request.post(`/api/workspaces/accept-invitation`, {
      ...useToken(token),
      data: acceptInvitationToWorkspaceRequest
    }
  )

  expect(response.ok()).toBeTruthy()
}

export async function inviteMeToWorkspace(
  request: APIRequestContext,
  senderEmail: string,
  myEmail: string
) {
  const token = (await registerUser(request, senderEmail)).token
  const workspace = await createWorkspace(request, { name: "yet another workspace" }, token)
  await inviteCollaborator(
    request,
    {
      id: workspace.id,
      email: myEmail
    },
    token
  )
  await acceptInvitation(request, myEmail, workspace.id)

  return workspace
}

export async function inviteThemToWorkspace(
  request: APIRequestContext,
  collaboratorEmail: string
) {
  const token = (await registerUser(request, collaboratorEmail)).token
  const workspace = await createWorkspace(request, { name: "yet another workspace" })
  await inviteCollaborator(
    request,
    {
      id: workspace.id,
      email: collaboratorEmail
    }
  )

  await acceptInvitation(request, collaboratorEmail, workspace.id, token)

  const user = await getCurrentUser(request, token)
  return [workspace, user]
}

export async function addCollaboratorToWorkspace(
  request: APIRequestContext,
  collaboratorEmail: string,
  workspaceId: string
) {
  const token = (await registerUser(request, collaboratorEmail)).token
  await inviteCollaborator(
    request,
    {
      id: workspaceId,
      email: collaboratorEmail
    }
  )

  await acceptInvitation(request, collaboratorEmail, workspaceId, token)

  return await getCurrentUser(request, token)
}

export async function updateCollaborator(
  request: APIRequestContext,
  updateCollaboratorFromWorkspaceRequest: UpdateCollaboratorFromWorkspaceRequest
) {
  const response = await request.put(`/api/workspaces/collaborators`, {
    ...useToken(),
    data: updateCollaboratorFromWorkspaceRequest
  })

  expect(response.ok()).toBeTruthy()
}