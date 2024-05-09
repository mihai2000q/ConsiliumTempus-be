import { APIRequestContext, expect } from "@playwright/test";
import { useToken } from "./utils";
import CreateProjectRequest from "../types/requests/project/CreateProjectRequest";
import { createWorkspace, getPersonalWorkspace } from "./workspaces.utils";

export async function getProjectsForUser(request: APIRequestContext) {
  const response = await request.get('/api/projects/user', useToken())
  expect(response.ok()).toBeTruthy()
  return (await response.json()).projects
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

  return (await getProjectsForUser(request)).filter((p: { name: string }) => p.name === body.name)[0]
}

export async function create2ProjectsIn2DifferentWorkspaces(request: APIRequestContext) {
  const createProjectRequest1: CreateProjectRequest = {
    workspaceId: (await getPersonalWorkspace(request)).id,
    name: "Unique Project Name",
    description: "some description",
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
    description: "some description",
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