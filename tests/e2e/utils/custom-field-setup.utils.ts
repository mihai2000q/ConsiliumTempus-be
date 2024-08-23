import { APIRequestContext, expect } from "@playwright/test";
import { useToken } from "./utils";
import CreateCustomFieldSetupOnProjectRequest
  from "../types/requests/custom-field-setup/CreateCustomFieldSetupOnProjectRequest";
import CreateCustomFieldSetupOnWorkspaceRequest
  from "../types/requests/custom-field-setup/CreateCustomFieldSetupOnWorkspaceRequest";

export async function getCustomFieldSetupsFromWorkspace(request: APIRequestContext, workspaceId: string) {
  const response = await request.get(`/api/customFieldSetups/workspace/${workspaceId}`, useToken())
  expect(response.ok()).toBeTruthy()
  return (await response.json()).customFieldSetups
}

export async function getCustomFieldSetupsFromProject(request: APIRequestContext, projectId: string) {
  const response = await request.get(`/api/customFieldSetups/project/${projectId}`, useToken())
  expect(response.ok()).toBeTruthy()
  return (await response.json()).customFieldSetups
}

export async function createCustomFieldSetupOnWorkspace(
  request: APIRequestContext,
  body: CreateCustomFieldSetupOnWorkspaceRequest,
) {
  const response = await request.post('/api/customFieldSetups/workspace', {
    ...useToken(),
    data: body
  })
  expect(response.ok()).toBeTruthy()

  return (await getCustomFieldSetupsFromWorkspace(request, body.workspaceId))
    .filter((cfs: { name: string }) => cfs.name === body.name)[0]
}

export async function createCustomFieldSetupOnProject(
  request: APIRequestContext,
  body: CreateCustomFieldSetupOnProjectRequest,
) {
  const response = await request.post('/api/customFieldSetups/project', {
    ...useToken(),
    data: body
  })
  expect(response.ok()).toBeTruthy()

  return (await getCustomFieldSetupsFromProject(request, body.projectId))
    .filter((cfs: { name: string }) => cfs.name === body.name)[0]
}