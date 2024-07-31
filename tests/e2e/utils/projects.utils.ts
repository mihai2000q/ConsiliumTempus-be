import { APIRequestContext, expect } from "@playwright/test";
import { useToken } from "./utils";
import CreateProjectRequest from "../types/requests/project/CreateProjectRequest";
import {
  acceptInvitation,
  addCollaboratorToWorkspace,
  createWorkspace,
  getPersonalWorkspace, inviteCollaborator,
  updateCollaborator
} from "./workspaces.utils";
import AddStatusToProjectRequest from "../types/requests/project/AddStatusToProjectRequest";
import AddAllowedMemberToProjectRequest from "../types/requests/project/AddAllowedMemberToProjectRequest";
import { getCurrentUser, registerUser } from "./users.utils";

export async function getProject(request: APIRequestContext, projectId: string) {
  const response = await request.get(`/api/projects/${projectId}`, useToken())
  expect(response.ok()).toBeTruthy()
  return await response.json()
}

export async function getProjectOverview(request: APIRequestContext, projectId: string) {
  const response = await request.get(`/api/projects/overview/${projectId}`, useToken())
  expect(response.ok()).toBeTruthy()
  return await response.json()
}

export async function getProjects(request: APIRequestContext, token?: string | undefined) {
  const response = await request.get('/api/projects', useToken(token))
  expect(response.ok()).toBeTruthy()
  return (await response.json()).projects
}

export async function getProjectStatus(request: APIRequestContext, projectId: string, projectStatusId: string) {
  const response = await request.get(`/api/projects/${projectId}/statuses`, useToken())
  expect(response.ok()).toBeTruthy()
  return (await response.json()).statuses.filter((ps: { id: string }) => ps.id === projectStatusId)[0]
}

export async function getProjectStatuses(request: APIRequestContext, projectId: string) {
  const response = await request.get(`/api/projects/${projectId}/statuses`, useToken())
  expect(response.ok()).toBeTruthy()
  return (await response.json()).statuses
}

export async function getAllowedMembers(request: APIRequestContext, projectId: string) {
  const response = await request.get(`/api/projects/${projectId}/allowed-members`, useToken())
  expect(response.ok()).toBeTruthy()
  return (await response.json()).allowedMembers
}

export async function addAllowedMember(
  request: APIRequestContext,
  addAllowedMemberToProjectRequest: AddAllowedMemberToProjectRequest,
  token?: string | undefined
) {
  const response = await request.post('/api/projects/add-allowed-member', {
    ...useToken(token),
    data: addAllowedMemberToProjectRequest
  });
  expect(response.ok()).toBeTruthy()
}

export async function addAllowedMemberToProject(
  request: APIRequestContext,
  collaboratorEmail: string,
  workspaceId: string
) {
  const createProjectRequest: CreateProjectRequest = {
    workspaceId: workspaceId,
    name: "Project",
    isPrivate: true
  }
  const project = await createProject(request, createProjectRequest)

  const collaborator = await addCollaboratorToWorkspace(request, collaboratorEmail, workspaceId)

  await addAllowedMember(
    request,
    {
      id: project.id,
      collaboratorId: collaborator.id
    }
  )
  return [project, collaborator]
}

export async function addMeToAllowedMembers(
  request: APIRequestContext,
  projectOwnerEmail: string,
  workspaceId: string
) {
  const token = (await registerUser(request, projectOwnerEmail)).token
  await inviteCollaborator(
    request,
    {
      id: workspaceId,
      email: projectOwnerEmail
    }
  )

  await acceptInvitation(request, projectOwnerEmail, workspaceId, token)

  const projectOwner = await getCurrentUser(request, token)

  await updateCollaborator(
    request,
    {
      id: workspaceId,
      collaboratorId: projectOwner.id,
      workspaceRole: 'Admin'
    }
  )

  const project = await createProject(
    request,
    {
      workspaceId: workspaceId,
      name: "Project",
      isPrivate: true
    },
    token
  )
  const me = await getCurrentUser(request)
  await addAllowedMember(
    request,
    {
      id: project.id,
      collaboratorId: me.id,
    },
    token
  )

  return project
}

export async function addProjectStatus(
  request: APIRequestContext,
  body: AddStatusToProjectRequest
) {
  const response = await request.post('/api/projects/add-status', {
    ...useToken(),
    data: body
  })
  expect(response.ok()).toBeTruthy()
  return (await getProjectStatuses(request, body.id)).filter((ps: { title: string }) => ps.title == body.title)[0]
}

export async function createProject(
  request: APIRequestContext,
  body: CreateProjectRequest,
  token?: string | undefined
) {
  const response = await request.post('/api/projects', {
    ...useToken(token),
    data: body
  })
  expect(response.ok()).toBeTruthy()

  return (await getProjects(request, token)).filter((p: { name: string }) => p.name === body.name)[0]
}

export async function create2ProjectsIn2DifferentWorkspaces(request: APIRequestContext) {
  const createProjectRequest1: CreateProjectRequest = {
    workspaceId: (await getPersonalWorkspace(request)).id,
    name: "Unique Project Name",
    isPrivate: false
  }
  await createProject(request, createProjectRequest1)

  const createProjectRequest2 = { ...createProjectRequest1 }
  createProjectRequest2.workspaceId = (await createWorkspace(request)).id
  createProjectRequest2.name = "Project 2"
  await createProject(request, createProjectRequest2)

  return {
    createProjectRequest1: createProjectRequest1,
    createProjectRequest2: createProjectRequest2
  }
}

export async function createProjects(request: APIRequestContext, count: number) {
  const requests = []

  const createProjectRequest1: CreateProjectRequest = {
    workspaceId: (await getPersonalWorkspace(request)).id,
    name: "Project 1",
    isPrivate: false
  }
  await createProject(request, createProjectRequest1)

  requests.push(createProjectRequest1)
  for (let i = 2; i <= count; i++) {
    const createRequest = { ...createProjectRequest1 }
    createRequest.name = "Project " + i
    await createProject(request, createRequest)
    requests.push(createRequest)
  }

  return requests
}