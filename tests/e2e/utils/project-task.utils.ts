import { APIRequestContext, expect } from "@playwright/test";
import { useToken } from "./utils";
import CreateProjectTaskRequest from "../types/requests/project-task/CreateProjectTaskRequest";

export async function getProjectTask(request: APIRequestContext, projectTaskId: string) {
  const response = await request.get(`/api/projects/tasks/${projectTaskId}`, useToken())
  expect(response.ok()).toBeTruthy()
  return await response.json()
}

export async function getProjectTasks(request: APIRequestContext, projectStageId: string) {
  const response = await request.get(`/api/projects/tasks?projectStageId=${projectStageId}`, useToken())
  expect(response.ok()).toBeTruthy()
  return (await response.json()).tasks
}

export async function createProjectTask(
  request: APIRequestContext,
  body: CreateProjectTaskRequest,
) {
  const response = await request.post('/api/projects/tasks', {
    ...useToken(),
    data: body
  })
  expect(response.ok()).toBeTruthy()

  return (await getProjectTasks(request, body.projectStageId))
    .filter((ps: { name: string }) => ps.name === body.name)[0]
}
