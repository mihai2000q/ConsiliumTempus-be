# Consilium Tempus Backend Git

## Git Issues

Usually, whenever the code has to be modified an issue will be created on **Github**. The title of the issue will be the developer's choice, however, the labels attached to it should be significant. The expected labels to use are:
- **feature** - for new features that are added to the application
- **enhancement** - when the proposed issue makes the code more performant (or in any way considered better)
- **refactor** - when the code is just being cleaned up
- **bug** - if the issue is supposed to solve a known problem
- **documentation** - if the source code is not the target, but its documentation
- **test** - if a unit or integration test for the source code is being written


## Git Branches

Based on the above label the branch will have similar prefixes:
- **feature** for a _feature_ issue
- **enhance** for an _enhancement_ issuee
- **refactor** for a _refactor_ issue
- **bug** for a _bug_ issue
- **doc** for a _documentation_ issue
- **test** for a _test_ issue

For example, if your ticket's name is "Infrastructure - New Repository for Menus" and it is a feature issue, then the branch name should look something like this: `feature/139-infrastructure-...`.

Also, the main development branch is **develop**, and **master** is used for release purposes.

## Git Commits

The commits on branches should follow a structure. First, should be a hashtag followed by the number of the issue (e.g., #3) followed by the Layer's name that is the target of the commit (e.g., Infrastructure) followed by a dash and the changes done on it. For example, `#3 Infrastructure - Add New Repository for Menu`.

Note: if the target of the commit has nothing to do with any layer, no worries, just try to be as concise as possible. However, if multiple layers are affected, just submit more small commits.