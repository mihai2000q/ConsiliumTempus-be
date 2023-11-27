# Consilium Tempus Backend API

- [Api Controller](#api-controller)
- [Authentication](#authentication)

In order to use the application you will need a JWT token.

## Api Controller

Inside the code you will find an Api Controller **class**, under the _Controllers_ package. This class implements basic functionality for all the controllers that are and will be created in the project. It is intended to be extended by all the controllers (Template Design Pattern).

The template Api Controller resolves the following:
- it sets the REST Api route to `/api/{{controller}}` to sub controllers, where **controller** is the name of the controller (i.e., for a Food Controller the route would be `{{host}}/api/food`)
- it adds a layer of authorization before accessing the controller (if the user is not authorized, the controller will return a status code 405)
- it injects the mapper and the mediator
- it contains a solution to returning validation problems, conflicts, etc.

## Authentication

For more information go to [Auth](api/Api.Auth.md).