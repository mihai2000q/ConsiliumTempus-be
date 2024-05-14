import { APIRequestContext, expect } from "@playwright/test";
import { useToken } from "./utils";
import CreateProjectSprintRequest from "../types/requests/project-sprint/CreateProjectSprintRequest";

export async function getProjectSprints(request: APIRequestContext, projectId: string) {
  const response = await request.get(`/api/projects/${projectId}`, useToken())
  expect(response.ok()).toBeTruthy()
  return (await response.json()).sprints
}

export async function createProjectSprint(
  request: APIRequestContext,
  body: CreateProjectSprintRequest,
) {
  const response = await request.post('/api/projects/sprints', {
    ...useToken(),
    data: body
  })
  expect(response.ok()).toBeTruthy()

  return (await getProjectSprints(request, body.projectId)).filter((ps: { name: string }) => ps.name === body.name)[0]
}