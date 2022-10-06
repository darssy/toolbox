# Toolbox

[![Build status](https://ci.appveyor.com/api/projects/status/73usvufg8c3a0csq/branch/master?svg=true&passingText=master%20-%20OK&failingText=master%20-%20FAILED&pendingText=master%20-%20PENDING)](https://ci.appveyor.com/project/adamstyl/toolbox/branch/master)
[![Build status](https://ci.appveyor.com/api/projects/status/73usvufg8c3a0csq/branch/develop?svg=true&passingText=dev%20-%20OK&failingText=dev%20-%20FAILED&pendingText=dev%20-%20PENDING)](https://ci.appveyor.com/project/adamstyl/toolbox/branch/master)

## About
A collection of classes methods and utilities that seem useful and quite common to me. The code is pretty simple and most of the times what you need to know is in documentation, which admittedly is not complete yet.

Currently the library's public API is **NOT** stable. It might change as I evolve the projects using it. That said and since the project is for self-learning, I am _trying_ to maintain backwards compatibility, pretend that there are other people besides me using it, and undergo the whole 9 yards of maintaining a shared library. As the versioning is not 100% compatible with semver yet, messages of commits with breaking changes will be prefixed with `BREAKING`.

Most of the code comes from elements of [DARSSY](http://darssy.com) and thus this code is used in a real world application. Now where I come from we use to say "one equals none" but I say one is better than none. Anyway, once I decide which parts of the original code I will open source, I will release a stable 1.0 version, hopefully with some unit tests as well. Currently the test coverage is at a humble 25%.
