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
  const newWorkspace = await createWorkspace(request, { name: "Unique Workspace Name", description: ""})

  const createProjectRequest1: CreateProjectRequest = {
    workspaceId: newWorkspace.id,
    name: "Unique Project Name",
    description: "some description",
    isPrivate: false
  }
  await createProject(request, createProjectRequest1)

  const createProjectRequest2 = { ...createProjectRequest1 }
  createProjectRequest2.workspaceId = (await getPersonalWorkspace(request)).id
  createProjectRequest2.name = "Project 2"
  await createProject(request, createProjectRequest2)

  return {
    createProjectRequest1: createProjectRequest1,
    createProjectRequest2: createProjectRequest2
  }
}