# How to contribute

We love getting contributions (e.g.: patches, feature requests, new features...) to MementoFX from the community. Here are a few guidelines that we
need all contributors to adhere to so that we can have a chance of keeping on top of the project.

## Getting Started

* Make sure you have a [GitHub account](https://github.com/signup/free)
* [Create a new issue](https://github.com/MementoFX/MementoFX/issues/new), if one does not already exist, and clearly describe it. If it is a bug:
  * Include steps to reproduce it.
  * Make sure you tell us what version you have encountered this bug on.
* Fork the repository on GitHub

## Making Changes

* Create a feature branch from where you want to base your work.
  * This is usually the develop branch since we never do any work off our master branch. The master is always our latest stable release (i.e.: the one from which the latest NuGet package was created)
  * Only target release branches if you are certain your fix must be on that
    branch.
  * To quickly create a feature branch based on develop; `git branch
    fix/develop/my_contribution` then checkout the new branch with `git
    checkout fix/develop/my_contribution`.  Please avoid working directly on the
    `develop` branch.
* Make commits of logical units.
* Check for unnecessary whitespaces with `git diff --check` before committing.
* Make sure your commit messages are in the proper format.
* Make sure you have added the necessary tests for your changes.

## Guidelines

Avoid introducing breaking changes of any sort.


## Submitting Changes

* Sign the [Contributor License Agreement](http://www.mastreeno.com/License/ContributorsAgreementConsent).
* Push your changes to a feature branch in your fork of the repository.
* Submit a pull request to the MementoFX repository

# Additional Resources

* [General GitHub documentation](http://help.github.com/)
* [GitHub pull request documentation](http://help.github.com/send-pull-requests/)