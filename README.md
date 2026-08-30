[![](https://img.shields.io/nuget/v/soenneker.playwrights.testenvironment.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.playwrights.testenvironment/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.playwrights.testenvironment/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.playwrights.testenvironment/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.playwrights.testenvironment.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.playwrights.testenvironment/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.playwrights.testenvironment/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.playwrights.testenvironment/actions/workflows/codeql.yml)

# Soenneker.Playwrights.TestEnvironment

Runs an ASP.NET Core project on an available loopback port and provides headless Playwright sessions for integration tests.

## Installation

```bash
dotnet add package Soenneker.Playwrights.TestEnvironment
```

## Registration

Register the host options and environment in the test service collection:

```csharp
using Microsoft.Extensions.DependencyInjection;
using Soenneker.Playwrights.TestEnvironment.Options;
using Soenneker.Playwrights.TestEnvironment.Registrars;

services.AddSingleton(new PlaywrightTestHostOptions
{
    SolutionFileName = "MyApp.slnx",
    ProjectRelativePath = "src/MyApp/MyApp.csproj",
    ApplicationName = "MyApp",
    Restore = true,
    Build = true,
    BuildConfiguration = "Release"
});

services.AddPlaywrightTestEnvironmentAsSingleton();
```

Use the singleton registration for a test host shared by the suite. `AddPlaywrightTestEnvironmentAsScoped()` gives each scope its own mutable host runtime, while the Playwright installer and HTTP transport remain process-wide singletons.

## Start the application and create a session

```csharp
using Soenneker.Playwrights.TestEnvironment.Abstract;

IPlaywrightTestEnvironment environment =
    serviceProvider.GetRequiredService<IPlaywrightTestEnvironment>();

await environment.Initialize(
    @"C:\git\MyApp\src\MyApp\MyApp.csproj",
    cancellationToken);

await using var session = await environment.CreateSession(cancellationToken: cancellationToken);

await session.Page.GotoAsync("/");
await Assertions.Expect(session.Page.GetByRole(AriaRole.Heading))
                .ToBeVisibleAsync();
```

`Initialize` installs the configured Playwright browser if needed, starts Chromium, launches the project at `BaseUrl`, and waits until the application accepts HTTP requests. Dispose the environment to close Playwright and terminate the application process.

## Session reuse

Sessions get their own context and page unless reuse is enabled:

```csharp
services.AddSingleton(new PlaywrightTestHostOptions
{
    SolutionFileName = "MyApp.slnx",
    ProjectRelativePath = "src/MyApp/MyApp.csproj",
    ReuseBrowserContextAcrossSessions = true,
    ReusePageAcrossSessions = false
});
```

Per-session overrides take precedence over the host defaults:

```csharp
await using var session = await environment.CreateSession(
    new PlaywrightSessionOptions
    {
        ReuseBrowserContextAcrossSessions = true,
        ReusePageAcrossSessions = false
    },
    cancellationToken);
```

Reusing a page implies reusing its context. A session does not dispose shared pages or contexts; the environment owns and releases them.
