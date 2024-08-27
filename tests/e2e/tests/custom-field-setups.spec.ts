import { test } from "@playwright/test";
import { deleteUser, registerUser } from "../utils/users.utils";
import { getPersonalWorkspace } from "../utils/workspaces.utils";
import { createProject } from "../utils/projects.utils";
import { useToken } from "../utils/utils";
import { expect } from "../utils/matchers";
import CreateCustomFieldSetupOnProjectRequest
  from "../types/requests/custom-field-setup/CreateCustomFieldSetupOnProjectRequest";
import {
  addCustomFieldSetupToProject,
  createCustomFieldSetupOnProject,
  createCustomFieldSetupOnWorkspace, getCustomFieldSetup,
  getCustomFieldSetupsFromProject, getCustomFieldSetupsFromWorkspace
} from "../utils/custom-field-setup.utils";
import UpdateWorkspaceCustomFieldSetupRequest
  from "../types/requests/custom-field-setup/UpdateWorkspaceCustomFieldSetupRequest";
import AddCustomFieldSetupToProjectRequest
  from "../types/requests/custom-field-setup/AddCustomFieldSetupToProjectRequest";
import CreateCustomFieldSetupOnWorkspaceRequest
  from "../types/requests/custom-field-setup/CreateCustomFieldSetupOnWorkspaceRequest";
import UpdateCustomFieldSetupRequest from "../types/requests/custom-field-setup/UpdateCustomFieldSetupRequest";

test.describe('should allow operations on the custom field setup entity', () => {
  let WORKSPACE_ID: string
  let PROJECT_ID: string

  test.beforeEach('should register user and get project id', async ({ request }) => {
    process.env.API_TOKEN = (await registerUser(request)).token
    const workspace = await getPersonalWorkspace(request)
    WORKSPACE_ID = workspace.id
    PROJECT_ID = (await createProject(request, {
      workspaceId: workspace.id,
      name: "Project name",
      isPrivate: false
    })).id
  })

  test.afterEach('should delete user', async ({ request }) => {
    await deleteUser(request)
  })

  test.describe('should allow retrieval of custom field', () => {
    test('should get number custom field', async ({ request }) => {
      const createCustomFieldSetupOnProjectRequest: CreateCustomFieldSetupOnProjectRequest = {
        projectId: PROJECT_ID,
        name: "Budget",
        description: "Represents a custom field",
        type: 'Number',
        numberCustomFieldSetup: {
          settings: {
            currencyCode: "USD",
            decimals: 2,
            rounding: true
          },
          defaultNumber: 0
        }
      }
      const numberCustomField = await createCustomFieldSetupOnProject(request, createCustomFieldSetupOnProjectRequest)
      const response = await request.get(`/api/customFieldSetups/${numberCustomField.id}`, useToken())

      expect(response.ok()).toBeTruthy()

      expect(await response.json()).toStrictEqual({
        customFieldSetup: {
          $type: expect.any(String),
          id: numberCustomField.id,
          name: createCustomFieldSetupOnProjectRequest.name,
          description: createCustomFieldSetupOnProjectRequest.description,
          type: "Number",
          settings: {
            currencyCode: createCustomFieldSetupOnProjectRequest.numberCustomFieldSetup!.settings.currencyCode,
            decimals: createCustomFieldSetupOnProjectRequest.numberCustomFieldSetup!.settings.decimals,
            rounding: createCustomFieldSetupOnProjectRequest.numberCustomFieldSetup!.settings.rounding,
          },
          defaultNumber: createCustomFieldSetupOnProjectRequest.numberCustomFieldSetup?.defaultNumber
        }
      })
    })

    test('should get single select custom field', async ({ request }) => {
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
      const singleSelectCustomField = await createCustomFieldSetupOnProject(request, createCustomFieldSetupOnProjectRequest)
      const response = await request.get(`/api/customFieldSetups/${singleSelectCustomField.id}`, useToken())

      expect(response.ok()).toBeTruthy()

      expect(await response.json()).toStrictEqual({
        customFieldSetup: {
          $type: expect.any(String),
          id: singleSelectCustomField.id,
          name: createCustomFieldSetupOnProjectRequest.name,
          description: createCustomFieldSetupOnProjectRequest.description,
          type: "SingleSelect",
          options: [
            {
              id: expect.any(String),
              value: createCustomFieldSetupOnProjectRequest.singleSelectCustomFieldSetup!.options[0].value,
              color: createCustomFieldSetupOnProjectRequest.singleSelectCustomFieldSetup!.options[0].color,
            },
            {
              id: expect.any(String),
              value: createCustomFieldSetupOnProjectRequest.singleSelectCustomFieldSetup!.options[1].value,
              color: createCustomFieldSetupOnProjectRequest.singleSelectCustomFieldSetup!.options[1].color,
            }
          ],
          defaultOption: {
            id: expect.any(String),
            value: createCustomFieldSetupOnProjectRequest.singleSelectCustomFieldSetup!.options[0].value,
            color: createCustomFieldSetupOnProjectRequest.singleSelectCustomFieldSetup!.options[0].color,
          }
        }
      })
    })

    test('should get text custom field', async ({ request }) => {
      const createCustomFieldSetupOnProjectRequest: CreateCustomFieldSetupOnProjectRequest = {
        projectId: PROJECT_ID,
        name: "Priority",
        description: "Represents a custom field",
        type: 'Text',
        textCustomFieldSetup: {
          defaultText: "some default text"
        }
      }
      const textCustomField = await createCustomFieldSetupOnProject(request, createCustomFieldSetupOnProjectRequest)
      const response = await request.get(`/api/customFieldSetups/${textCustomField.id}`, useToken())

      expect(response.ok()).toBeTruthy()

      expect(await response.json()).toStrictEqual({
        customFieldSetup: {
          $type: expect.any(String),
          id: textCustomField.id,
          name: createCustomFieldSetupOnProjectRequest.name,
          description: createCustomFieldSetupOnProjectRequest.description,
          type: "Text",
          defaultText: createCustomFieldSetupOnProjectRequest.textCustomFieldSetup?.defaultText,
        }
      })
    })
  })

  test('should get custom field setups from workspace', async ({ request }) => {
    const numberCustomField = await createCustomFieldSetupOnWorkspace(request, {
      workspaceId: WORKSPACE_ID,
      name: "Budget",
      description: "Represents a custom field",
      type: 'Number',
      numberCustomFieldSetup: {
        settings: {
          currencyCode: "USD",
          decimals: 2,
          rounding: true
        }
      }
    })
    const singleSelectCustomField = await createCustomFieldSetupOnWorkspace(request, {
      workspaceId: WORKSPACE_ID,
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
        ]
      }
    })
    const textCustomField = await createCustomFieldSetupOnWorkspace(request, {
      workspaceId: WORKSPACE_ID,
      name: "New Text Custom Field",
      description: "Represents a custom field",
      type: 'Text',
      textCustomFieldSetup: {
        defaultText: undefined
      }
    })

    const response = await request.get(`/api/customFieldSetups/workspace/${WORKSPACE_ID}`, useToken())

    expect(response.ok()).toBeTruthy()

    const json = await response.json()
    expect(json.customFieldSetups).toHaveLength(3)
    expect(json.customFieldSetups).toStrictEqual([
      {
        id: numberCustomField.id,
        name: numberCustomField.name,
        description: numberCustomField.description,
        type: 'Number'
      },
      {
        id: singleSelectCustomField.id,
        name: singleSelectCustomField.name,
        description: singleSelectCustomField.description,
        type: 'SingleSelect'
      },
      {
        id: textCustomField.id,
        name: textCustomField.name,
        description: textCustomField.description,
        type: 'Text'
      }
    ])
  })

  test('should get custom field setups from project', async ({ request }) => {
    const numberCustomField = await createCustomFieldSetupOnProject(request, {
      projectId: PROJECT_ID,
      name: "Budget",
      description: "Represents a custom field",
      type: 'Number',
      numberCustomFieldSetup: {
        settings: {
          currencyCode: "USD",
          decimals: 2,
          rounding: true
        }
      }
    })
    const singleSelectCustomField = await createCustomFieldSetupOnProject(request, {
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
        ]
      }
    })
    const textCustomField = await createCustomFieldSetupOnProject(request, {
      projectId: PROJECT_ID,
      name: "New Text Custom Field",
      description: "Represents a custom field",
      type: 'Text',
      textCustomFieldSetup: {
        defaultText: undefined
      }
    })

    const response = await request.get(`/api/customFieldSetups/project/${PROJECT_ID}`, useToken())

    expect(response.ok()).toBeTruthy()

    const json = await response.json()
    expect(json.customFieldSetups).toHaveLength(3)
    expect(json.customFieldSetups).toStrictEqual([
      {
        id: numberCustomField.id,
        name: numberCustomField.name,
        description: numberCustomField.description,
        type: 'Number'
      },
      {
        id: singleSelectCustomField.id,
        name: singleSelectCustomField.name,
        description: singleSelectCustomField.description,
        type: 'SingleSelect'
      },
      {
        id: textCustomField.id,
        name: textCustomField.name,
        description: textCustomField.description,
        type: 'Text'
      }
    ])
  })

  test.describe(`should allow creation of custom field setup on project`, () => {
    test('should create number custom field setup on project', async ({ request }) => {
      const body: CreateCustomFieldSetupOnProjectRequest = {
        projectId: PROJECT_ID,
        name: "Budget",
        description: "Represents a custom field",
        type: 'Number',
        numberCustomFieldSetup: {
          settings: {
            currencyCode: "USD",
            decimals: 2,
            rounding: true
          },
          defaultNumber: 1
        }
      }
      const response = await request.post('/api/customFieldSetups/project', {
        ...useToken(),
        data: body
      });

      expect(response.ok()).toBeTruthy()

      expect(await response.json()).toStrictEqual({
        message: expect.any(String)
      })

      const customFieldSetups = await getCustomFieldSetupsFromProject(request, PROJECT_ID)
      expect(customFieldSetups).toHaveLength(1)
      expect(customFieldSetups).toStrictEqual([
        {
          id: expect.any(String),
          name: body.name,
          description: body.description,
          type: 'Number'
        }
      ])
    })

    test('should create single select custom field setup on project', async ({ request }) => {
      const body: CreateCustomFieldSetupOnProjectRequest = {
        projectId: PROJECT_ID,
        name: "Priority",
        description: "Represents a custom field",
        type: 'SingleSelect',
        singleSelectCustomFieldSetup: {
          options: [
            {
              id: '1',
              value: "High",
              color: "#FF1122"
            },
            {
              id: '2',
              value: "Low",
              color: "#1122FF"
            }
          ],
          defaultOptionId: '2'
        }
      }
      const response = await request.post('/api/customFieldSetups/project', {
        ...useToken(),
        data: body
      });

      expect(response.ok()).toBeTruthy()

      expect(await response.json()).toStrictEqual({
        message: expect.any(String)
      })

      const customFieldSetups = await getCustomFieldSetupsFromProject(request, PROJECT_ID)
      expect(customFieldSetups).toHaveLength(1)
      expect(customFieldSetups).toStrictEqual([
        {
          id: expect.any(String),
          name: body.name,
          description: body.description,
          type: 'SingleSelect'
        }
      ])
    })

    test('should create text custom field setup on project', async ({ request }) => {
      const body: CreateCustomFieldSetupOnProjectRequest = {
        projectId: PROJECT_ID,
        name: "New Text Custom Field",
        description: "Represents a custom field",
        type: 'Text',
        textCustomFieldSetup: {
          defaultText: "default"
        }
      }
      const response = await request.post('/api/customFieldSetups/project', {
        ...useToken(),
        data: body
      });

      expect(response.ok()).toBeTruthy()

      expect(await response.json()).toStrictEqual({
        message: expect.any(String)
      })

      const customFieldSetups = await getCustomFieldSetupsFromProject(request, PROJECT_ID)
      expect(customFieldSetups).toHaveLength(1)
      expect(customFieldSetups).toStrictEqual([
        {
          id: expect.any(String),
          name: body.name,
          description: body.description,
          type: 'Text'
        }
      ])
    })
  })

  test('should add custom field setup to project', async ({ request }) => {
    const createCustomFieldSetupOnWorkspaceRequest: CreateCustomFieldSetupOnWorkspaceRequest = {
      workspaceId: WORKSPACE_ID,
      name: "New Text Custom Field",
      description: "Represents a custom field",
      type: 'Text',
      textCustomFieldSetup: {
        defaultText: undefined
      }
    }
    const textCustomField = await createCustomFieldSetupOnWorkspace(request, createCustomFieldSetupOnWorkspaceRequest)

    const body: AddCustomFieldSetupToProjectRequest = {
      id: textCustomField.id,
      projectId: PROJECT_ID,
    }
    const response = await request.post(`/api/customFieldSetups/add-project`, {
      ...useToken(),
      data: body
    })

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toStrictEqual({
      message: expect.any(String)
    })

    const customFieldSetups = await getCustomFieldSetupsFromProject(request, PROJECT_ID)
    expect(customFieldSetups).toHaveLength(1)
    expect(customFieldSetups).toStrictEqual([
      {
        id: textCustomField.id,
        name: textCustomField.name,
        description: textCustomField.description,
        type: 'Text'
      }
    ])
  })

  test.describe('should allow update of custom field', () => {
    test('should update number custom field', async ({ request }) => {
      const createCustomFieldSetupOnProjectRequest: CreateCustomFieldSetupOnProjectRequest = {
        projectId: PROJECT_ID,
        name: "Budget",
        description: "Represents a custom field",
        type: 'Number',
        numberCustomFieldSetup: {
          settings: {
            currencyCode: "USD",
            decimals: 2,
            rounding: true
          },
          defaultNumber: 0
        }
      }
      const numberCustomField = await createCustomFieldSetupOnProject(request, createCustomFieldSetupOnProjectRequest)

      const body: UpdateCustomFieldSetupRequest = {
        id: numberCustomField.id,
        name: "New Budget",
        description: "no description",
        type: "Number",
        numberCustomFieldSetup: {
          settings: {
            currencyCode: "EUR",
            decimals: 3,
            rounding: true
          },
          defaultNumber: undefined
        }
      }
      const response = await request.put(`/api/customFieldSetups`, {
        ...useToken(),
        data: body
      })

      expect(response.ok()).toBeTruthy()

      expect(await response.json()).toStrictEqual({
        message: expect.any(String)
      })

      const customFieldSetup = await getCustomFieldSetup(request, numberCustomField.id)
      expect(customFieldSetup).toStrictEqual({
        customFieldSetup: {
          $type: expect.any(String),
          id: numberCustomField.id,
          name: body.name,
          description: body.description,
          type: "Number",
          settings: {
            currencyCode: body.numberCustomFieldSetup!.settings.currencyCode,
            decimals: body.numberCustomFieldSetup!.settings.decimals,
            rounding: body.numberCustomFieldSetup!.settings.rounding,
          },
          defaultNumber: body.numberCustomFieldSetup?.defaultNumber
        }
      })
    })

    test.describe('should allow update of single select custom field' , () => {
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
        const singleSelectCustomField = await createCustomFieldSetupOnProject(request, createCustomFieldSetupOnProjectRequest)

        const body: UpdateCustomFieldSetupRequest = {
          id: singleSelectCustomField.id,
          name: "New setup",
          description: "no description",
          type: "SingleSelect",
          singleSelectCustomFieldSetup: {
            defaultOptionId: singleSelectCustomField.options[1].id,
          }
        }
        const response = await request.put(`/api/customFieldSetups`, {
          ...useToken(),
          data: body
        })

        expect(response.ok()).toBeTruthy()

        expect(await response.json()).toStrictEqual({
          message: expect.any(String)
        })

        const customFieldSetup = await getCustomFieldSetup(request, singleSelectCustomField.id)
        expect(customFieldSetup).toStrictEqual({
          customFieldSetup: {
            $type: expect.any(String),
            id: singleSelectCustomField.id,
            name: body.name,
            description: body.description,
            type: "SingleSelect",
            options: [
              singleSelectCustomField.options[0],
              singleSelectCustomField.options[1]
            ],
            defaultOption: {
              id: body.singleSelectCustomFieldSetup?.defaultOptionId,
              value: createCustomFieldSetupOnProjectRequest.singleSelectCustomFieldSetup!.options[1].value,
              color: createCustomFieldSetupOnProjectRequest.singleSelectCustomFieldSetup!.options[1].color,
            }
          }
        })
      })

      test('should update single select custom field with add operation', async ({ request }) => {
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
        const singleSelectCustomField = await createCustomFieldSetupOnProject(request, createCustomFieldSetupOnProjectRequest)

        const body: UpdateCustomFieldSetupRequest = {
          id: singleSelectCustomField.id,
          name: "New setup",
          description: "no description",
          type: "SingleSelect",
          singleSelectCustomFieldSetup: {
            operation: "Add",
            newOption: {
              color: "#214fc5",
              value: "Medium"
            }
          }
        }
        const response = await request.put(`/api/customFieldSetups`, {
          ...useToken(),
          data: body
        })

        expect(response.ok()).toBeTruthy()

        expect(await response.json()).toStrictEqual({
          message: expect.any(String)
        })

        const customFieldSetup = await getCustomFieldSetup(request, singleSelectCustomField.id)
        expect(customFieldSetup).toStrictEqual({
          customFieldSetup: {
            $type: expect.any(String),
            id: singleSelectCustomField.id,
            name: body.name,
            description: body.description,
            type: "SingleSelect",
            options: [
              singleSelectCustomField.options[0],
              singleSelectCustomField.options[1],
              {
                id: expect.any(String),
                value: body.singleSelectCustomFieldSetup?.newOption?.value,
                color: body.singleSelectCustomFieldSetup?.newOption?.color,
              }
            ],
            defaultOption: null
          }
        })
      })

      test('should update single select custom field with update operation', async ({ request }) => {
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
        const singleSelectCustomField = await createCustomFieldSetupOnProject(request, createCustomFieldSetupOnProjectRequest)

        const body: UpdateCustomFieldSetupRequest = {
          id: singleSelectCustomField.id,
          name: "New setup",
          description: "no description",
          type: "SingleSelect",
          singleSelectCustomFieldSetup: {
            operation: "Update",
            newOption: {
              color: "#214fc5",
              value: "Medium"
            },
            optionId: singleSelectCustomField.options[1].id
          }
        }
        const response = await request.put(`/api/customFieldSetups`, {
          ...useToken(),
          data: body
        })

        expect(response.ok()).toBeTruthy()

        expect(await response.json()).toStrictEqual({
          message: expect.any(String)
        })

        const customFieldSetup = await getCustomFieldSetup(request, singleSelectCustomField.id)
        expect(customFieldSetup).toStrictEqual({
          customFieldSetup: {
            $type: expect.any(String),
            id: singleSelectCustomField.id,
            name: body.name,
            description: body.description,
            type: "SingleSelect",
            options: [
              singleSelectCustomField.options[0],
              {
                id: body.singleSelectCustomFieldSetup?.optionId,
                value: body.singleSelectCustomFieldSetup?.newOption?.value,
                color: body.singleSelectCustomFieldSetup?.newOption?.color,
              },
            ],
            defaultOption: null
          }
        })
      })

      test('should update single select custom field with move operation', async ({ request }) => {
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
        const singleSelectCustomField = await createCustomFieldSetupOnProject(request, createCustomFieldSetupOnProjectRequest)

        const body: UpdateCustomFieldSetupRequest = {
          id: singleSelectCustomField.id,
          name: "New setup",
          description: "no description",
          type: "SingleSelect",
          singleSelectCustomFieldSetup: {
            operation: "Move",
            optionId: singleSelectCustomField.options[0].id,
            overOptionId: singleSelectCustomField.options[1].id,
          }
        }
        const response = await request.put(`/api/customFieldSetups`, {
          ...useToken(),
          data: body
        })

        expect(response.ok()).toBeTruthy()

        expect(await response.json()).toStrictEqual({
          message: expect.any(String)
        })

        const customFieldSetup = await getCustomFieldSetup(request, singleSelectCustomField.id)
        expect(customFieldSetup).toStrictEqual({
          customFieldSetup: {
            $type: expect.any(String),
            id: singleSelectCustomField.id,
            name: body.name,
            description: body.description,
            type: "SingleSelect",
            options: [
              singleSelectCustomField.options[1],
              singleSelectCustomField.options[0],
            ],
            defaultOption: null
          }
        })
      })

      test('should update single select custom field with remove operation', async ({ request }) => {
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
        const singleSelectCustomField = await createCustomFieldSetupOnProject(request, createCustomFieldSetupOnProjectRequest)

        const body: UpdateCustomFieldSetupRequest = {
          id: singleSelectCustomField.id,
          name: "New setup",
          description: "no description",
          type: "SingleSelect",
          singleSelectCustomFieldSetup: {
            operation: "Remove",
            optionId: singleSelectCustomField.options[1].id
          }
        }
        const response = await request.put(`/api/customFieldSetups`, {
          ...useToken(),
          data: body
        })

        expect(response.ok()).toBeTruthy()

        expect(await response.json()).toStrictEqual({
          message: expect.any(String)
        })

        const customFieldSetup = await getCustomFieldSetup(request, singleSelectCustomField.id)
        expect(customFieldSetup).toStrictEqual({
          customFieldSetup: {
            $type: expect.any(String),
            id: singleSelectCustomField.id,
            name: body.name,
            description: body.description,
            type: "SingleSelect",
            options: [
              singleSelectCustomField.options[0]
            ],
            defaultOption: null
          }
        })
      })
    })

    test('should update text custom field', async ({ request }) => {
      const createCustomFieldSetupOnProjectRequest: CreateCustomFieldSetupOnProjectRequest = {
        projectId: PROJECT_ID,
        name: "Priority",
        description: "Represents a custom field",
        type: 'Text',
        textCustomFieldSetup: {
          defaultText: "some default text"
        }
      }
      const textCustomField = await createCustomFieldSetupOnProject(request, createCustomFieldSetupOnProjectRequest)

      const body: UpdateCustomFieldSetupRequest = {
        id: textCustomField.id,
        name: "New setup",
        description: "no description",
        type: "Text",
        textCustomFieldSetup: {
          defaultText: undefined
        }
      }
      const response = await request.put(`/api/customFieldSetups`, {
        ...useToken(),
        data: body
      })

      expect(response.ok()).toBeTruthy()

      expect(await response.json()).toStrictEqual({
        message: expect.any(String)
      })

      const customFieldSetup = await getCustomFieldSetup(request, textCustomField.id)
      expect(customFieldSetup).toStrictEqual({
        customFieldSetup: {
          $type: expect.any(String),
          id: textCustomField.id,
          name: body.name,
          description: body.description,
          type: "Text",
          defaultText: body.textCustomFieldSetup?.defaultText,
        }
      })
    })
  })

  test('should update workspace on custom field setup', async ({ request }) => {
    const createCustomFieldSetupOnProjectRequest: CreateCustomFieldSetupOnProjectRequest = {
      projectId: PROJECT_ID,
      name: "New Text Custom Field",
      description: "Represents a custom field",
      type: 'Text',
      textCustomFieldSetup: {
        defaultText: undefined
      }
    }
    const textCustomField = await createCustomFieldSetupOnProject(request, createCustomFieldSetupOnProjectRequest)

    const body: UpdateWorkspaceCustomFieldSetupRequest = {
      id: textCustomField.id,
      workspaceId: WORKSPACE_ID,
    }
    const response = await request.put(`/api/customFieldSetups/workspace`, {
      ...useToken(),
      data: body
    })

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toStrictEqual({
      message: expect.any(String)
    })

    const customFieldSetups = await getCustomFieldSetupsFromWorkspace(request, WORKSPACE_ID)
    expect(customFieldSetups).toHaveLength(1)
    expect(customFieldSetups).toStrictEqual([
      {
        id: textCustomField.id,
        name: textCustomField.name,
        description: textCustomField.description,
        type: 'Text'
      }
    ])
  })

  test('should delete custom field setup', async ({ request }) => {
    const textCustomField = await createCustomFieldSetupOnProject(request, {
      projectId: PROJECT_ID,
      name: "New Text Custom Field",
      description: "Represents a custom field",
      type: 'Text',
      textCustomFieldSetup: {
        defaultText: undefined
      }
    })

    const response = await request.delete(`/api/customFieldSetups/${textCustomField.id}`, useToken())

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toStrictEqual({
      message: expect.any(String)
    })

    const customFieldSetups = await getCustomFieldSetupsFromProject(request, PROJECT_ID)
    expect(customFieldSetups).toHaveLength(0)
    expect(customFieldSetups).toStrictEqual(expect.not.arrayContaining([
      {
        id: textCustomField.id,
        name: textCustomField.name,
        description: textCustomField.description,
        type: 'Text'
      }
    ]))
  })

  test('should remove custom field setup from project', async ({ request }) => {
    const createCustomFieldSetupOnWorkspaceRequest: CreateCustomFieldSetupOnWorkspaceRequest = {
      workspaceId: WORKSPACE_ID,
      name: "New Text Custom Field",
      description: "Represents a custom field",
      type: 'Text',
      textCustomFieldSetup: {
        defaultText: undefined
      }
    }
    const textCustomField = await createCustomFieldSetupOnWorkspace(request, createCustomFieldSetupOnWorkspaceRequest)

    await addCustomFieldSetupToProject(request, {
      id: textCustomField.id,
      projectId: PROJECT_ID
    })

    const response = await request.delete(
      `/api/customFieldSetups/${textCustomField.id}/remove-project/${PROJECT_ID}`,
      useToken()
    )

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toStrictEqual({
      message: expect.any(String)
    })

    const customFieldSetups = await getCustomFieldSetupsFromProject(request, PROJECT_ID)
    expect(customFieldSetups).toHaveLength(0)
    expect(customFieldSetups).toStrictEqual(expect.not.arrayContaining([
      {
        id: textCustomField.id,
        name: textCustomField.name,
        description: textCustomField.description,
        type: 'Text'
      }
    ]))
  })
})