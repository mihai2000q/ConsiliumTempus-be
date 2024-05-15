import { test } from "@playwright/test";
import { useToken } from "../utils/utils";
import { expect } from "../utils/matchers";
import { deleteUser, registerUser } from "../utils/users.utils";
import { getPersonalWorkspace } from "../utils/workspaces.utils";
import { createProject } from "../utils/projects.utils";
import CreateProjectStageRequest from "../types/requests/project-stage/CreateProjectStageRequest";
import { createProjectStage, getProjectStages } from "../utils/project-stage.utils";
import UpdateProjectStageRequest from "../types/requests/project-stage/UpdateProjectStageRequest";
import { createProjectSprint } from "../utils/project-sprint.utils";

test.describe('should allow operations on the project stage entity', () => {
  let SPRINT_ID: string

  test.beforeEach('should register user and get project sprint id', async ({ request }) => {
    process.env.API_TOKEN = (await registerUser(request)).token
    const workspace = await getPersonalWorkspace(request)
    const project = await createProject(request, {
      workspaceId: workspace.id,
      name: "Project name",
      isPrivate: true
    })
    SPRINT_ID = (await createProjectSprint(request, {
      projectId: project.id,
      name: "Sprint 2",
      startDate: null,
      endDate: null,
    })).id
  })

  test.afterEach('should delete user', async ({ request }) => {
    await deleteUser(request)
  })

  test('should get collection of projects', async ({ request }) => {
    const createProjectStageRequest: CreateProjectStageRequest = {
      projectSprintId: SPRINT_ID,
      name: "In Transit",
    }
    const stage = await createProjectStage(request, createProjectStageRequest)

    const response = await request.get(`/api/projects/stages?projectSprintId=${SPRINT_ID}`, useToken())

    expect(response.ok()).toBeTruthy()

    const json = await response.json()
    expect(json).toStrictEqual({
      stages: [
        {
          id: stage.id,
          name: createProjectStageRequest.name
        }
      ]
    })
    expect(json.stages).toHaveLength(1)
  })

  test('should create project stage', async ({ request }) => {
    const body: CreateProjectStageRequest = {
      projectSprintId: SPRINT_ID,
      name: "In Transit"
    }
    const response = await request.post('/api/projects/stages', {
      ...useToken(),
      data: body
    });

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toStrictEqual({
      message: expect.any(String)
    })

    const stages = await getProjectStages(request, SPRINT_ID)
    expect(stages).toHaveLength(1)
    expect(stages).toStrictEqual(expect.arrayContaining([
      {
        id: expect.any(String),
        name: body.name
      }
    ]))
  })

  test('should update project stage', async ({ request }) => {
    const createProjectStageRequest: CreateProjectStageRequest = {
      projectSprintId: SPRINT_ID,
      name: "In Transit"
    }
    const stage = await createProjectStage(request, createProjectStageRequest)

    const body: UpdateProjectStageRequest = {
      id: stage.id,
      name: "Staging"
    }
    const response = await request.put('/api/projects/stages', {
      ...useToken(),
      data: body
    });

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toStrictEqual({
      message: expect.any(String)
    })

    const stages = await getProjectStages(request, SPRINT_ID)
    expect(stages).toHaveLength(1)
    expect(stages).not.toStrictEqual(expect.arrayContaining([stage]))
    expect(stages).toStrictEqual(expect.arrayContaining([
      {
        id: body.id,
        name: body.name
      }
    ]))
  })

  test('should delete project stage', async ({ request }) => {
    const stage = await createProjectStage(request, {
      projectSprintId: SPRINT_ID,
      name: "In Transit"
    })

    const response = await request.delete(`/api/projects/stages/${stage.id}`, useToken());

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toStrictEqual({
      message: expect.any(String)
    })

    const sprints = await getProjectStages(request, SPRINT_ID)
    expect(sprints).toHaveLength(0)
    expect(sprints).not.toStrictEqual(expect.arrayContaining([
      {
        id: expect.any(String),
        name: stage.name
      }
    ]))
  })
})