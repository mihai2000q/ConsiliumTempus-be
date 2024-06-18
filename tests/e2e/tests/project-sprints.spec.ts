import { test } from "@playwright/test";
import { useToken } from "../utils/utils";
import { expect } from "../utils/matchers";
import { deleteUser, registerUser } from "../utils/users.utils";
import { getPersonalWorkspace } from "../utils/workspaces.utils";
import { createProject, getProjectStatuses } from "../utils/projects.utils";
import {
  addStageToProjectSprint,
  createProjectSprint,
  getProjectSprints,
  getProjectStages
} from "../utils/project-sprint.utils";
import CreateProjectSprintRequest from "../types/requests/project-sprint/CreateProjectSprintRequest";
import UpdateProjectSprintRequest from "../types/requests/project-sprint/UpdateProjectSprintRequest";
import { ProjectSprintName, ProjectStageName1, ProjectStageName2, ProjectStageName3 } from "../utils/constants";
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
      stages: [
        {
          id: expect.any(String),
          name: ProjectStageName1
        }
      ]
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
    expect(json.sprints).toHaveLength(2);
    expect(json).toStrictEqual({
      sprints: [
        {
          id: expect.any(String),
          name: ProjectSprintName,
          startDate: expect.any(String),
          endDate: new Date().toISOString().slice(0, 10),
        },
        {
          id: expect.any(String),
          name: projectSprint.name,
          startDate: projectSprint.startDate,
          endDate: projectSprint.endDate,
        }
      ],
      totalCount: 2
    })
  })

  test.describe(`should allow creation of project sprint`, () => {
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
          startDate: expect.any(String),
          endDate: new Date().toISOString().slice(0, 10),
        },
        {
          id: expect.any(String),
          name: body.name,
          startDate: body.startDate,
          endDate: body.endDate
        }
      ])
    })

    test('should create project sprint and keep previous stages', async ({ request }) => {
      const body: CreateProjectSprintRequest = {
        projectId: PROJECT_ID,
        name: "Sprint 2",
        startDate: "2024-01-12",
        endDate: "2024-01-26",
        keepPreviousStages: true
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
          startDate: expect.any(String),
          endDate: new Date().toISOString().slice(0, 10),
        },
        {
          id: expect.any(String),
          name: body.name,
          startDate: body.startDate,
          endDate: body.endDate
        }
      ])
      const createdSprint = sprints.filter((s: { name: string; }) => s.name == body.name)[0]

      const stages = await getProjectStages(request, createdSprint.id)
      expect(stages).toHaveLength(3);
      expect(stages).toStrictEqual([
        {
          id: expect.any(String),
          name: ProjectStageName1
        },
        {
          id: expect.any(String),
          name: ProjectStageName2
        },
        {
          id: expect.any(String),
          name: ProjectStageName3
        },
      ])
    })

    test('should create project sprint with status', async ({ request }) => {
      const body: CreateProjectSprintRequest = {
        projectId: PROJECT_ID,
        name: "Sprint 2",
        startDate: "2024-01-12",
        endDate: "2024-01-26",
        projectStatus: {
          title: "Status Update",
          status: 'OnTrack',
          description: "This is a new status"
        }
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
          startDate: expect.any(String),
          endDate: new Date().toISOString().slice(0, 10),
        },
        {
          id: expect.any(String),
          name: body.name,
          startDate: body.startDate,
          endDate: body.endDate
        }
      ])

      const statuses = await getProjectStatuses(request, PROJECT_ID)
      expect(statuses).toEqual(
        expect.arrayContaining([
          expect.objectContaining({
            id: expect.any(String),
            title: body.projectStatus?.title,
            status: body.projectStatus?.status,
            description: body.projectStatus?.description
          })
        ])
      )
    })
  })

  test('should add stage to project sprint', async ({ request }) => {
    const createProjectSprintRequest: CreateProjectSprintRequest = {
      projectId: PROJECT_ID,
      name: "Sprint 2",
      startDate: "2024-01-12",
      endDate: "2024-01-26"
    }
    const sprint = await createProjectSprint(request, createProjectSprintRequest)

    const addStageToProjectSprintRequest1: AddStageToProjectSprintRequest = {
      id: sprint.id,
      name: "In Transit1",
      onTop: false
    }
    const response1 = await request.post('/api/projects/sprints/add-stage', {
      ...useToken(),
      data: addStageToProjectSprintRequest1
    });

    const addStageToProjectSprintRequest2: AddStageToProjectSprintRequest = {
      id: sprint.id,
      name: "In Transit2",
      onTop: false
    }
    const response2 = await request.post('/api/projects/sprints/add-stage', {
      ...useToken(),
      data: addStageToProjectSprintRequest2
    });

    const addStageToProjectSprintRequest3: AddStageToProjectSprintRequest = {
      id: sprint.id,
      name: "In Transit3",
      onTop: true
    }
    const response3 = await request.post('/api/projects/sprints/add-stage', {
      ...useToken(),
      data: addStageToProjectSprintRequest3
    });

    expect(response1.ok()).toBeTruthy()
    expect(response2.ok()).toBeTruthy()
    expect(response3.ok()).toBeTruthy()

    expect(await response1.json()).toStrictEqual({
      message: expect.any(String)
    })
    expect(await response2.json()).toStrictEqual({
      message: expect.any(String)
    })
    expect(await response3.json()).toStrictEqual({
      message: expect.any(String)
    })

    const stages = await getProjectStages(request, addStageToProjectSprintRequest1.id)
    expect(stages).toHaveLength(4)
    expect(stages).toStrictEqual([
      {
        id: expect.any(String),
        name: addStageToProjectSprintRequest3.name,
      },
      {
        id: expect.any(String),
        name: ProjectStageName1,
      },
      {
        id: expect.any(String),
        name: addStageToProjectSprintRequest1.name,
      },
      {
        id: expect.any(String),
        name: addStageToProjectSprintRequest2.name,
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
      endDate: new Date().toISOString().slice(0, 10)
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
        startDate: expect.any(String),
        endDate: new Date().toISOString().slice(0, 10),
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
    expect(stages).not.toStrictEqual(expect.arrayContaining([
      {
        id: stage.id,
        name: createProjectSprintRequest.name,
      }
    ]))
    expect(stages).toStrictEqual(expect.arrayContaining([
      {
        id: stage.id,
        name: body.name,
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
    expect(sprints).toStrictEqual([
      {
        id: expect.any(String),
        name: ProjectSprintName,
        startDate: expect.any(String),
        endDate: new Date().toISOString().slice(0, 10),
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
      onTop: false
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
    expect(stages).toHaveLength(1)
    expect(stages).not.toStrictEqual([
      {
        id: expect.any(String),
        name: addStageToProjectSprintRequest.name
      }
    ])
    expect(stages).toStrictEqual([
      {
        id: expect.any(String),
        name: ProjectStageName1
      }
    ])
  })
})