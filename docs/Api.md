# Consilium Tempus Backend API

- [Api Controller](#api-controller)
- [Authentication](#authentication)

## Api Controller

Inside the code you will find an Api Controller **class**, under the _Controllers_ package. This class implements basic functionality for all the controllers that are and will be created in the project. It is intended to be extended by all the controllers (Template Design Pattern).

The template Api Controller resolves the following:
- it sets the REST Api route to ``/api/``{{controller}} to sub controllers (i.e., for a Food Controller the prefix would be {{host}}``/api``/food)
- it adds a layer of authorization before accessing the controller
- it injects the mapper and the mediator
- it contains a solution to returning validation problems, conflicts, etc.

## Authentication

In order to use the application the you will need a JWT token. <br>
For more information go to [Auth](api/Api.Auth.md)