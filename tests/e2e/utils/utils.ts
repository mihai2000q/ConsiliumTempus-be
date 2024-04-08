import { APIRequestContext, expect } from "@playwright/test";

const EMAIL = "michaeljordan@example.com"
const PASSWORD = "Password123"
const FIRSTNAME = "Michael"
const LASTNAME = "Jordan"
const ROLE = "Pro Basketball Player"
const DATE_OF_BIRTH = "2000-12-21"

export function useToken(
  token: string = process.env.API_TOKEN!
) {
  return {
    headers: {
      'Authorization': `Bearer ${token}`
    }
  }
}

export async function getCurrentUser(request: APIRequestContext) {
  const response = await request.get('api/users/current', {
    headers: {
      'Authorization': `Bearer ${process.env.API_TOKEN}`
    }
  })

  expect(response.ok()).toBeTruthy()

  return await response.json()
}

export async function getUserId(request: APIRequestContext) {
  const response = await request.get('api/users/current', {
    headers: {
      'Authorization': `Bearer ${process.env.API_TOKEN}`
    }
  })

  expect(response.ok()).toBeTruthy()

  return (await response.json()).id as string
}

export async function registerUser(
  request: APIRequestContext,
  email: string = EMAIL,
  password: string = PASSWORD,
  firstName: string = FIRSTNAME,
  lastName: string = LASTNAME,
  role: string = ROLE,
  dateOfBirth: string = DATE_OF_BIRTH
) {
  const res = await request.post('api/auth/register', {
    headers: {
      'Authorization': ''
    },
    data: {
      firstName: firstName,
      lastName: lastName,
      email: email,
      password: password,
      role: role,
      dateOfBirth: dateOfBirth
    }
  })

  expect(res.ok()).toBeTruthy()

  return await res.json()
}

export async function deleteUser(
  request: APIRequestContext,
  email: String = EMAIL,
  password: String = PASSWORD
) {
  const tokens = await loginUser(request, email, password)
  const token = process.env.API_TOKEN == undefined ? tokens.token : process.env.API_TOKEN

  const res = await request.delete('api/users/current', useToken(token))

  expect(res.ok()).toBeTruthy()
}

export async function loginUser(
  request: APIRequestContext,
  email: String = EMAIL,
  password: String = PASSWORD
) {
  const res = await request.post('api/auth/login', {
    headers: {
      'Authorization': ''
    },
    data: {
      email: email,
      password: password,
    }
  })

  expect(res.ok()).toBeTruthy()

  return await res.json()
}