import { APIRequestContext, expect } from "@playwright/test";
import { useToken } from "./utils";
import CreateProjectSprintRequest from "../types/requests/project-sprint/CreateProjectSprintRequest";
import AddStageToProjectSprintRequest from "../types/requests/project-sprint/AddStageToProjectSprintRequest";

export async function getProjectSprints(request: APIRequestContext, projectId: string) {
  const response = await request.get(`/api/projects/sprints?projectId=${projectId}`, useToken())
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

export async function getProjectStages(request: APIRequestContext, projectSprintId: string) {
  const response = await request.get(`/api/projects/sprints/${projectSprintId}`, useToken())
  expect(response.ok()).toBeTruthy()
  return (await response.json()).stages
}

export async function addStageToProjectSprint(
  request: APIRequestContext,
  body: AddStageToProjectSprintRequest,
) {
  const response = await request.post('/api/projects/sprints/add-stage', {
    ...useToken(),
    data: body
  })
  expect(response.ok()).toBeTruthy()

  return (await getProjectStages(request, body.id)).filter((ps: { name: string }) => ps.name === body.name)[0]
}
