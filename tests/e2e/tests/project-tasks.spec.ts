import { test } from "@playwright/test";
import { useToken } from "../utils/utils";
import { expect } from "../utils/matchers";
import { deleteUser, registerUser } from "../utils/users.utils";
import { getPersonalWorkspace } from "../utils/workspaces.utils";
import { createProject } from "../utils/projects.utils";
import { getProjectSprints, getProjectStages } from "../utils/project-sprint.utils";
import CreateProjectTaskRequest from "../types/requests/project-task/CreateProjectTaskRequest";
import { createProjectTask, getProjectTask, getProjectTasks } from "../utils/project-task.utils";
import UpdateProjectTaskRequest from "../types/requests/project-task/UpdateProjectTaskRequest";
import UpdateOverviewProjectTaskRequest from "../types/requests/project-task/UpdateOverviewProjectTaskRequest";

test.describe('should allow operations on the project task entity', () => {
  let STAGE_ID: string

  test.beforeEach('should register user and get project id', async ({ request }) => {
    process.env.API_TOKEN = (await registerUser(request)).token
    const workspace = await getPersonalWorkspace(request)
    const project = await createProject(request, {
      workspaceId: workspace.id,
      name: "Project name",
      isPrivate: true
    })
    const sprints = await getProjectSprints(request, project.id)
    const stages = await getProjectStages(request, sprints[0].id)
    STAGE_ID = stages[1].id
  })

  test.afterEach('should delete user', async ({ request }) => {
    await deleteUser(request)
  })

  test('should get project task', async ({ request }) => {
    const createProjectTaskRequest: CreateProjectTaskRequest = {
      projectStageId: STAGE_ID,
      name: "task Name",
      onTop: false
    }
    const projectTask = await createProjectTask(request, createProjectTaskRequest)

    const response = await request.get(`/api/projects/tasks/${projectTask.id}`, useToken())

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toStrictEqual({
      name: createProjectTaskRequest.name,
      description: "",
      isCompleted: false,
      assignee: null,
      stage: expect.any(Object),
      sprint: expect.any(Object),
      project: expect.any(Object),
      workspace: expect.any(Object)
    })
  })

  test.describe('should allow to get collection of project tasks', () => {
    test('should get collection of project tasks', async ({ request }) => {
      const projectTask = await createProjectTask(request, {
        projectStageId: STAGE_ID,
        name: "task Name",
        onTop: false
      })

      const response = await request.get(`/api/projects/tasks?projectStageId=${STAGE_ID}`, useToken())

      expect(response.ok()).toBeTruthy()

      const json = await response.json()
      expect(json.tasks).toHaveLength(1);
      expect(json).toStrictEqual({
        tasks: [
          {
            id: expect.any(String),
            name: projectTask.name,
            isCompleted: false,
            assignee: null
          }
        ],
        totalCount: 1
      })
    })

    test('should get collection of project tasks ordered by name ascending', async ({ request }) => {
      const projectTask2 = await createProjectTask(request, {
        projectStageId: STAGE_ID,
        name: "task Name 2",
        onTop: false
      })
      const projectTask1 = await createProjectTask(request, {
        projectStageId: STAGE_ID,
        name: "task Name 1",
        onTop: false
      })

      const response = await request.get(
        `/api/projects/tasks?projectStageId=${STAGE_ID}&orderBy=name.asc`,
        useToken()
      )

      expect(response.ok()).toBeTruthy()

      const json = await response.json()
      expect(json.tasks).toHaveLength(2);
      expect(json).toStrictEqual({
        tasks: [
          {
            id: expect.any(String),
            name: projectTask1.name,
            isCompleted: false,
            assignee: null
          },
          {
            id: expect.any(String),
            name: projectTask2.name,
            isCompleted: false,
            assignee: null
          }
        ],
        totalCount: 2
      })
    })

    test('should get collection of project tasks filtered by name', async ({ request }) => {
      const projectTask = await createProjectTask(request, {
        projectStageId: STAGE_ID,
        name: "task Name",
        onTop: false
      })

      const response = await request.get(
        `/api/projects/tasks?projectStageId=${STAGE_ID}&search=name ct task`,
        useToken()
      )

      expect(response.ok()).toBeTruthy()

      const json = await response.json()
      expect(json.tasks).toHaveLength(1);
      expect(json).toStrictEqual({
        tasks: [
          {
            id: expect.any(String),
            name: projectTask.name,
            isCompleted: false,
            assignee: null
          }
        ],
        totalCount: 1
      })
    })

    test('should get collection of project tasks paginated', async ({ request }) => {
      const projectTask1 = await createProjectTask(request, {
        projectStageId: STAGE_ID,
        name: "task Name1",
        onTop: false
      })
      const projectTask2 = await createProjectTask(request, {
        projectStageId: STAGE_ID,
        name: "task Name2",
        onTop: false
      })
      await createProjectTask(request, {
        projectStageId: STAGE_ID,
        name: "task Name3",
        onTop: false
      })

      const response = await request.get(
        `/api/projects/tasks?projectStageId=${STAGE_ID}&currentPage=1&pageSize=2`,
        useToken()
      )

      expect(response.ok()).toBeTruthy()

      const json = await response.json()
      expect(json.tasks).toHaveLength(2);
      expect(json).toStrictEqual({
        tasks: [
          {
            id: expect.any(String),
            name: projectTask1.name,
            isCompleted: false,
            assignee: null
          },
          {
            id: expect.any(String),
            name: projectTask2.name,
            isCompleted: false,
            assignee: null
          }
        ],
        totalCount: 3
      })
    })
  })

  test.describe(`should allow creation of project task`, () => {
    test('should create project task', async ({ request }) => {
      const body: CreateProjectTaskRequest = {
        projectStageId: STAGE_ID,
        name: "task 2",
        onTop: false
      }
      const response = await request.post('/api/projects/tasks', {
        ...useToken(),
        data: body
      });

      expect(response.ok()).toBeTruthy()

      expect(await response.json()).toStrictEqual({
        message: expect.any(String)
      })

      const tasks = await getProjectTasks(request, STAGE_ID)
      expect(tasks).toHaveLength(1)
      expect(tasks).toStrictEqual([
        {
          id: expect.any(String),
          name: body.name,
          isCompleted: false,
          assignee: null
        }
      ])
    })

    test('should create project task and place it on top', async ({ request }) => {
      const createProjectTaskRequest1: CreateProjectTaskRequest = {
        projectStageId: STAGE_ID,
        name: "task 1"
      }
      await createProjectTask(request, createProjectTaskRequest1)

      const createProjectTaskRequest2: CreateProjectTaskRequest = {
        projectStageId: STAGE_ID,
        name: "task 1"
      }
      await createProjectTask(request, createProjectTaskRequest2)

      const body: CreateProjectTaskRequest = {
        projectStageId: STAGE_ID,
        name: "task 2",
        onTop: true
      }
      const response = await request.post('/api/projects/tasks', {
        ...useToken(),
        data: body
      });

      expect(response.ok()).toBeTruthy()

      expect(await response.json()).toStrictEqual({
        message: expect.any(String)
      })

      const tasks = await getProjectTasks(request, STAGE_ID)
      expect(tasks).toHaveLength(3)
      expect(tasks).toStrictEqual([
        {
          id: expect.any(String),
          name: body.name,
          isCompleted: false,
          assignee: null
        },
        {
          id: expect.any(String),
          name: createProjectTaskRequest1.name,
          isCompleted: false,
          assignee: null
        },
        {
          id: expect.any(String),
          name: createProjectTaskRequest2.name,
          isCompleted: false,
          assignee: null
        },
      ])
    })
  })

  test('should update project task', async ({ request }) => {
    const createProjectTaskRequest: CreateProjectTaskRequest = {
      projectStageId: STAGE_ID,
      name: "task 2"
    }
    const task = await createProjectTask(request, createProjectTaskRequest)

    const body: UpdateProjectTaskRequest = {
      id: task.id,
      name: "task 2 - Updated",
      isCompleted: true
    }
    const response = await request.put('/api/projects/tasks', {
      ...useToken(),
      data: body
    });

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toStrictEqual({
      message: expect.any(String)
    })

    const newTask = await getProjectTask(request, task.id)
    expect(newTask).toStrictEqual({
      name: body.name,
      description: "",
      isCompleted: body.isCompleted,
      assignee: body.assigneeId ? expect.objectContaining({ id: body.assigneeId }) : null,
      stage: expect.any(Object),
      sprint: expect.any(Object),
      project: expect.any(Object),
      workspace: expect.any(Object)
    })
  })

  test('should update overview project task', async ({ request }) => {
    const createProjectTaskRequest: CreateProjectTaskRequest = {
      projectStageId: STAGE_ID,
      name: "task 2"
    }
    const task = await createProjectTask(request, createProjectTaskRequest)

    const body: UpdateOverviewProjectTaskRequest = {
      id: task.id,
      name: "task 2 - Updated",
      description: "THis is a new description",
      isCompleted: true,
      assigneeId: null
    }
    const response = await request.put('/api/projects/tasks/overview', {
      ...useToken(),
      data: body
    });

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toStrictEqual({
      message: expect.any(String)
    })

    const newTask = await getProjectTask(request, task.id)
    expect(newTask).toStrictEqual({
      name: body.name,
      description: body.description,
      isCompleted: body.isCompleted,
      assignee: body.assigneeId ? expect.objectContaining({ id: body.assigneeId }) : null,
      stage: expect.any(Object),
      sprint: expect.any(Object),
      project: expect.any(Object),
      workspace: expect.any(Object)
    })
  })

  test('should delete project task', async ({ request }) => {
    const projectTask = await createProjectTask(request, {
      projectStageId: STAGE_ID,
      name: "Project task Name"
    })

    const response = await request.delete(`/api/projects/tasks/${projectTask.id}`, useToken());

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toStrictEqual({
      message: expect.any(String)
    })

    const tasks = await getProjectTasks(request, STAGE_ID)
    expect(tasks).toHaveLength(0)
  })
})