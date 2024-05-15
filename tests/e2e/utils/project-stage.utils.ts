import { APIRequestContext, expect } from "@playwright/test";
import { useToken } from "./utils";
import CreateProjectStageRequest from "../types/requests/project-stage/CreateProjectStageRequest";

export async function getProjectStages(request: APIRequestContext, projectSprintId: string) {
  const response = await request.get(`/api/projects/stages?projectSprintId=${projectSprintId}`, useToken())
  expect(response.ok()).toBeTruthy()
  return (await response.json()).stages
}

export async function createProjectStage(
  request: APIRequestContext,
  body: CreateProjectStageRequest,
) {
  const response = await request.post('/api/projects/stages', {
    ...useToken(),
    data: body
  })
  expect(response.ok()).toBeTruthy()

  return (await getProjectStages(request, body.projectSprintId)).filter((ps: { name: string }) => ps.name === body.name)[0]
}