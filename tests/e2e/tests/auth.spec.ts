import { expect, test } from '@playwright/test';
import { deleteUser, registerUser } from "../utils/utils";

test.describe('should allow anonymous authentication', () => {
  const EMAIL = "michaeljordan@example.com"
  const PASSWORD = "Password123"

  test.afterEach('should delete user', async ({ request }) => {
    await deleteUser(request, EMAIL, PASSWORD)
  })

  test('should register and return tokens', async ({ request }) => {
    const response = await request.post('api/auth/register', {
      headers: {
        'Authorization': ''
      },
      data: {
        firstName: "Michael",
        lastName: "Jordan",
        email: EMAIL,
        password: PASSWORD,
        role: "Pro Basketball Player",
        dateOfBirth: "2000-12-21"
      }
    })

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toEqual({
      token: expect.any(String),
      refreshToken: expect.any(String)
    })
  })

  test('should login and return tokens', async ({ request }) => {
    await registerUser(request, EMAIL, PASSWORD)

    const response = await request.post('api/auth/login', {
      headers: {
        'Authorization': ''
      },
      data: {
        email: EMAIL,
        password: PASSWORD
      }
    })

    expect(response.ok()).toBeTruthy()

    expect(await response.json()).toEqual({
      token: expect.any(String),
      refreshToken: expect.any(String)
    })
  })
})