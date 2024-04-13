import { test } from "@playwright/test";
import { useToken } from "../utils/utils";
import { expect } from "../utils/matchers";
import { deleteUser, registerUser } from "../utils/users.utils";
import { createWorkspace, getPersonalWorkspace } from "../utils/workspaces.utils";
import { create2ProjectsIn2DifferentWorkspaces, createProject, getProjectsForUser } from "../utils/projects.utils";
import CreateProjectRequest from "../types/requests/project/CreateProjectRequest";
import { create } from "node:domain";

test.describe('should allow operations on the project entity', () => {
  let WORKSPACE_ID: string

  test.beforeEach('should register user and get workspace id', async ({ request }) => {
    process.env.API_TOKEN = (await registerUser(request)).token
    WORKSPACE_ID = await getPersonalWorkspace(request)
  })

  test.afterEach('should delete user', async ({ request }) => {
    await deleteUser(request)
  })

  test('should get collection of projects for workspace', async ({ request }) => {
    const { createProjectRequest1 } = await create2ProjectsIn2DifferentWorkspaces(request)

    const response = await request.get(
      `/api/projects/workspace?workspaceId=${createProjectRequest1.workspaceId}`,
      useToken()
    )

    expect(response.ok()).toBeTruthy()

    const json = await response.json()
    expect(json).toStrictEqual({
      projects: expect.arrayContaining([
        {
          id: expect.any(String),
          name: createProjectRequest1.name,
          description: createProjectRequest1.description,
        }
      ])
    })
    expect(json.projects).toHaveLength(1)
  })

  test('should get collection of projects for user', async ({ request }) => {
    const {
      createProjectRequest1,
      createProjectRequest2
    } = await create2ProjectsIn2DifferentWorkspaces(request)

    const response = await request.get(`/api/projects/user`, useToken())

    expect(response.ok()).toBeTruthy()

    const json = await response.json()
    expect(json).toStrictEqual({
      projects: expect.arrayContaining([
        {
          id: expect.any(String),
          name: createProjectRequest1.name
        },
        {
          id: expect.any(String),
          name: createProjectRequest2.name
        }
      ])
    })
    expect(json.projects).toHaveLength(2)
  })

  test('should create project', async ({ request }) => {
    const body: CreateProjectRequest = {
      workspaceId: WORKSPACE_ID,
      name: "New Project",
      description: "This is a new project description",
      isPrivate: false
    }

    const response = await request.post('/api/projects', {
      ...useToken(),
      data: body
    });

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toStrictEqual({
      message: expect.any(String)
    })

    const projects = await getProjectsForUser(request)
    expect(projects).toHaveLength(1)
    expect(projects).toStrictEqual({
      projects: expect.arrayContaining([
        {
          id: expect.any(String),
          name: body.name
        }
      ])
    })
  })

  test('should delete project', async ({ request }) => {
    const project = await createProject(request, {
      workspaceId: WORKSPACE_ID,
      name: "Project name",
      description: "",
      isPrivate: true
    })
    const response = await request.delete(`/api/projects/${project.id}`, useToken());

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toStrictEqual({
      message: expect.any(String)
    })

    const projects = await getProjectsForUser(request)
    await expect(projects).toBeEmpty()
  })
})