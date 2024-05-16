import { test } from "@playwright/test";
import { useToken } from "../utils/utils";
import { expect } from "../utils/matchers";
import { deleteUser, registerUser } from "../utils/users.utils";
import { getPersonalWorkspace } from "../utils/workspaces.utils";
import { createProject } from "../utils/projects.utils";
import {
  addStageToProjectSprint,
  createProjectSprint,
  getProjectSprints,
  getProjectStages
} from "../utils/project-sprint.utils";
import CreateProjectSprintRequest from "../types/requests/project-sprint/CreateProjectSprintRequest";
import UpdateProjectSprintRequest from "../types/requests/project-sprint/UpdateProjectSprintRequest";
import { ProjectSprintName } from "../utils/constants";
import AddStageToProjectSprintRequest from "../types/requests/project-sprint/AddStageToProjectSprintRequest";
import UpdateStageFromProjectSprintRequest from "../types/requests/project-sprint/UpdateStageFromProjectSprintRequest";

test.describe('should allow operations on the project sprint entity', () => {
  let PROJECT_ID: string

  test.beforeEach('should register user and get project id', async ({ request }) => {
    process.env.API_TOKEN = (await registerUser(request)).token
    const workspace = await getPersonalWorkspace(request)
    PROJECT_ID = (await createProject(request, {
      workspaceId: workspace.id,
      name: "Project name",
      isPrivate: true
    })).id
  })

  test.afterEach('should delete user', async ({ request }) => {
    await deleteUser(request)
  })

  test('should get project sprint', async ({ request }) => {
    const createProjectSprintRequest: CreateProjectSprintRequest = {
      projectId: PROJECT_ID,
      name: "Sprint Name",
      startDate: "2024-01-12",
      endDate: "2024-01-26"
    }
    const projectSprint = await createProjectSprint(request, createProjectSprintRequest)

    const response = await request.get(`/api/projects/sprints/${projectSprint.id}`, useToken())

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toStrictEqual({
      name: createProjectSprintRequest.name,
      startDate: createProjectSprintRequest.startDate,
      endDate: createProjectSprintRequest.endDate,
      stages: []
    })
  })

  test('should get collection of project sprints', async ({ request }) => {
    const projectSprint = await createProjectSprint(request, {
      projectId: PROJECT_ID,
      name: "Sprint Name",
      startDate: "2024-01-12",
      endDate: "2024-01-26"
    })

    const response = await request.get(`/api/projects/sprints?projectId=${PROJECT_ID}`, useToken())

    expect(response.ok()).toBeTruthy()

    const json = await response.json()
    expect(json).toStrictEqual({
      sprints: [
        {
          id: expect.any(String),
          name: ProjectSprintName,
          startDate: null,
          endDate: null,
        },
        {
          id: expect.any(String),
          name: projectSprint.name,
          startDate: projectSprint.startDate,
          endDate: projectSprint.endDate,
        }
      ]
    })
    expect(json.sprints).toHaveLength(2);
  })

  test('should create project sprint', async ({ request }) => {
    const body: CreateProjectSprintRequest = {
      projectId: PROJECT_ID,
      name: "Sprint 2",
      startDate: "2024-01-12",
      endDate: "2024-01-26"
    }
    const response = await request.post('/api/projects/sprints', {
      ...useToken(),
      data: body
    });

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toStrictEqual({
      message: expect.any(String)
    })

    const sprints = await getProjectSprints(request, PROJECT_ID)
    expect(sprints).toHaveLength(2)
    expect(sprints).toStrictEqual([
      {
        id: expect.any(String),
        name: ProjectSprintName,
        startDate: null,
        endDate: null,
      },
      {
        id: expect.any(String),
        name: body.name,
        startDate: body.startDate,
        endDate: body.endDate
      }
    ])
  })

  test('should add stage to project sprint', async ({ request }) => {
    const createProjectSprintRequest: CreateProjectSprintRequest = {
      projectId: PROJECT_ID,
      name: "Sprint 2",
      startDate: "2024-01-12",
      endDate: "2024-01-26"
    }
    const sprint = await createProjectSprint(request, createProjectSprintRequest)

    const body: AddStageToProjectSprintRequest = {
      id: sprint.id,
      name: "In Transit",
      onTop: true
    }
    const response = await request.post('/api/projects/sprints/add-stage', {
      ...useToken(),
      data: body
    });

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toStrictEqual({
      message: expect.any(String)
    })

    const stages = await getProjectStages(request, body.id)
    expect(stages).toHaveLength(1)
    expect(stages).toStrictEqual([
      {
        id: expect.any(String),
        name: body.name,
      }
    ])
  })

  test('should update project sprint', async ({ request }) => {
    const createProjectSprintRequest: CreateProjectSprintRequest = {
      projectId: PROJECT_ID,
      name: "Sprint 2",
      startDate: "2024-01-12",
      endDate: "2024-01-26"
    }
    const sprint = await createProjectSprint(request, createProjectSprintRequest)

    const body: UpdateProjectSprintRequest = {
      id: sprint.id,
      name: "Sprint 2 - Updated",
      startDate: null,
      endDate: null
    }
    const response = await request.put('/api/projects/sprints', {
      ...useToken(),
      data: body
    });

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toStrictEqual({
      message: expect.any(String)
    })

    const sprints = await getProjectSprints(request, PROJECT_ID)
    expect(sprints).toHaveLength(2)
    expect(sprints).not.toStrictEqual(expect.arrayContaining([sprint]))
    expect(sprints).toStrictEqual([
      {
        id: expect.any(String),
        name: ProjectSprintName,
        startDate: null,
        endDate: null,
      },
      {
        id: body.id,
        name: body.name,
        startDate: body.startDate,
        endDate: body.endDate
      }
    ])
  })

  test('should update stage from project sprint', async ({ request }) => {
    const createProjectSprintRequest: CreateProjectSprintRequest = {
      projectId: PROJECT_ID,
      name: "Sprint 2",
      startDate: "2024-01-12",
      endDate: "2024-01-26"
    }
    const sprint = await createProjectSprint(request, createProjectSprintRequest)

    const addStageToProjectSprintRequest: AddStageToProjectSprintRequest = {
      id: sprint.id,
      name: "In Transit",
      onTop: true
    }
    const stage = await addStageToProjectSprint(request, addStageToProjectSprintRequest)

    const body: UpdateStageFromProjectSprintRequest = {
      id: sprint.id,
      stageId: stage.id,
      name: "Staging"
    }
    const response = await request.put('/api/projects/sprints/update-stage', {
      ...useToken(),
      data: body
    });

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toStrictEqual({
      message: expect.any(String)
    })

    const stages = await getProjectStages(request, body.id)
    expect(stages).not.toStrictEqual([
      {
        id: stage.id,
        name: createProjectSprintRequest.name,
      }
    ])
    expect(stages).toStrictEqual([
      {
        id: stage.id,
        name: body.name,
      }
    ])
  })

  test('should delete project sprint', async ({ request }) => {
    const projectSprint = await createProjectSprint(request, {
      projectId: PROJECT_ID,
      name: "Project Sprint Name",
      startDate: "2024-01-12",
      endDate: "2024-01-26"
    })

    const response = await request.delete(`/api/projects/sprints/${projectSprint.id}`, useToken());

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toStrictEqual({
      message: expect.any(String)
    })

    const sprints = await getProjectSprints(request, PROJECT_ID)
    expect(sprints).toHaveLength(1)
    expect(sprints).not.toStrictEqual(expect.arrayContaining([
      {
        id: expect.any(String),
        name: projectSprint.name,
        startDate: projectSprint.startDate,
        endDate: projectSprint.endDate
      }
    ]))
    expect(sprints).toStrictEqual([
      {
        id: expect.any(String),
        name: ProjectSprintName,
        startDate: null,
        endDate: null,
      }
    ])
  })

  test('should remove stage from project sprint', async ({ request }) => {
    const createProjectSprintRequest: CreateProjectSprintRequest = {
      projectId: PROJECT_ID,
      name: "Sprint 2",
      startDate: "2024-01-12",
      endDate: "2024-01-26"
    }
    const sprint = await createProjectSprint(request, createProjectSprintRequest)

    const addStageToProjectSprintRequest: AddStageToProjectSprintRequest = {
      id: sprint.id,
      name: "In Transit",
      onTop: true
    }
    const stage = await addStageToProjectSprint(request, addStageToProjectSprintRequest)

    const response = await request.delete(
      `/api/projects/sprints/${sprint.id}/remove-stage/${stage.id}`,
      useToken());

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toStrictEqual({
      message: expect.any(String)
    })

    const stages = await getProjectStages(request, sprint.id)
    expect(stages).toHaveLength(0)
  })
})