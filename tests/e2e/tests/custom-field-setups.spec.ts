import { test } from "@playwright/test";
import { deleteUser, registerUser } from "../utils/users.utils";
import { getPersonalWorkspace } from "../utils/workspaces.utils";
import { createProject } from "../utils/projects.utils";
import { useToken } from "../utils/utils";
import { expect } from "../utils/matchers";
import CreateCustomFieldSetupOnProjectRequest
  from "../types/requests/custom-field-setup/CreateCustomFieldSetupOnProjectRequest";
import { createCustomFieldSetup, getCustomFieldSetupsFromProject } from "../utils/custom-field-setup.utils";

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
      const numberCustomField = await createCustomFieldSetup(request, createCustomFieldSetupOnProjectRequest)
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
      const singleSelectCustomField = await createCustomFieldSetup(request, createCustomFieldSetupOnProjectRequest)
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
              id: createCustomFieldSetupOnProjectRequest.singleSelectCustomFieldSetup!.options[0].id,
              value: createCustomFieldSetupOnProjectRequest.singleSelectCustomFieldSetup!.options[0].value,
              color: createCustomFieldSetupOnProjectRequest.singleSelectCustomFieldSetup!.options[0].color,
            },
            {
              id: createCustomFieldSetupOnProjectRequest.singleSelectCustomFieldSetup!.options[1].id,
              value: createCustomFieldSetupOnProjectRequest.singleSelectCustomFieldSetup!.options[1].value,
              color: createCustomFieldSetupOnProjectRequest.singleSelectCustomFieldSetup!.options[1].color,
            }
          ],
          defaultOption: {
            id: createCustomFieldSetupOnProjectRequest.singleSelectCustomFieldSetup!.options[0].id,
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
      const textCustomField = await createCustomFieldSetup(request, createCustomFieldSetupOnProjectRequest)
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

  test('should get custom field setups from project', async ({ request }) => {
    const textCustomField = await createCustomFieldSetup(request, {
      projectId: PROJECT_ID,
      name: "New Text Custom Field",
      description: "Represents a custom field",
      type: 'Text',
      textCustomFieldSetup: {
        defaultText: undefined
      }
    })
    const numberCustomField = await createCustomFieldSetup(request, {
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
    const singleSelectCustomField = await createCustomFieldSetup(request, {
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

    const response = await request.get(`/api/customFieldSetups/project/${PROJECT_ID}`, useToken())

    expect(response.ok()).toBeTruthy()

    const json = await response.json()
    expect(json.customFieldSetups).toHaveLength(3)
    expect(json.customFieldSetups).toStrictEqual(expect.arrayContaining([
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
    ]))
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
          description: body.description
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
          description: body.description
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
        }
      ])
    })
  })

  test('should delete custom field setup', async ({ request }) => {
    const textCustomField = await createCustomFieldSetup(request, {
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
})