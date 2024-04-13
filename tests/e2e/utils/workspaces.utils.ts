import { APIRequestContext, expect } from "@playwright/test";
import { useToken } from "./utils";
import CreateWorkspaceRequest from "../types/requests/workspace/CreateWorkspaceRequest";

export async function getPersonalWorkspace(request: APIRequestContext) {
  const response = await request.get('/api/workspaces')
  expect(response.ok()).toBeTruthy()
  const json = await response.json()
  expect(json.workspaces).toHaveLength(1)
  return json.workspaces[0]
}

export async function getWorkspaces(request: APIRequestContext) {
  const response = await request.get('/api/workspaces', useToken())
  expect(response.ok()).toBeTruthy()
  return (await response.json()).workspaces
}

export async function createWorkspace(
  request: APIRequestContext,
  body: CreateWorkspaceRequest = {
    name: "Workspace name",
    description: "This is a workspace description"
  }
) {
  const response = await request.post('/api/workspaces', {
    ...useToken(),
    data: body
  })
  expect(response.ok()).toBeTruthy()

  return (await getWorkspaces(request)).filter((w: { name: string }) => w.name === body.name)[0]
}