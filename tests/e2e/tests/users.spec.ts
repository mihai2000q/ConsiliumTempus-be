import { expect } from '../utils/matchers';
import { test } from "@playwright/test";
import { deleteUser, getCurrentUser, getUserId, registerUser, useToken } from "../utils/utils";

test.describe('should allow operations on the user entity', () => {
  const EMAIL = "michaeljordan@example.com"
  const PASSWORD = "Password123"
  const FIRSTNAME = "Michael"
  const LASTNAME = "Jordan"
  const ROLE = "Pro Basketball Player"
  const DATE_OF_BIRTH = "2000-12-21"

  test.beforeEach('should register and create token', async ({ request }) => {
    process.env.API_TOKEN = (await registerUser(
        request,
        EMAIL,
        PASSWORD,
        FIRSTNAME,
        LASTNAME,
        ROLE,
        DATE_OF_BIRTH
      )
    ).token
  })

  test.afterEach('should delete user', async ({ request }) => {
    await deleteUser(request)
  })

  test('should get user', async ({ request }) => {
    const userId = await getUserId(request)
    const response = await request.get(`api/users/${userId}`, useToken())

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toEqual({
      firstName: FIRSTNAME,
      lastName: LASTNAME,
      email: EMAIL,
      role: ROLE
    })
  })

  test('should get current user', async ({ request }) => {
    const response = await request.get('api/users/current', useToken())

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toEqual({
      id: expect.any(String),
      firstName: FIRSTNAME,
      lastName: LASTNAME,
      email: EMAIL,
      role: ROLE,
      dateOfBirth: DATE_OF_BIRTH
    })
  })

  test('should update current user', async ({ request }) => {
    const body = {
      firstName: "Michelle",
      lastName: "Moron",
      role: "CEO",
      dateOfBirth: null
    }

    const response = await request.put('api/users/current', {
      ...useToken(),
      data: body
    })

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toEqual({
      message: expect.any(String)
    })

    const newUser = await getCurrentUser(request)

    expect(newUser).toEqual({
      id: expect.any(String),
      firstName: body.firstName,
      lastName: body.lastName,
      email: EMAIL,
      role: body.role,
      dateOfBirth: body.dateOfBirth
    })
  })
})

test.describe('should allow removal of the user entity', () => {
  test('should delete current user', async ({ request }) => {
    process.env.API_TOKEN = (await registerUser(request)).token

    const response = await request.delete('api/users/current', useToken())

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toEqual({
      message: expect.any(String)
    })

    const getCurrentUserResponse = await request.get('/api/users/current', useToken())
    expect(getCurrentUserResponse.ok()).toBeFalsy()
  })
})