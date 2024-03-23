import { APIRequestContext } from "@playwright/test";

export async function registerUser(request: APIRequestContext, email: String, password: String) {
  return await request.post('api/auth/register', {
    headers: {
      'Authorization': ''
    },
    data: {
      firstName: "Michael",
      lastName: "Jordan",
      email: email,
      password: password,
      role: "Pro Basketball Player",
      dateOfBirth: "2000-12-21"
    }
  })
}

export async function loginUser(request: APIRequestContext, email: String, password: String) {
  return await request.post('api/auth/login', {
    headers: {
      'Authorization': ''
    },
    data: {
      email: email,
      password: password,
    }
  })
}

export async function deleteUser(request: APIRequestContext, email: String, password: String) {
  let res = await loginUser(request, email, password)

  let token = (await res.json()).token

  return await request.delete('api/users', {
    headers: {
      'Authorization': `Bearer ${token}`
    }
  })
}