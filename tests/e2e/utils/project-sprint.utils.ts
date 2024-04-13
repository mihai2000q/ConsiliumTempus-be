import { APIRequestContext, expect } from "@playwright/test";
import { useToken } from "./utils";
import CreateProjectSprintRequest from "../types/requests/project-sprint/CreateProjectSprintRequest";

export async function getProjectSprints(request: APIRequestContext) {
  return [{ id: "", name: "name"}]
}

export async function createProjectSprint(
  request: APIRequestContext,
  body: CreateProjectSprintRequest,
) {
  const response = await request.post('/api/project/sprints', {
    ...useToken(),
    data: body
  })
  expect(response.ok()).toBeTruthy()

  return (await getProjectSprints(request)).filter((ps: { name: string }) => ps.name === body.name)[0]
}