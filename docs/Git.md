# Consilium Tempus Backend Git

## Git Issues

Usually, whenever the code has to be modified, an issue will be created on **GitHub**. 
The title of the issue will be the developer's choice 
(for preference, it should resemble the git commits as a higher level overview without, of course, the issue number), 
however, the labels attached to it should be significant.
The expected labels to use are:
- **build** if the issue is supposed to update the dependencies (or the build, docker and CI setup)
- **bug** if the issue is supposed to solve a known problem
- **documentation** if the source code is not the target, but its documentation
- **feature** for new features that are added to the application
- **improvement** when the proposed issue makes the code more performant (or in any way considered better)
- **refactor** when the code is just being cleaned up
- **test** if a unit, integration, architecture or e2e test for the source code is being written

## Git Branches

Based on the above label, the branch will have similar prefixes:
- **build** for a _build_ issue
- **bug** for a _bug_ issue
- **doc** for a _documentation_ issue
- **feature** for a _feature_ issue
- **improve** for an _improvement_ issue
- **refactor** for a _refactor_ issue
- **test** for a _test_ issue

For example, if your ticket's name is "Infrastructure - New Repository for Menus" 
and it is a feature issue, then the branch name should look something like this: `feature/139-infrastructure-...`.

Also, the main development branch is **develop**, and **master** is used for release.

## Git Commits

The commits on branches should follow a structure (*#ISSUE LAYER - CHANGES*).
First, there should be a hashtag followed by the number of the issue (e.g., #3)
followed by the Layer's name that is the target of the commit
(e.g., Infrastructure) 
followed by a dash and the changes done on it 
(by preference, try to include a meaningful verb like add or update).
For example, `#3 Infrastructure - Add New Repository for Menu`.

Note: if the target of the commit has nothing to do with any layer, no worries, just try to be as concise as possible.
However, if multiple layers are affected, submit more small commits.

## Git Workflow Example

To put everything together, this is how the work on a new feature would look like:

1. Create the issue on GitHub, i.e., *Infrastructure - New Repository for Menus*, with the **feature** label
   (the issue gets the number #139).
2. Create the branch for it, from **develop**: `feature/139-infrastructure-new-repository-for-menus`.
3. Commit the work in small steps, following the structure: `#139 Infrastructure - Add New Repository for Menu`.
4. Open a pull request into **develop** and merge it (the merge shows up as *Merge pull request #... from ...*).
5. Once enough features are ready, **develop** gets released into **master** (see [Continuous Integration](CI.md#release-and-deployment)).
