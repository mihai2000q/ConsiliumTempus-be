import { APIRequestContext, expect } from "@playwright/test";
import { useToken } from "./utils";
import RegisterRequest from "../types/requests/auth/RegisterRequest";
import LoginRequest from "../types/requests/auth/LoginRequest";

const EMAIL = "michaeljordan@example.com"
const PASSWORD = "Password123"
const FIRSTNAME = "Michael"
const LASTNAME = "Jordan"
const ROLE = "Pro Basketball Player"
const DATE_OF_BIRTH = "2000-12-21"

// every user registered through registerUser, so they can all be removed at the end of the test
const registeredUsers: LoginRequest[] = []

export async function getCurrentUser(request: APIRequestContext, token?: string | undefined) {
  const response = await request.get('api/users/current', useToken(token))

  expect(response.ok()).toBeTruthy()

  return await response.json()
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
  const body: RegisterRequest = {
    email: email,
    password: password,
    firstName: firstName,
    lastName: lastName,
    role: role,
    dateOfBirth: dateOfBirth
  }

  const res = await request.post('api/auth/register', {
    headers: {
      'Authorization': ''
    },
    data: body
  })

  expect(res.ok()).toBeTruthy()

  registeredUsers.push({ email: email, password: password })

  return await res.json()
}

export async function deleteAllUsers(request: APIRequestContext) {
  const failed: string[] = []

  // last registered first, so the collaborators are removed before the owners
  for (const user of registeredUsers.splice(0).reverse()) {
    const loginRes = await request.post('api/auth/login', {
      headers: {
        'Authorization': ''
      },
      data: user
    })

    // the user was already deleted by the test itself
    if (!loginRes.ok()) continue

    const token = (await loginRes.json()).token
    const res = await request.delete('api/users/current', useToken(token))
    if (!res.ok()) failed.push(user.email)
  }

  delete process.env.API_TOKEN

  expect(failed).toEqual([])
}

export async function deleteUser(
  request: APIRequestContext,
  body: LoginRequest = {
    email: EMAIL,
    password: PASSWORD
  }
) {
  const token = process.env.API_TOKEN == undefined
    ? (await loginUser(request, body)).token
    : process.env.API_TOKEN

  const res = await request.delete('api/users/current', useToken(token))

  expect(res.ok()).toBeTruthy()
}

export async function loginUser(
  request: APIRequestContext,
  body: LoginRequest = {
    email: EMAIL,
    password: PASSWORD
  }
) {
  const res = await request.post('api/auth/login', {
    headers: {
      'Authorization': ''
    },
    data: body
  })

  expect(res.ok()).toBeTruthy()

  return await res.json()
}