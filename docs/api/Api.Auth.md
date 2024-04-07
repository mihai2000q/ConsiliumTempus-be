# Consilium Tempus API

- [Auth](#auth)
    - [Register](#register)
        - [Register Request](#register-request)
        - [Register Response](#register-response)
    - [Login](#login)
        - [Login Request](#login-request)
        - [Login Response](#login-response)
    - [Refresh](#refresh)
        - [Refresh Request](#refresh-request)
        - [Refresh Response](#refresh-response)

## Auth

To use the application, the user should first register. 
When registering, a **User** will be created and an _Access Token_ will be returned as a result.
Alternatively, if the user already exists, use their credentials to login and get a new token.
The developer can test 
whether the generated token worked accordingly by grabbing the token hash and decode it on [jwt.io](https://jwt.io).
See the example below:

![Decoded Jwt Token](../images/api/auth-jwt-token.png)

Also, the token is short-lived; therefore, when it expires, it can be refreshed using the _Refresh Token_, 
which has a long lifetime.

### Register

Anyone can register.

```js
POST {{host}}/api/auth/register
```

#### Register Request

Sends data needed to create the user in body.

```json
{
  "firstName": "Firsty",
  "lastName": "Lasty",
  "email": "FirstyLasty@example.com",
  "password": "password123",
  "role": "Software Developer",
  "dateOfBirth": "2000-12-21"
}
```

#### Register Response

Returns an access token and a refresh token.

```js
200 OK
```

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJjNzQyM2NkMS0wNGY1LTQ5ZjAtOWY1OS0yOGVjNDk1YjllNzAiLCJlbWFpbCI6IkZpcnN0TGFzdHlAZXhhbXBsZS5jb20iLCJnaXZlbl9uYW1lIjoiRmlyc3R5IiwiZmFtaWx5X25hbWUiOiJMYXN0eSIsImp0aSI6ImQzZWJlOTExLWFmNDktNDE3OC1iZmVmLWEwODljMDg5ZGI5ZSIsImV4cCI6MTcwMTIwMjQyNSwiaXNzIjoiQ29uc2lsaXVtVGVtcHVzIiwiYXVkIjoiQ29uc2lsaXVtVGVtcHVzIn0.KqiE-eQqyd-VtfMJB3x9dNW2wR6_TrkGpGZ-ZyxxPWI",
  "refreshToken": "88882448-bd63-4731-8a05-f6333b6d22e2"
}
```


### Login

Anyone can login.

```js
POST {{host}}/api/auth/login
```

#### Login Request

Sends credentials of user in body.

```json
{
  "email": "FirstLasty@example.com",
  "password": "password123"
}
```

#### Login Response

Returns an access token and a refresh token.

```js
200 OK
```

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJjNzQyM2NkMS0wNGY1LTQ5ZjAtOWY1OS0yOGVjNDk1YjllNzAiLCJlbWFpbCI6IkZpcnN0TGFzdHlAZXhhbXBsZS5jb20iLCJnaXZlbl9uYW1lIjoiRmlyc3R5IiwiZmFtaWx5X25hbWUiOiJMYXN0eSIsImp0aSI6ImQzZWJlOTExLWFmNDktNDE3OC1iZmVmLWEwODljMDg5ZGI5ZSIsImV4cCI6MTcwMTIwMjQyNSwiaXNzIjoiQ29uc2lsaXVtVGVtcHVzIiwiYXVkIjoiQ29uc2lsaXVtVGVtcHVzIn0.KqiE-eQqyd-VtfMJB3x9dNW2wR6_TrkGpGZ-ZyxxPWI",
  "refreshToken": "88882448-bd63-4731-8a05-f6333b6d22e2"
}
```


### Refresh

Anyone can refresh their token.

```js
PUT {{host}}/api/auth/refresh
```

#### Refresh Request

Sends access and refresh tokens in body.

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJjNzQyM2NkMS0wNGY1LTQ5ZjAtOWY1OS0yOGVjNDk1YjllNzAiLCJlbWFpbCI6IkZpcnN0TGFzdHlAZXhhbXBsZS5jb20iLCJnaXZlbl9uYW1lIjoiRmlyc3R5IiwiZmFtaWx5X25hbWUiOiJMYXN0eSIsImp0aSI6ImQzZWJlOTExLWFmNDktNDE3OC1iZmVmLWEwODljMDg5ZGI5ZSIsImV4cCI6MTcwMTIwMjQyNSwiaXNzIjoiQ29uc2lsaXVtVGVtcHVzIiwiYXVkIjoiQ29uc2lsaXVtVGVtcHVzIn0.KqiE-eQqyd-VtfMJB3x9dNW2wR6_TrkGpGZ-ZyxxPWI",
  "refreshToken": "88882448-bd63-4731-8a05-f6333b6d22e2"
}
```

#### Refresh Response

Returns new access token.

```js
200 OK
```

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJjNzQyM2NkMS0wNGY1LTQ5ZjAtOWY1OS0yOGVjNDk1YjllNzAiLCJlbWFpbCI6IkZpcnN0TGFzdHlAZXhhbXBsZS5jb20iLCJnaXZlbl9uYW1lIjoiRmlyc3R5IiwiZmFtaWx5X25hbWUiOiJMYXN0eSIsImp0aSI6ImQzZWJlOTExLWFmNDktNDE3OC1iZmVmLWEwODljMDg5ZGI5ZSIsImV4cCI6MTcwMTIwMjQyNSwiaXNzIjoiQ29uc2lsaXVtVGVtcHVzIiwiYXVkIjoiQ29uc2lsaXVtVGVtcHVzIn0.KqiE-eQqyd-VtfMJB3x9dNW2wR6_TrkGpGZ-ZyxxPWI"
}
```