# Consilium Tempus Backend Continuous Integration

* [Workflows](#workflows)
  * [Build](#build)
  * [Testing](#testing)
* [Secrets](#secrets)
* [Release and Deployment](#release-and-deployment)

The continuous integration is done with **GitHub Actions**, and the workflows are defined inside `.github/workflows`.

## Workflows

Both workflows run on each push and pull request to the **develop** branch.

### Build

The **Build** workflow (`build.yml`) checks that the application builds.
It uses the `build-action`, which installs the .NET SDK (10.0.x), restores the dependencies and builds the solution.

### Testing

The **Testing** workflow (`testing.yml`) runs the tests, each kind in its own job
(see [Testing](Testing.md) for more about them):

| Job                  | What it does                                                                             | Timeout |
|----------------------|------------------------------------------------------------------------------------------|--------:|
| Architecture Testing | builds the solution and runs the architecture tests                                      |  5 min  |
| Unit Testing         | builds the solution and runs the unit tests (it retries up to 3 times if they fail)      | 10 min  |
| Integration Testing  | builds the solution and runs the integration tests, that use a database container        | 10 min  |
| End To End Testing   | starts the application and the database, applies the migrations and runs Playwright      | 15 min  |

For the end-to-end job, the `docker-action` prepares the environment:
it writes the secret key of the Jwt Settings inside the `appsettings.json`, copies `.env.dev` to `.env`,
starts the containers with Docker Compose and applies the migrations to the database.
After that, the job installs the Node dependencies and the Playwright browsers, and runs the tests.

## Secrets

| Secret                 | Used for                                                                    |
|------------------------|-----------------------------------------------------------------------------|
| `SUPER_SECRET_JWT_KEY` | the secret key of the Jwt Settings, for the application of the e2e job      |

## Release and Deployment

The releases are done through the **master** branch: once enough features are ready,
**develop** gets merged into **master** (see [Git](Git.md)).

There is no automated deployment, as the application is deployed locally
(see [Docker Containers](../README.md#docker-containers)).
