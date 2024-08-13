import { APIRequestContext, expect } from "@playwright/test";
import { useToken } from "./utils";
import CreateCustomFieldSetupOnProjectRequest
  from "../types/requests/custom-field-setup/CreateCustomFieldSetupOnProjectRequest";

export async function getCustomFieldSetupsFromProject(request: APIRequestContext, projectId?: string) {
  const response = await request.get(`/api/customFieldSetups/project/${projectId}`, useToken())
  expect(response.ok()).toBeTruthy()
  return (await response.json()).customFieldSetups
}

export async function createCustomFieldSetup(
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