import { expect, test } from '@playwright/test';
import LoginRequest from "../types/requests/auth/LoginRequest";
import { deleteUser, registerUser } from "../utils/users.utils";
import RefreshRequest from "../types/requests/auth/RefreshRequest";
import RegisterRequest from "../types/requests/auth/RegisterRequest";

test.describe('should allow anonymous authentication', () => {
  const EMAIL = "michaeljordan@example.com"
  const PASSWORD = "Password123"

  test.afterEach('should delete user', async ({ request }) => {
    await deleteUser(request, EMAIL, PASSWORD)
  })

  test('should register and return tokens', async ({ request }) => {
    const body: RegisterRequest = {
      firstName: "Michael",
      lastName: "Jordan",
      email: EMAIL,
      password: PASSWORD,
      role: "Pro Basketball Player",
      dateOfBirth: "2000-12-21"
    }

    const response = await request.post('api/auth/register', {
      headers: {
        'Authorization': ''
      },
      data: body
    })

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toStrictEqual({
      token: expect.any(String),
      refreshToken: expect.any(String)
    })
  })

  test('should login and return tokens', async ({ request }) => {
    await registerUser(request, EMAIL, PASSWORD)

    const body: LoginRequest = {
      email: EMAIL,
      password: PASSWORD
    }

    const response = await request.post('api/auth/login', {
      headers: {
        'Authorization': ''
      },
      data: body
    })

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toStrictEqual({
      token: expect.any(String),
      refreshToken: expect.any(String)
    })
  })

  test('should refresh token', async ({ request }) => {
    const tokens = await registerUser(request, EMAIL, PASSWORD)

    const body: RefreshRequest = {
      token: tokens.token,
      refreshToken: tokens.refreshToken
    }

    const response = await request.put('api/auth/refresh', {
      headers: {
        'Authorization': ''
      },
      data: body
    })

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toStrictEqual({
      token: expect.any(String)
    })
  })
})