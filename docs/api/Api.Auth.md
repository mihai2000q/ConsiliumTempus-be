# Consilium Tempus API

- [Auth](#auth)
    - [Register](#register)
        - [Register Request](#register-request)
        - [Register Response](#register-response)
    - [Login](#login)
        - [Login Request](#login-request)
        - [Login Response](#login-response)

## Auth

To use the application, the user should first register. 
When registering, a **User** will be created. 
After that, the user can login, where a token will be created. 
This token holds sensitive data. 
The developer can test 
whether the token generator worked accordingly by grabbing the token hash and decode it on [jwt.io](https://jwt.io). 
See the example below:

![Decoded Jwt Token](../images/api/auth-jwt-token.png)

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
  "email": "FirstLasty@example.com",
  "password": "password123",
  "role": "Software Developer",
  "dateOfBirth": "2000-12-21"
}
```

#### Register Response

Returns a token about the newly created user.

```js
200 OK
```

```json
{
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJjNzQyM2NkMS0wNGY1LTQ5ZjAtOWY1OS0yOGVjNDk1YjllNzAiLCJlbWFpbCI6IkZpcnN0TGFzdHlAZXhhbXBsZS5jb20iLCJnaXZlbl9uYW1lIjoiRmlyc3R5IiwiZmFtaWx5X25hbWUiOiJMYXN0eSIsImp0aSI6ImQzZWJlOTExLWFmNDktNDE3OC1iZmVmLWEwODljMDg5ZGI5ZSIsImV4cCI6MTcwMTIwMjQyNSwiaXNzIjoiQ29uc2lsaXVtVGVtcHVzIiwiYXVkIjoiQ29uc2lsaXVtVGVtcHVzIn0.KqiE-eQqyd-VtfMJB3x9dNW2wR6_TrkGpGZ-ZyxxPWI"
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

Returns token based on the user information.

```js
200 OK
```

```json
{
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJjNzQyM2NkMS0wNGY1LTQ5ZjAtOWY1OS0yOGVjNDk1YjllNzAiLCJlbWFpbCI6IkZpcnN0TGFzdHlAZXhhbXBsZS5jb20iLCJnaXZlbl9uYW1lIjoiRmlyc3R5IiwiZmFtaWx5X25hbWUiOiJMYXN0eSIsImp0aSI6ImQzZWJlOTExLWFmNDktNDE3OC1iZmVmLWEwODljMDg5ZGI5ZSIsImV4cCI6MTcwMTIwMjQyNSwiaXNzIjoiQ29uc2lsaXVtVGVtcHVzIiwiYXVkIjoiQ29uc2lsaXVtVGVtcHVzIn0.KqiE-eQqyd-VtfMJB3x9dNW2wR6_TrkGpGZ-ZyxxPWI"
}
```