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

  test('should get custom field setups from project', async ({ request }) => {
    const textCustomField = await createCustomFieldSetup(request, {
      projectId: PROJECT_ID,
      name: "New Text Custom Field",
      description: "Represents a custom field",
      type: 'text'
    })
    const numberCustomField = await createCustomFieldSetup(request, {
      projectId: PROJECT_ID,
      name: "Budget",
      description: "Represents a custom field",
      type: 'number',
      numberSettings: {
        currencyCode: "USD",
        decimals: 2,
        rounding: true
      }
    })
    const singleSelectCustomField = await createCustomFieldSetup(request, {
      projectId: PROJECT_ID,
      name: "Priority",
      description: "Represents a custom field",
      type: 'singleSelect',
      singleSelectOptions: [
        {
          value: "High",
          color: "#FF1122"
        },
        {
          value: "Low",
          color: "#1122FF"
        }
      ]
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
        settings: {
          currencyCode: numberCustomField.settings.currencyCode,
          decimals: numberCustomField.settings.decimals,
          rounding: numberCustomField.settings.rounding,
        }
      },
      {
        id: singleSelectCustomField.id,
        name: singleSelectCustomField.name,
        description: singleSelectCustomField.description,
        options: [
          {
            value: singleSelectCustomField.options[0].value,
            color: singleSelectCustomField.options[0].color,
          },
          {
            value: singleSelectCustomField.options[1].value,
            color: singleSelectCustomField.options[1].color,
          }
        ]
      },
      {
        id: textCustomField.id,
        name: textCustomField.name,
        description: textCustomField.description,
      }
    ]))
  })

  test.describe(`should allow creation of custom field setup on project`, () => {
    test('should create number custom field setup on project', async ({ request }) => {
      const body: CreateCustomFieldSetupOnProjectRequest = {
        projectId: PROJECT_ID,
        name: "Budget",
        description: "Represents a custom field",
        type: 'number',
        numberSettings: {
          currencyCode: "USD",
          decimals: 2,
          rounding: true
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
          settings: {
            currencyCode: body.numberSettings!.currencyCode,
            decimals: body.numberSettings!.decimals,
            rounding: body.numberSettings!.rounding,
          }
        }
      ])
    })

    test('should create single select custom field setup on project', async ({ request }) => {
      const body: CreateCustomFieldSetupOnProjectRequest = {
        projectId: PROJECT_ID,
        name: "Priority",
        description: "Represents a custom field",
        type: 'singleSelect',
        singleSelectOptions: [
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
          options: [
            {
              value: body.singleSelectOptions![0].value,
              color: body.singleSelectOptions![0].color,
            },
            {
              value: body.singleSelectOptions![1].value,
              color: body.singleSelectOptions![1].color,
            }
          ]
        }
      ])
    })

    test('should create text custom field setup on project', async ({ request }) => {
      const body: CreateCustomFieldSetupOnProjectRequest = {
        projectId: PROJECT_ID,
        name: "New Text Custom Field",
        description: "Represents a custom field",
        type: 'text'
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
})