import { test } from "@playwright/test";
import { useToken } from "../utils/utils";
import { expect } from "../utils/matchers";
import { deleteUser, getCurrentUser, registerUser } from "../utils/users.utils";
import { getPersonalWorkspace } from "../utils/workspaces.utils";
import { createProject } from "../utils/projects.utils";
import { addStageToProjectSprint, getProjectSprints } from "../utils/project-sprint.utils";
import CreateProjectTaskRequest from "../types/requests/project-task/CreateProjectTaskRequest";
import { createProjectTask, getProjectTask, getProjectTasks } from "../utils/project-task.utils";
import UpdateProjectTaskRequest from "../types/requests/project-task/UpdateProjectTaskRequest";
import UpdateOverviewProjectTaskRequest from "../types/requests/project-task/UpdateOverviewProjectTaskRequest";
import MoveProjectTaskRequest from "../types/requests/project-task/MoveProjectTaskRequest";
import UpdateIsCompletedProjectTaskRequest from "../types/requests/project-task/UpdateIsCompletedProjectTaskRequest";
import UpdateCustomFieldFromProjectTaskRequest
  from "../types/requests/project-task/UpdateCustomFieldFromProjectTaskRequest";
import CreateCustomFieldSetupOnProjectRequest
  from "../types/requests/custom-field-setup/CreateCustomFieldSetupOnProjectRequest";
import { createCustomFieldSetupOnProject } from "../utils/custom-field-setup.utils";

test.describe('should allow operations on the project task entity', () => {
  let PROJECT_ID: string
  let STAGE_ID: string

  test.beforeEach('should register user and get project id', async ({ request }) => {
    process.env.API_TOKEN = (await registerUser(request)).token
    const workspace = await getPersonalWorkspace(request)
    const project = await createProject(request, {
      workspaceId: workspace.id,
      name: "Project name",
      isPrivate: false
    })
    PROJECT_ID = project.id
    const sprints = await getProjectSprints(request, project.id)
    const stage = await addStageToProjectSprint(request, {
      id: sprints[0].id,
      name: 'stage 2',
      onTop: false
    })
    STAGE_ID = stage.id
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
      workspace: expect.any(Object),
      customFields: []
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
            description: "",
            isCompleted: false,
            assignee: null,
            customFields: []
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
            description: "",
            isCompleted: false,
            assignee: null,
            customFields: []
          },
          {
            id: expect.any(String),
            name: projectTask2.name,
            description: "",
            isCompleted: false,
            assignee: null,
            customFields: []
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
            description: "",
            isCompleted: false,
            assignee: null,
            customFields: []
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
            description: "",
            isCompleted: false,
            assignee: null,
            customFields: []
          },
          {
            id: expect.any(String),
            name: projectTask2.name,
            description: "",
            isCompleted: false,
            assignee: null,
            customFields: []
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
          description: "",
          isCompleted: false,
          assignee: null,
          customFields: []
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
          description: "",
          isCompleted: false,
          assignee: null,
          customFields: []
        },
        {
          id: expect.any(String),
          name: createProjectTaskRequest1.name,
          description: "",
          isCompleted: false,
          assignee: null,
          customFields: []
        },
        {
          id: expect.any(String),
          name: createProjectTaskRequest2.name,
          description: "",
          isCompleted: false,
          assignee: null,
          customFields: []
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
      name: "task 2 - Updated"
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
      isCompleted: false,
      assignee: body.assigneeId ? expect.objectContaining({ id: body.assigneeId }) : null,
      stage: expect.any(Object),
      sprint: expect.any(Object),
      project: expect.any(Object),
      workspace: expect.any(Object),
      customFields: []
    })
  })

  test.describe('should update custom field from project task', () => {
    test('should update date custom field', async ({ request }) => {
      const createCustomFieldSetupOnProjectRequest: CreateCustomFieldSetupOnProjectRequest = {
        projectId: PROJECT_ID,
        name: "the date",
        description: "represents some date",
        type: 'Date',
        dateCustomFieldSetup: {
          defaultDate: undefined
        }
      }
      const customFieldSetup = await createCustomFieldSetupOnProject(request, createCustomFieldSetupOnProjectRequest)

      const createProjectTaskRequest: CreateProjectTaskRequest = {
        projectStageId: STAGE_ID,
        name: "task 2"
      }
      const task = await createProjectTask(request, createProjectTaskRequest)

      const body: UpdateCustomFieldFromProjectTaskRequest = {
        id: task.id,
        customFieldId: task.customFields[0].id,
        type: 'Date',
        dateCustomField: {
          date: "2022-10-10"
        }
      }
      const response = await request.put('/api/projects/tasks/custom-fields', {
        ...useToken(),
        data: body
      });

      expect(response.ok()).toBeTruthy()

      expect(await response.json()).toStrictEqual({
        message: expect.any(String)
      })

      const newTask = await getProjectTask(request, task.id)
      expect(newTask).toStrictEqual({
        name: createProjectTaskRequest.name,
        description: "",
        isCompleted: false,
        assignee: null,
        stage: expect.any(Object),
        sprint: expect.any(Object),
        project: expect.any(Object),
        workspace: expect.any(Object),
        customFields: [
          {
            $type: expect.any(String),
            id: body.customFieldId,
            name: customFieldSetup.name,
            description: customFieldSetup.description,
            type: body.type,
            date: body.dateCustomField?.date
          }
        ]
      })
    })

    test('should update date time custom field', async ({ request }) => {
      const createCustomFieldSetupOnProjectRequest: CreateCustomFieldSetupOnProjectRequest = {
        projectId: PROJECT_ID,
        name: "the date and time",
        description: "represents some date and time",
        type: 'DateTime',
        dateTimeCustomFieldSetup: {
          defaultDateTime: undefined
        }
      }
      const customFieldSetup = await createCustomFieldSetupOnProject(request, createCustomFieldSetupOnProjectRequest)

      const createProjectTaskRequest: CreateProjectTaskRequest = {
        projectStageId: STAGE_ID,
        name: "task 2"
      }
      const task = await createProjectTask(request, createProjectTaskRequest)

      const body: UpdateCustomFieldFromProjectTaskRequest = {
        id: task.id,
        customFieldId: task.customFields[0].id,
        type: 'DateTime',
        dateTimeCustomField: {
          dateTime: "2022-10-10T10:30:00"
        }
      }
      const response = await request.put('/api/projects/tasks/custom-fields', {
        ...useToken(),
        data: body
      });

      expect(response.ok()).toBeTruthy()

      expect(await response.json()).toStrictEqual({
        message: expect.any(String)
      })

      const newTask = await getProjectTask(request, task.id)
      expect(newTask).toStrictEqual({
        name: createProjectTaskRequest.name,
        description: "",
        isCompleted: false,
        assignee: null,
        stage: expect.any(Object),
        sprint: expect.any(Object),
        project: expect.any(Object),
        workspace: expect.any(Object),
        customFields: [
          {
            $type: expect.any(String),
            id: body.customFieldId,
            name: customFieldSetup.name,
            description: customFieldSetup.description,
            type: body.type,
            dateTime: body.dateTimeCustomField?.dateTime
          }
        ]
      })
    })

    test('should update duration custom field', async ({ request }) => {
      const createCustomFieldSetupOnProjectRequest: CreateCustomFieldSetupOnProjectRequest = {
        projectId: PROJECT_ID,
        name: "the duration",
        description: "represents some duration",
        type: 'Duration',
        durationCustomFieldSetup: {
          defaultDuration: undefined
        }
      }
      const customFieldSetup = await createCustomFieldSetupOnProject(request, createCustomFieldSetupOnProjectRequest)

      const createProjectTaskRequest: CreateProjectTaskRequest = {
        projectStageId: STAGE_ID,
        name: "task 2"
      }
      const task = await createProjectTask(request, createProjectTaskRequest)

      const body: UpdateCustomFieldFromProjectTaskRequest = {
        id: task.id,
        customFieldId: task.customFields[0].id,
        type: 'Duration',
        durationCustomField: {
          duration: "10:30:00"
        }
      }
      const response = await request.put('/api/projects/tasks/custom-fields', {
        ...useToken(),
        data: body
      });

      expect(response.ok()).toBeTruthy()

      expect(await response.json()).toStrictEqual({
        message: expect.any(String)
      })

      const newTask = await getProjectTask(request, task.id)
      expect(newTask).toStrictEqual({
        name: createProjectTaskRequest.name,
        description: "",
        isCompleted: false,
        assignee: null,
        stage: expect.any(Object),
        sprint: expect.any(Object),
        project: expect.any(Object),
        workspace: expect.any(Object),
        customFields: [
          {
            $type: expect.any(String),
            id: body.customFieldId,
            name: customFieldSetup.name,
            description: customFieldSetup.description,
            type: body.type,
            duration: body.durationCustomField?.duration
          }
        ]
      })
    })

    test('should update multi select custom field', async ({ request }) => {
      const createCustomFieldSetupOnProjectRequest: CreateCustomFieldSetupOnProjectRequest = {
        projectId: PROJECT_ID,
        name: "Priorities",
        description: "represents priorities",
        type: 'MultiSelect',
        multiSelectCustomFieldSetup: {
          options: [
            {
              value: "High",
              color: "#FF1122"
            },
            {
              value: "Low",
              color: "#1122FF"
            }
          ]
        }
      }
      const customFieldSetup = await createCustomFieldSetupOnProject(request, createCustomFieldSetupOnProjectRequest)

      const createProjectTaskRequest: CreateProjectTaskRequest = {
        projectStageId: STAGE_ID,
        name: "task 2"
      }
      const taskId = (await createProjectTask(request, createProjectTaskRequest)).id
      const task = await getProjectTask(request, taskId)

      // add new option
      const body1: UpdateCustomFieldFromProjectTaskRequest = {
        id: taskId,
        customFieldId: task.customFields[0].id,
        type: 'MultiSelect',
        multiSelectCustomField: {
          optionId: task.customFields[0].availableOptions[0].id,
          remove: false
        }
      }
      const response1 = await request.put('/api/projects/tasks/custom-fields', {
        ...useToken(),
        data: body1
      });

      expect(response1.ok()).toBeTruthy()

      expect(await response1.json()).toStrictEqual({
        message: expect.any(String)
      })

      const newTask1 = await getProjectTask(request, taskId)
      expect(newTask1).toStrictEqual({
        name: createProjectTaskRequest.name,
        description: "",
        isCompleted: false,
        assignee: null,
        stage: expect.any(Object),
        sprint: expect.any(Object),
        project: expect.any(Object),
        workspace: expect.any(Object),
        customFields: [
          {
            $type: expect.any(String),
            id: body1.customFieldId,
            name: customFieldSetup.name,
            description: customFieldSetup.description,
            type: body1.type,
            options: [
              {
                id: body1.multiSelectCustomField!.optionId,
                value: expect.any(String),
                color: expect.any(String),
              }
            ],
            availableOptions: task.customFields[0].availableOptions
          }
        ]
      })

      // remove option
      const body2: UpdateCustomFieldFromProjectTaskRequest = {
        id: taskId,
        customFieldId: task.customFields[0].id,
        type: 'MultiSelect',
        multiSelectCustomField: {
          optionId: task.customFields[0].availableOptions[0].id,
          remove: true
        }
      }
      const response2 = await request.put('/api/projects/tasks/custom-fields', {
        ...useToken(),
        data: body2
      });

      expect(response2.ok()).toBeTruthy()

      expect(await response2.json()).toStrictEqual({
        message: expect.any(String)
      })

      const newTask2 = await getProjectTask(request, taskId)
      expect(newTask2).toStrictEqual({
        name: createProjectTaskRequest.name,
        description: "",
        isCompleted: false,
        assignee: null,
        stage: expect.any(Object),
        sprint: expect.any(Object),
        project: expect.any(Object),
        workspace: expect.any(Object),
        customFields: [
          {
            $type: expect.any(String),
            id: body2.customFieldId,
            name: customFieldSetup.name,
            description: customFieldSetup.description,
            type: body2.type,
            options: [],
            availableOptions: task.customFields[0].availableOptions
          }
        ]
      })
    })

    test('should update number custom field', async ({ request }) => {
      const createCustomFieldSetupOnProjectRequest: CreateCustomFieldSetupOnProjectRequest = {
        projectId: PROJECT_ID,
        name: "Budget",
        description: "represents budget",
        type: 'Number',
        numberCustomFieldSetup: {
          settings: {
            decimals: 2,
            rounding: true
          }
        }
      }
      const customFieldSetup = await createCustomFieldSetupOnProject(request, createCustomFieldSetupOnProjectRequest)

      const createProjectTaskRequest: CreateProjectTaskRequest = {
        projectStageId: STAGE_ID,
        name: "task 2"
      }
      const task = await createProjectTask(request, createProjectTaskRequest)

      const body: UpdateCustomFieldFromProjectTaskRequest = {
        id: task.id,
        customFieldId: task.customFields[0].id,
        type: 'Number',
        numberCustomField: {
          number: 12
        }
      }
      const response = await request.put('/api/projects/tasks/custom-fields', {
        ...useToken(),
        data: body
      });

      expect(response.ok()).toBeTruthy()

      expect(await response.json()).toStrictEqual({
        message: expect.any(String)
      })

      const newTask = await getProjectTask(request, task.id)
      expect(newTask).toStrictEqual({
        name: createProjectTaskRequest.name,
        description: "",
        isCompleted: false,
        assignee: null,
        stage: expect.any(Object),
        sprint: expect.any(Object),
        project: expect.any(Object),
        workspace: expect.any(Object),
        customFields: [
          {
            $type: expect.any(String),
            id: body.customFieldId,
            name: customFieldSetup.name,
            description: customFieldSetup.description,
            type: body.type,
            number: body.numberCustomField?.number
          }
        ]
      })
    })

    test('should update people custom field', async ({ request }) => {
      const createCustomFieldSetupOnProjectRequest: CreateCustomFieldSetupOnProjectRequest = {
        projectId: PROJECT_ID,
        name: "the people",
        description: "represents some people",
        type: 'People'
      }
      const customFieldSetup = await createCustomFieldSetupOnProject(request, createCustomFieldSetupOnProjectRequest)

      const createProjectTaskRequest: CreateProjectTaskRequest = {
        projectStageId: STAGE_ID,
        name: "task 2"
      }
      const task = await createProjectTask(request, createProjectTaskRequest)

      const newUserToken = (await registerUser(request, "some_new_user@gmail.com")).token
      const person = await getCurrentUser(request, newUserToken)

      const body: UpdateCustomFieldFromProjectTaskRequest = {
        id: task.id,
        customFieldId: task.customFields[0].id,
        type: 'People',
        peopleCustomField: {
          personId: person.id
        }
      }
      const response = await request.put('/api/projects/tasks/custom-fields', {
        ...useToken(),
        data: body
      });

      expect(response.ok()).toBeTruthy()

      expect(await response.json()).toStrictEqual({
        message: expect.any(String)
      })

      const newTask = await getProjectTask(request, task.id)
      expect(newTask).toStrictEqual({
        name: createProjectTaskRequest.name,
        description: "",
        isCompleted: false,
        assignee: null,
        stage: expect.any(Object),
        sprint: expect.any(Object),
        project: expect.any(Object),
        workspace: expect.any(Object),
        customFields: [
          {
            $type: expect.any(String),
            id: body.customFieldId,
            name: customFieldSetup.name,
            description: customFieldSetup.description,
            type: body.type,
            person: {
              id: body.peopleCustomField?.personId,
              name: person.firstName + " " + person.lastName,
              email: person.email
            }
          }
        ]
      })
    })

    test('should update single select custom field', async ({ request }) => {
      const createCustomFieldSetupOnProjectRequest: CreateCustomFieldSetupOnProjectRequest = {
        projectId: PROJECT_ID,
        name: "Priority",
        description: "Represents a custom field",
        type: 'SingleSelect',
        singleSelectCustomFieldSetup: {
          options: [
            {
              id: "1",
              value: "High",
              color: "#FF1122"
            },
            {
              id: "2",
              value: "Low",
              color: "#1122FF"
            }
          ],
          defaultOptionId: "1"
        }
      }
      const customFieldSetup = await createCustomFieldSetupOnProject(request, createCustomFieldSetupOnProjectRequest)

      const createProjectTaskRequest: CreateProjectTaskRequest = {
        projectStageId: STAGE_ID,
        name: "task 2"
      }
      const taskId = (await createProjectTask(request, createProjectTaskRequest)).id
      const task = await getProjectTask(request, taskId)
      
      const body: UpdateCustomFieldFromProjectTaskRequest = {
        id: taskId,
        customFieldId: task.customFields[0].id,
        type: 'SingleSelect',
        singleSelectCustomField: {
          optionId: task.customFields[0].availableOptions[1].id,
        }
      }
      const response = await request.put('/api/projects/tasks/custom-fields', {
        ...useToken(),
        data: body
      });

      expect(response.ok()).toBeTruthy()

      expect(await response.json()).toStrictEqual({
        message: expect.any(String)
      })

      const newTask = await getProjectTask(request, taskId)
      expect(newTask).toStrictEqual({
        name: createProjectTaskRequest.name,
        description: "",
        isCompleted: false,
        assignee: null,
        stage: expect.any(Object),
        sprint: expect.any(Object),
        project: expect.any(Object),
        workspace: expect.any(Object),
        customFields: [
          {
            $type: expect.any(String),
            id: body.customFieldId,
            name: customFieldSetup.name,
            description: customFieldSetup.description,
            type: body.type,
            option: {
              id: task.customFields[0].availableOptions[1].id,
              color: task.customFields[0].availableOptions[1].color,
              value: task.customFields[0].availableOptions[1].value
            },
            availableOptions: [
              {
                id: task.customFields[0].availableOptions[0].id,
                color: task.customFields[0].availableOptions[0].color,
                value: task.customFields[0].availableOptions[0].value,
              },
              {
                id: task.customFields[0].availableOptions[1].id,
                color: task.customFields[0].availableOptions[1].color,
                value: task.customFields[0].availableOptions[1].value,
              }
            ]
          }
        ]
      })
    })

    test('should update text custom field', async ({ request }) => {
      const createCustomFieldSetupOnProjectRequest: CreateCustomFieldSetupOnProjectRequest = {
        projectId: PROJECT_ID,
        name: "Notes",
        description: "Additional text",
        type: 'Text',
        textCustomFieldSetup: { defaultText: undefined }
      }
      const customFieldSetup = await createCustomFieldSetupOnProject(request, createCustomFieldSetupOnProjectRequest)

      const createProjectTaskRequest: CreateProjectTaskRequest = {
        projectStageId: STAGE_ID,
        name: "task 2"
      }
      const task = await createProjectTask(request, createProjectTaskRequest)

      const body: UpdateCustomFieldFromProjectTaskRequest = {
        id: task.id,
        customFieldId: task.customFields[0].id,
        type: 'Text',
        textCustomField: {
          text: "Some text"
        }
      }
      const response = await request.put('/api/projects/tasks/custom-fields', {
        ...useToken(),
        data: body
      });

      expect(response.ok()).toBeTruthy()

      expect(await response.json()).toStrictEqual({
        message: expect.any(String)
      })

      const newTask = await getProjectTask(request, task.id)
      expect(newTask).toStrictEqual({
        name: createProjectTaskRequest.name,
        description: "",
        isCompleted: false,
        assignee: null,
        stage: expect.any(Object),
        sprint: expect.any(Object),
        project: expect.any(Object),
        workspace: expect.any(Object),
        customFields: [
          {
            $type: expect.any(String),
            id: body.customFieldId,
            name: customFieldSetup.name,
            description: customFieldSetup.description,
            type: body.type,
            text: body.textCustomField?.text
          }
        ]
      })
    })

    test('should update time custom field', async ({ request }) => {
      const createCustomFieldSetupOnProjectRequest: CreateCustomFieldSetupOnProjectRequest = {
        projectId: PROJECT_ID,
        name: "the time",
        description: "represents some time",
        type: 'Time',
        timeCustomFieldSetup: {
          defaultTime: undefined
        }
      }
      const customFieldSetup = await createCustomFieldSetupOnProject(request, createCustomFieldSetupOnProjectRequest)

      const createProjectTaskRequest: CreateProjectTaskRequest = {
        projectStageId: STAGE_ID,
        name: "task 2"
      }
      const task = await createProjectTask(request, createProjectTaskRequest)

      const body: UpdateCustomFieldFromProjectTaskRequest = {
        id: task.id,
        customFieldId: task.customFields[0].id,
        type: 'Time',
        timeCustomField: {
          time: "10:30:00"
        }
      }
      const response = await request.put('/api/projects/tasks/custom-fields', {
        ...useToken(),
        data: body
      });

      expect(response.ok()).toBeTruthy()

      expect(await response.json()).toStrictEqual({
        message: expect.any(String)
      })

      const newTask = await getProjectTask(request, task.id)
      expect(newTask).toStrictEqual({
        name: createProjectTaskRequest.name,
        description: "",
        isCompleted: false,
        assignee: null,
        stage: expect.any(Object),
        sprint: expect.any(Object),
        project: expect.any(Object),
        workspace: expect.any(Object),
        customFields: [
          {
            $type: expect.any(String),
            id: body.customFieldId,
            name: customFieldSetup.name,
            description: customFieldSetup.description,
            type: body.type,
            time: body.timeCustomField?.time
          }
        ]
      })
    })
  })

  test('should update is completed project task', async ({ request }) => {
    const createProjectTaskRequest: CreateProjectTaskRequest = {
      projectStageId: STAGE_ID,
      name: "task 2"
    }
    const task = await createProjectTask(request, createProjectTaskRequest)

    const body: UpdateIsCompletedProjectTaskRequest = {
      id: task.id,
      isCompleted: true
    }
    const response = await request.put('/api/projects/tasks/is-completed', {
      ...useToken(),
      data: body
    });

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toStrictEqual({
      message: expect.any(String)
    })

    const newTask = await getProjectTask(request, task.id)
    expect(newTask).toStrictEqual({
      name: createProjectTaskRequest.name,
      description: "",
      isCompleted: body.isCompleted,
      assignee: null,
      stage: expect.any(Object),
      sprint: expect.any(Object),
      project: expect.any(Object),
      workspace: expect.any(Object),
      customFields: []
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
      isCompleted: false,
      assignee: body.assigneeId ? expect.objectContaining({ id: body.assigneeId }) : null,
      stage: expect.any(Object),
      sprint: expect.any(Object),
      project: expect.any(Object),
      workspace: expect.any(Object),
      customFields: []
    })
  })

  test('should move project task', async ({ request }) => {
    const createProjectTaskRequest1: CreateProjectTaskRequest = {
      projectStageId: STAGE_ID,
      name: "task 1"
    }
    const task1 = await createProjectTask(request, createProjectTaskRequest1)

    const createProjectTaskRequest2: CreateProjectTaskRequest = {
      projectStageId: STAGE_ID,
      name: "task 2"
    }
    const task2 = await createProjectTask(request, createProjectTaskRequest2)

    const createProjectTaskRequest3: CreateProjectTaskRequest = {
      projectStageId: STAGE_ID,
      name: "task 3"
    }
    const task3 = await createProjectTask(request, createProjectTaskRequest3)

    const body: MoveProjectTaskRequest = {
      id: task2.id,
      overId: task3.id
    }
    const response = await request.post('/api/projects/tasks/move', {
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
        name: task1.name,
        description: "",
        isCompleted: false,
        assignee: null,
        customFields: []
      },
      {
        id: expect.any(String),
        name: task3.name,
        description: "",
        isCompleted: false,
        assignee: null,
        customFields: []
      },
      {
        id: expect.any(String),
        name: task2.name,
        description: "",
        isCompleted: false,
        assignee: null,
        customFields: []
      }
    ])
  })

  test('should delete project task', async ({ request }) => {
    const projectTask = await createProjectTask(request, {
      projectStageId: STAGE_ID,
      name: "Project task Name"
    })

    const response = await request.delete(
      `/api/projects/tasks/${projectTask.id}/from/${STAGE_ID}`,
      useToken()
    );

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toStrictEqual({
      message: expect.any(String)
    })

    const tasks = await getProjectTasks(request, STAGE_ID)
    expect(tasks).toHaveLength(0)
  })
})