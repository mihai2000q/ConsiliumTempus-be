# Consilium Tempus API

- [Auth](#auth)
    - [Register](#register)
        - [Register Request](#register-request)
        - [Register Response](#register-response)
    - [Login](#register)
        - [Login Request](#register-request)
        - [Login Response](#register-response)

## Auth

In order to use the application the user should first register. When registering a JWT Token will be returned. This token holds sensitive data. The developer can test whether the token generator worked accordingly by grabbing the token hash and decode it on [jwt.io](https://jwt.io). See example below:

### Register

```js
POST {{host}}/auth/register
```

#### Register Request

```json
{
    "firstName": "Firsty",
    "lastName": "Lasty",
    "email": "FirstLasty@example.com",
    "password": "password123"
}
```

#### Register Response

```js
200 OK
```

```json
{
    "token": ""
}
```

### Login

```js
POST {{host}}/auth/login
```

#### Login Request

```json
{
    
}
```

#### Login Response

```js
200 OK
```

```json
{
    
}
```