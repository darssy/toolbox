# Toolbox

[![Build status](https://ci.appveyor.com/api/projects/status/73usvufg8c3a0csq/branch/master?svg=true&passingText=master%20-%20OK&failingText=master%20-%20FAILED&pendingText=master%20-%20PENDING)](https://ci.appveyor.com/project/adamstyl/toolbox/branch/master)
[![Build status](https://ci.appveyor.com/api/projects/status/73usvufg8c3a0csq/branch/develop?svg=true&passingText=dev%20-%20OK&failingText=dev%20-%20FAILED&pendingText=dev%20-%20PENDING)](https://ci.appveyor.com/project/adamstyl/toolbox/branch/develop)

## About
A collection of classes methods and utilities that seem useful and quite common to me. The code is pretty simple and most of the times what you need to know is in documentation, which admittedly is not complete yet.

Currently the library's public API is **NOT** stable. It might change as I evolve the projects using it. That said and since the project is for self-learning, I am _trying_ to maintain backwards compatibility, pretend that there are other people besides me using it, and undergo the whole 9 yards of maintaining a shared library. As the versioning is not 100% compatible with semver yet, messages of commits with breaking changes will be prefixed with `BREAKING`.

Most of the code comes from elements of [DARSSY](http://darssy.com) and thus this code is used in a real world application. Now where I come from we use to say "one equals none" but I say one is better than none. Anyway, once I decide which parts of the original code I will open source, I will release a stable 1.0 version, hopefully with some unit tests as well. Currently the test coverage is at a humble 25%.

## Contents
Like with a real toolbox, you need to know its contents before you open it, before even taking it onboard. Either an amateur handyman looking at the toolbox and wondering "what we have here" or a professional one who knows with eyes closed what's where, the result is the same. You need to know your toolbox's contents. Let's go then:

_(Attention: work in progress)_

### Extensions

Extension methods can be a powerful tool (LINQ anyone?) and there are some framework types that could benefit from extension methods. For example I find it more practical to get an absolute value from the subtraction of 2 signed numbers like this `(a - b).Abs()` instead of `Math.Abs(a - b)`. The former is more _fluent_.

It's worth noting that most of the "wrapper" extensions (like `Abs` `Round` etc) are `AggressiveInline`d and some early benchmarks show that inlining makes a difference. Not all methods have the inline directive at them but the goal is to add it everywhere there is a benefit from it. For example .NET Framework 4.8 fails to inline methods that are calling a delegate but in .NET 7 that's not the case.

Bellow you can find a list of the most important extension methods grouped per category.

#### Collections

`static T Last<T>(this IList<T> list)`<br/>
Retrieves the last element of a list. That is if you can't use `list[^1]`

`LinkedListNode<T> GetNode<T>(this LinkedList<T> list, Func<T, bool> condition)`<br/>
Retrieves an element that fulfills the provided condition

`V GetOrCreate<K, V>(this IDictionary<K, V> dictionary, ...)` overloads<br/>
Tired of the "if exists then get it otherwise create it" pattern? So was I, hence the `GetOrCreate` extension. Example:
```csharp
SolidBrush solidBrush = BrushCache.GetOrCreate(e.Item.BackColor, color => new SolidBrush(color));
e.Graphics.FillRectangle(solidBrush, new Rectangle(Point.Empty, e.Item.Size));
```
or if you don't care about the closure allocation:

```csharp
SolidBrush solidBrush = BrushCache.GetOrCreate(e.Item.BackColor, () => new SolidBrush(e.Item.BackColor));
e.Graphics.FillRectangle(solidBrush, new Rectangle(Point.Empty, e.Item.Size));
```

### Logging
You can use `EventLogger` static class logger wrapper if you want to wrap a logging library inside an `ILogWrapper` and then switch implementation if you change your mind. You can skip `EventLogger` altogether and go straight to `ILogWrapper` if you are hard-core into DI, but I consider it an overkill. Whenever I want to log, **I want to log.** Not to think how the heck I could get the logger in XYZ class.

You can use `ConsoleLogWrapper` which logs in the console; it is suggested only in early testing cases as most sophisticated logging libraries have a gazillion of sinks, among others a Console sink.

`UnitTestLogger` can help you assert that some logs were created during a run. Useful when you want to ensure that the test executed a part of the code that you can't easily "reach" otherwise. 

Finally, `LogEntry` struct can be used to hold log creation time, severity, message and optionally the exception

### Railway Oriented Programming (ROP)
Well not exactly ROP as ROP is a more complex concept than just a `Result` type, but the `Result` and its variants are inspired by the ROP concept. Use a `Result` return type when a method failing is not an _exceptional_ case and as a result throwing an exception is an overkill. Instead of using an `out` argument which might not be elegant or even practical you can return a `Result<ActualResultType, string>` where the second generic argument is the text describing the possible error.

It's worth to note that `Result` and its variants are not "just tuples" but they also contain properties (like `Success` for example to check the status) and implicit cast operators that help reduce boilerplate.
