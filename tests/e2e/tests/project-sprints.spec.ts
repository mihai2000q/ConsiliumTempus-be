import { test } from "@playwright/test";
import { useToken } from "../utils/utils";
import { expect } from "../utils/matchers";
import { deleteUser, registerUser } from "../utils/users.utils";
import { getPersonalWorkspace } from "../utils/workspaces.utils";
import { createProject } from "../utils/projects.utils";
import { createProjectSprint, getProjectSprints } from "../utils/project-sprint.utils";
import CreateProjectSprintRequest from "../types/requests/project-sprint/CreateProjectSprintRequest";

test.describe('should allow operations on the project entity', () => {
  let PROJECT_ID: string

  test.beforeEach('should register user and get project id', async ({ request }) => {
    process.env.API_TOKEN = (await registerUser(request)).token
    const workspace = await getPersonalWorkspace(request)
    PROJECT_ID = (await createProject(request, {
      workspaceId: workspace.id,
      name: "Project name",
      description: "",
      isPrivate: true
    })).id
  })

  test.afterEach('should delete user', async ({ request }) => {
    await deleteUser(request)
  })

  test('should get project sprint', async ({ request }) => {
    const createProjectSprintRequest: CreateProjectSprintRequest = {
      projectId: PROJECT_ID,
      name: "Project Sprint Name",
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
    })
  })

  test('should get collection of project sprints', async ({ request }) => {
    const projectSprint = await createProjectSprint(request, {
      projectId: PROJECT_ID,
      name: "Project Sprint Name",
      startDate: "2024-01-12",
      endDate: "2024-01-26"
    })

    const response = await request.get(`/api/projects/sprints?projectId=${PROJECT_ID}`, useToken())

    expect(response.ok()).toBeTruthy()

    const json = await response.json()
    expect(json).toStrictEqual({
      sprints: expect.arrayContaining([
        {
          id: expect.any(String),
          name: projectSprint.name,
          startDate: projectSprint.startDate,
          endDate: projectSprint.endDate,
        }
      ])
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
    expect(sprints).toStrictEqual(expect.arrayContaining([
      {
        id: expect.any(String),
        name: body.name,
        startDate: body.startDate,
        endDate: body.endDate
      }
    ]))
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
  })
})