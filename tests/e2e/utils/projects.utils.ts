import { APIRequestContext, expect } from "@playwright/test";
import { useToken } from "./utils";
import CreateProjectRequest from "../types/requests/project/CreateProjectRequest";
import { createWorkspace, getPersonalWorkspace } from "./workspaces.utils";
import AddStatusToProjectRequest from "../types/requests/project/AddStatusToProjectRequest";

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

export async function getProjects(request: APIRequestContext) {
  const response = await request.get('/api/projects', useToken())
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
  body: CreateProjectRequest
) {
  const response = await request.post('/api/projects', {
    ...useToken(),
    data: body
  })
  expect(response.ok()).toBeTruthy()

  return (await getProjects(request)).filter((p: { name: string }) => p.name === body.name)[0]
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