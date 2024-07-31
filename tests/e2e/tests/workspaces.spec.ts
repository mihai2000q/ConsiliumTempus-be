import { test } from "@playwright/test";
import { useToken } from "../utils/utils";
import { expect } from "../utils/matchers";
import {
  addCollaboratorToWorkspace,
  createWorkspace,
  createWorkspaces,
  getCollaborators,
  getInvitation,
  getInvitations,
  getInvitationsByWorkspace,
  getPersonalWorkspace,
  getWorkspace,
  getWorkspaceOverview,
  getWorkspaces,
  inviteCollaborator,
  inviteCollaboratorWithRegister,
  inviteMeToWorkspace, inviteThemToWorkspace
} from "../utils/workspaces.utils";
import { PersonalWorkspaceName } from "../utils/constants";
import { deleteUser, registerUser } from "../utils/users.utils";
import CreateWorkspaceRequest from "../types/requests/workspace/CreateWorkspaceRequest";
import UpdateWorkspaceRequest from "../types/requests/workspace/UpdateWorkspaceRequest";
import UpdateOverviewWorkspaceRequest from "../types/requests/workspace/UpdateOverviewWorkspaceRequest";
import UpdateFavoritesWorkspaceRequest from "../types/requests/workspace/UpdateFavoritesWorkspaceRequest";
import InviteCollaboratorToWorkspaceRequest from "../types/requests/workspace/InviteCollaboratorToWorkspaceRequest";
import AcceptInvitationToWorkspaceRequest from "../types/requests/workspace/AcceptInvitationToWorkspaceRequest";
import LeaveWorkspaceRequest from "../types/requests/workspace/LeaveWorkspaceRequest";
import UpdateOwnerWorkspaceRequest from "../types/requests/workspace/UpdateOwnerWorkspaceRequest";
import UpdateCollaboratorFromWorkspaceRequest from "../types/requests/workspace/UpdateCollaboratorFromWorkspaceRequest";

test.describe('should allow operations on the workspace entity', () => {
  const EMAIL = "michaelj@gmail.com"

  test.beforeEach('should register and create token', async ({ request }) => {
    process.env.API_TOKEN = (await registerUser(request, EMAIL)).token
  })

  test.afterEach('should delete user', async ({ request }) => {
    await deleteUser(request)
  })

  test('should get workspace', async ({ request }) => {
    const workspace = await getPersonalWorkspace(request);

    const response = await request.get(`/api/workspaces/${workspace.id}`, useToken())

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toStrictEqual({
      name: workspace.name,
      isPersonal: true,
      isFavorite: false,
      owner: {
        id: expect.any(String),
        name: expect.any(String),
        email: EMAIL
      }
    })
  })

  test('should get workspace overview', async ({ request }) => {
    const workspace = await getPersonalWorkspace(request);

    const response = await request.get(`/api/workspaces/overview/${workspace.id}`, useToken())

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toStrictEqual({
      description: workspace.description,
    })
  })

  test.describe('should allow to get collection of workspaces', async () => {
    test('should get collection of workspaces', async ({ request }) => {
      const createWorkspaceRequest: CreateWorkspaceRequest = {
        name: "Some Workspace",
      }
      await createWorkspace(request, createWorkspaceRequest)

      const response = await request.get('/api/workspaces', useToken())

      expect(response.ok()).toBeTruthy()

      const json = await response.json()
      expect(json).toStrictEqual({
        workspaces: expect.arrayContaining([
          {
            id: expect.any(String),
            name: PersonalWorkspaceName,
            description: expect.any(String),
            isFavorite: false,
            isPersonal: true,
            owner: expect.any(Object)
          },
          {
            id: expect.any(String),
            name: createWorkspaceRequest.name,
            description: "",
            isFavorite: false,
            isPersonal: false,
            owner: expect.any(Object)
          }
        ]),
        totalCount: 2
      })
      expect(json.workspaces).toHaveLength(2)
    })

    test('should get collection of workspaces with the personal workspace on top', async ({ request }) => {
      const createWorkspaceRequest: CreateWorkspaceRequest = {
        name: "Some Workspace",
      }
      await createWorkspace(request, createWorkspaceRequest)

      const response = await request.get('/api/workspaces?isPersonalWorkspaceFirst=true', useToken())

      expect(response.ok()).toBeTruthy()

      const json = await response.json()
      expect(json).toStrictEqual({
        workspaces: [
          {
            id: expect.any(String),
            name: PersonalWorkspaceName,
            description: expect.any(String),
            isFavorite: false,
            isPersonal: true,
            owner: expect.any(Object)
          },
          {
            id: expect.any(String),
            name: createWorkspaceRequest.name,
            description: "",
            isFavorite: false,
            isPersonal: false,
            owner: expect.any(Object)
          }
        ],
        totalCount: 2
      })
      expect(json.workspaces).toHaveLength(2)
    })

    test('should get collection of workspaces ordered by create date time descending', async ({ request }) => {
      const createWorkspaceRequests = await createWorkspaces(request, 2)

      const response = await request.get('/api/workspaces?orderBy=created_date_time.desc', useToken())

      expect(response.ok()).toBeTruthy()

      const json = await response.json()
      expect(json).toStrictEqual({
        workspaces: [
          {
            id: expect.any(String),
            name: createWorkspaceRequests[1].name,
            description: "",
            isFavorite: false,
            isPersonal: false,
            owner: expect.any(Object)
          },
          {
            id: expect.any(String),
            name: createWorkspaceRequests[0].name,
            description: "",
            isFavorite: false,
            isPersonal: false,
            owner: expect.any(Object)
          },
          {
            id: expect.any(String),
            name: PersonalWorkspaceName,
            description: expect.any(String),
            isFavorite: false,
            isPersonal: true,
            owner: expect.any(Object)
          }
        ],
        totalCount: 3
      })
      expect(json.workspaces).toHaveLength(3)
    })

    test('should get collection of workspaces filtered by name', async ({ request }) => {
      const createWorkspaceRequest: CreateWorkspaceRequest = {
        name: "Some Workspace",
      }
      await createWorkspace(request, createWorkspaceRequest)

      const response = await request.get('/api/workspaces?search=name ct some works', useToken())

      expect(response.ok()).toBeTruthy()

      const json = await response.json()
      expect(json).toStrictEqual({
        workspaces: [
          {
            id: expect.any(String),
            name: createWorkspaceRequest.name,
            description: "",
            isFavorite: false,
            isPersonal: false,
            owner: expect.any(Object)
          }
        ],
        totalCount: 1
      })
      expect(json.workspaces).toHaveLength(1)
    })

    test('should get collection of workspaces paginated and ordered by name ascending', async ({ request }) => {
      const totalCount = 5
      const createWorkspaceRequests = await createWorkspaces(request, totalCount - 1)

      const pageSize = 6
      const currentPage = 1
      const response = await request.get(`/api/workspaces?orderBy=name.asc&pageSize=${pageSize}&currentPage=${currentPage}`, useToken())

      expect(response.ok()).toBeTruthy()

      const start = pageSize * (currentPage - 1)
      const expectedWorkspaces: any[] = createWorkspaceRequests
        .slice(start, start + pageSize)
        .map(r => {
          return {
            id: expect.any(String),
            name: r.name,
            description: "",
            isFavorite: false,
            isPersonal: false,
            owner: expect.any(Object)
          }
        })
      expectedWorkspaces.unshift({
        id: expect.any(String),
        name: PersonalWorkspaceName,
        description: expect.any(String),
        isFavorite: false,
        isPersonal: true,
        owner: expect.any(Object)
      })

      const json = await response.json()
      expect(json).toStrictEqual({
        workspaces: expectedWorkspaces,
        totalCount: totalCount
      })
      expect(json.workspaces).toHaveLength(totalCount < pageSize ? totalCount : pageSize)
    })
  })

  test('should get collaborators', async ({ request }) => {
    const workspace = await createWorkspace(request, {
      name: "Some Random Workspace"
    });

    const response = await request.get(`/api/workspaces/${workspace.id}/collaborators`, useToken())

    expect(response.ok()).toBeTruthy()

    const json = await response.json()
    expect(json.collaborators).toHaveLength(1)
    expect(json.collaborators).toStrictEqual([
      {
        id: expect.any(String),
        name: expect.any(String),
        email: EMAIL,
      }
    ])
  })

  test.describe('should allow to get invitations', async () => {
    test('should get invitations for sender', async ({ request }) => {
      const workspace = await createWorkspace(request)

      const collaboratorEmail1 = "hector_lector@gmail.com"
      const collaboratorName1 = "Hector Lector"
      await inviteCollaboratorWithRegister(
        request,
        collaboratorEmail1,
        collaboratorName1.split(" ")[0],
        collaboratorName1.split(" ")[1],
        workspace.id
      )

      const collaboratorEmail2 = "michelle_obama_official_account@gmail.com"
      const collaboratorName2 = "Michelle Obama"
      await inviteCollaboratorWithRegister(
        request,
        collaboratorEmail2,
        collaboratorName2.split(" ")[0],
        collaboratorName2.split(" ")[1],
        workspace.id
      )

      const response = await request.get(`/api/workspaces/invitations?isSender=true`, useToken())

      expect(response.ok()).toBeTruthy()

      const expectedWorkspace = {
        id: workspace.id,
        name: workspace.name,
        isPersonal: workspace.isPersonal,
      }

      const json = await response.json()
      expect(json.invitations).toHaveLength(2)
      expect(json).toStrictEqual(
        {
          invitations: [
            {
              id: expect.any(String),
              sender: {
                id: expect.any(String),
                email: EMAIL,
                name: expect.any(String)
              },
              workspace: expectedWorkspace,
              collaborator: {
                id: expect.any(String),
                email: collaboratorEmail2,
                name: collaboratorName2
              }
            },
            {
              id: expect.any(String),
              sender: {
                id: expect.any(String),
                email: EMAIL,
                name: expect.any(String)
              },
              workspace: expectedWorkspace,
              collaborator: {
                id: expect.any(String),
                email: collaboratorEmail1,
                name: collaboratorName1
              }
            },
          ],
          totalCount: 2
        }
      )
    })

    test('should get invitations for collaborator', async ({ request }) => {
      const senderEmail1 = "schecter_guitar@gmail.com"
      const token1 = (await registerUser(request, senderEmail1)).token
      const workspace1 = (await getPersonalWorkspace(request, token1))
      await inviteCollaborator(
        request,
        {
          id: workspace1.id,
          email: EMAIL
        },
        token1
      )

      const senderEmail2 = "ibanez_guitar@gmail.com"
      const token2 = (await registerUser(request, senderEmail2)).token
      const workspace2 = (await getPersonalWorkspace(request, token2))
      await inviteCollaborator(
        request,
        {
          id: workspace2.id,
          email: EMAIL
        },
        token2
      )

      const response = await request.get(`/api/workspaces/invitations?isSender=false`, useToken())

      expect(response.ok()).toBeTruthy()

      const expectedCollaborator = {
        id: expect.any(String),
        email: EMAIL,
        name: expect.any(String)
      }

      const json = await response.json()
      expect(json.invitations).toHaveLength(2)
      expect(json).toStrictEqual(
        {
          invitations: [
            {
              id: expect.any(String),
              collaborator: expectedCollaborator,
              sender: {
                id: expect.any(String),
                email: senderEmail2,
                name: expect.any(String)
              },
              workspace: {
                id: workspace2.id,
                name: workspace2.name,
                isPersonal: workspace2.isPersonal,
              },
            },
            {
              id: expect.any(String),
              collaborator: expectedCollaborator,
              sender: {
                id: expect.any(String),
                email: senderEmail1,
                name: expect.any(String)
              },
              workspace: {
                id: workspace1.id,
                name: workspace1.name,
                isPersonal: workspace1.isPersonal,
              },
            }
          ],
          totalCount: 2
        }
      )
    })

    test('should get invitations for workspace', async ({ request }) => {
      const workspace1 = await createWorkspace(request, {
        name: "Another Random Workspace"
      })
      const workspace2 = await createWorkspace(request, {
        name: "Let's see, is this another workspace?"
      })

      const collaboratorEmail1 = "someone@gmail.com"
      const collaboratorName1 = "Someone Else"
      await inviteCollaboratorWithRegister(
        request,
        collaboratorEmail1,
        collaboratorName1.split(" ")[0],
        collaboratorName1.split(" ")[1],
        workspace1.id
      )

      const collaboratorEmail2 = "michelle_obama@gmail.com"
      const collaboratorName2 = "Michelle Obama"
      await inviteCollaboratorWithRegister(
        request,
        collaboratorEmail2,
        collaboratorName2.split(" ")[0],
        collaboratorName2.split(" ")[1],
        workspace2.id
      )

      const expectedSender = {
        id: expect.any(String),
        email: EMAIL,
        name: expect.any(String)
      }
      const expectedWorkspace = {
        id: workspace2.id,
        name: workspace2.name,
        isPersonal: workspace2.isPersonal,
      }

      const response = await request.get(`/api/workspaces/invitations?workspaceId=${workspace2.id}`, useToken())

      expect(response.ok()).toBeTruthy()

      const json = await response.json()
      expect(json.invitations).toHaveLength(1)
      expect(json).toStrictEqual(
        {
          invitations: [
            {
              id: expect.any(String),
              sender: expectedSender,
              workspace: expectedWorkspace,
              collaborator: {
                id: expect.any(String),
                email: collaboratorEmail2,
                name: collaboratorName2
              }
            }
          ],
          totalCount: 1
        }
      )
    })

    test('should get invitations for workspace with pagination', async ({ request }) => {
      const workspace = await createWorkspace(request, {
        name: "perhaps, another workspace?"
      })

      const collaboratorEmail1 = "howard_impact@gmail.com"
      const collaboratorName1 = "Howard Impact"
      await inviteCollaboratorWithRegister(
        request,
        collaboratorEmail1,
        collaboratorName1.split(" ")[0],
        collaboratorName1.split(" ")[1],
        workspace.id
      )

      const collaboratorEmail2 = "tremolo_base@gmail.com"
      const collaboratorName2 = "Tremolo Base"
      await inviteCollaboratorWithRegister(
        request,
        collaboratorEmail2,
        collaboratorName2.split(" ")[0],
        collaboratorName2.split(" ")[1],
        workspace.id
      )

      const response = await request.get(
        `/api/workspaces/invitations?workspaceId=${workspace.id}&pageSize=1&currentPage=2`,
        useToken()
      )

      expect(response.ok()).toBeTruthy()

      const expectedSender = {
        id: expect.any(String),
        email: EMAIL,
        name: expect.any(String)
      }
      const expectedWorkspace = {
        id: workspace.id,
        name: workspace.name,
        isPersonal: workspace.isPersonal,
      }

      const json = await response.json()
      expect(json.invitations).toHaveLength(1)
      expect(json).toStrictEqual(
        {
          invitations: [
            {
              id: expect.any(String),
              sender: expectedSender,
              workspace: expectedWorkspace,
              collaborator: {
                id: expect.any(String),
                email: collaboratorEmail1,
                name: collaboratorName1
              }
            },
          ],
          totalCount: 2
        }
      )
    })
  })

  test('should create workspace', async ({ request }) => {
    const createWorkspaceRequest: CreateWorkspaceRequest = {
      name: "New Workspace"
    }
    const response = await request.post('/api/workspaces', {
      ...useToken(),
      data: createWorkspaceRequest
    })

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toStrictEqual({
      message: expect.any(String)
    })

    const workspaces = await getWorkspaces(request)
    expect(workspaces).toStrictEqual(expect.arrayContaining([
      {
        id: expect.any(String),
        name: createWorkspaceRequest.name,
        description: "",
        isFavorite: false,
        isPersonal: false,
        owner: expect.any(Object)
      }
    ]))
  })

  test('should invite collaborator to workspace', async ({ request }) => {
    const collaboratorEmail = "julian_goth@gmail.com"
    const collaboratorName = "Julian Goth"
    await registerUser(
      request,
      collaboratorEmail,
      "Some Password 123",
      collaboratorName.split(" ")[0],
      collaboratorName.split(" ")[1]
    )

    const workspace = await createWorkspace(request, {
      name: "Some other workspace"
    });

    const body: InviteCollaboratorToWorkspaceRequest = {
      id: workspace.id,
      email: collaboratorEmail,
    }
    const response = await request.post(`/api/workspaces/invite-collaborator`, {
      ...useToken(),
      data: body
    })

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toStrictEqual({
      message: expect.any(String)
    })

    const invitations = await getInvitationsByWorkspace(request, workspace.id)
    expect(invitations).toHaveLength(1)
    expect(invitations).toStrictEqual([
      {
        id: expect.any(String),
        sender: {
          id: expect.any(String),
          email: EMAIL,
          name: expect.any(String)
        },
        workspace: {
          id: workspace.id,
          name: workspace.name,
          isPersonal: workspace.isPersonal
        },
        collaborator: {
          id: expect.any(String),
          email: collaboratorEmail,
          name: collaboratorName
        }
      }
    ])
  })

  test('should accept invitation to workspace', async ({ request }) => {
    const senderEmail = "sender_email@gmail.com"
    const token = (await registerUser(request, senderEmail)).token
    const workspace = await createWorkspace(request, { name: "yet another workspace" }, token)
    await inviteCollaborator(
      request,
      {
        id: workspace.id,
        email: EMAIL
      },
      token
    )
    const invitation = await getInvitation(request, workspace.id)

    const body: AcceptInvitationToWorkspaceRequest = {
      id: workspace.id,
      invitationId: invitation.id,
    }
    const response = await request.post(`/api/workspaces/accept-invitation`, {
        ...useToken(),
        data: body
      }
    )

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toStrictEqual({
      message: expect.any(String)
    })

    const invitations = await getInvitations(request)
    expect(invitations).toHaveLength(0)

    const collaborators = await getCollaborators(request, workspace.id)
    expect(collaborators).toHaveLength(2)
    expect(collaborators).toStrictEqual([
      {
        id: expect.any(String),
        name: expect.any(String),
        email: EMAIL,
      },
      {
        id: expect.any(String),
        name: expect.any(String),
        email: senderEmail,
      }
    ])
  })

  test('should reject invitation to workspace', async ({ request }) => {
    const senderEmail = "sender_email2@gmail.com"
    const token = (await registerUser(request, senderEmail)).token
    const workspace = await createWorkspace(request, { name: "yet another workspace to reject" }, token)
    await inviteCollaborator(
      request,
      {
        id: workspace.id,
        email: EMAIL
      },
      token
    )
    const invitation = await getInvitation(request, workspace.id)

    const body: AcceptInvitationToWorkspaceRequest = {
      id: workspace.id,
      invitationId: invitation.id,
    }
    const response = await request.post(`/api/workspaces/reject-invitation`, {
        ...useToken(),
        data: body
      }
    )

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toStrictEqual({
      message: expect.any(String)
    })
    
    const invitations = await getInvitations(request)
    expect(invitations).toHaveLength(0)
  })

  test('should leave workspace', async ({ request }) => {
    const workspace = await inviteMeToWorkspace(request, "somebody_else@yahoo.com", EMAIL)

    const body: LeaveWorkspaceRequest = {
      id: workspace.id,
    }
    const response = await request.post('/api/workspaces/leave', {
      ...useToken(),
      data: body
    })

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toStrictEqual({
      message: expect.any(String)
    })

    const workspaces = await getWorkspaces(request)
    expect(workspaces).toEqual(expect.not.arrayContaining([
      {
        id: workspace.id
      }
    ]))
  })

  test('should update workspace', async ({ request }) => {
    const createWorkspaceRequest: CreateWorkspaceRequest = {
      name: "Some workspace",
    }
    const workspace = await createWorkspace(request, createWorkspaceRequest)

    const body: UpdateWorkspaceRequest = {
      id: workspace.id,
      name: "New Workspace Name"
    }
    const response = await request.put('/api/workspaces', {
      ...useToken(),
      data: body
    })

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toStrictEqual({
      message: expect.any(String)
    })

    const newWorkspace = await getWorkspace(request, body.id)
    expect(newWorkspace).toStrictEqual({
      name: body.name,
      isFavorite: false,
      isPersonal: false,
      owner: expect.any(Object)
    })
  })

  test('should update collaborator from workspace', async ({ request }) => {
    const workspace = await createWorkspace(request, {
      name: "Some Random Workspace"
    });
    const collaborator = await addCollaboratorToWorkspace(request, "some_random_guy@gmaill.com", workspace.id)

    const body: UpdateCollaboratorFromWorkspaceRequest = {
      id: workspace.id,
      collaboratorId: collaborator.id,
      workspaceRole: 'member'
    }
    const response = await request.put(`/api/workspaces/collaborators`, {
      ...useToken(),
      data: body
    })

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toStrictEqual({
      message: expect.any(String)
    })

    const collaborators = await getCollaborators(request, workspace.id)
    expect(collaborators).toHaveLength(2)
    expect(collaborators).toStrictEqual([
      {
        id: expect.any(String),
        name: expect.any(String),
        email: EMAIL,
      },
      {
        id: body.id,
        name: collaborator.firstName + " " + collaborator.lastName,
        email: collaborator.email,
      }
    ])
  })

  test('should update workspace favorites', async ({ request }) => {
    const createWorkspaceRequest: CreateWorkspaceRequest = {
      name: "Some workspace",
    }
    const workspace = await createWorkspace(request, createWorkspaceRequest)

    const body: UpdateFavoritesWorkspaceRequest = {
      id: workspace.id,
      isFavorite: true
    }
    const response = await request.put('/api/workspaces/favorites', {
      ...useToken(),
      data: body
    })

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toStrictEqual({
      message: expect.any(String)
    })

    const newWorkspace = await getWorkspace(request, body.id)
    expect(newWorkspace).toStrictEqual({
      name: createWorkspaceRequest.name,
      isFavorite: body.isFavorite,
      isPersonal: false,
      owner: expect.any(Object)
    })
  })

  test('should update workspace overview', async ({ request }) => {
    const createWorkspaceRequest: CreateWorkspaceRequest = {
      name: "Some workspace",
    }
    const workspace = await createWorkspace(request, createWorkspaceRequest)

    const body: UpdateOverviewWorkspaceRequest = {
      id: workspace.id,
      description: "This is a new workspace description"
    }
    const response = await request.put('/api/workspaces/overview', {
      ...useToken(),
      data: body
    })

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toStrictEqual({
      message: expect.any(String)
    })

    const newWorkspace = await getWorkspaceOverview(request, body.id)
    expect(newWorkspace).toStrictEqual({
      description: body.description,
    })
  })

  test('should update workspace owner', async ({ request }) => {
    const [workspace, owner] = await inviteThemToWorkspace(request, "guapo@email.com")

    const body: UpdateOwnerWorkspaceRequest = {
      id: workspace.id,
      ownerId: owner.id
    }
    const response = await request.put('/api/workspaces/owner', {
      ...useToken(),
      data: body
    })

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toStrictEqual({
      message: expect.any(String)
    })

    const newWorkspace = await getWorkspace(request, body.id)
    expect(newWorkspace).toStrictEqual({
      name: workspace.name,
      isFavorite: workspace.isFavorite,
      isPersonal: workspace.isPersonal,
      owner: expect.objectContaining({
        id: body.ownerId
      })
    })
  })

  test('should delete workspace', async ({ request }) => {
    const createWorkspaceRequest: CreateWorkspaceRequest = {
      name: "New Team",
    }
    const workspace = await createWorkspace(request, createWorkspaceRequest)

    const response = await request.delete(`/api/workspaces/${workspace.id}`, useToken())

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toStrictEqual({
      message: expect.any(String)
    })

    const newWorkspaces = await getWorkspaces(request)
    expect(newWorkspaces).not.toStrictEqual(expect.arrayContaining([workspace]))
  })
})