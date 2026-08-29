[![](https://img.shields.io/nuget/v/soenneker.playwrights.testenvironment.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.playwrights.testenvironment/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.playwrights.testenvironment/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.playwrights.testenvironment/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.playwrights.testenvironment.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.playwrights.testenvironment/)

# Soenneker.Playwrights.TestEnvironment

Defines the playwright test environment contract.

## Install

```bash
dotnet add package Soenneker.Playwrights.TestEnvironment
```

## Quick start

```csharp
using Soenneker.Playwrights.TestEnvironment.Registrars;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
var result = services.AddPlaywrightTestEnvironmentAsSingleton();
```

Registers Playwright Test Environment with a singleton lifetime.

## What you get

- `IPlaywrightTestEnvironment` — Defines the playwright test environment contract.
- `PlaywrightTestEnvironmentRegistrar` — A utility library for configuration related operations.
- `PlaywrightSessionOptions` — Represents the playwright session options.
- `PlaywrightTestHostOptions` — Represents the playwright test host options.
- `PlaywrightTestHostRuntime` — Represents the playwright test host runtime.

## API at a glance

| API | What it does | Result / important behavior |
| --- | --- | --- |
| `IPlaywrightTestEnvironment.BaseUrl` | Gets base url. | Gets base url. |
| `IPlaywrightTestEnvironment.Initialize(projectPath, cancellationToken)` | Initializes the Playwright Test Environment so it is ready for use. | A task that completes when the Playwright Test Environment is ready for use. |
| `PlaywrightTestEnvironmentRegistrar.AddPlaywrightTestEnvironmentAsSingleton(services)` | Registers Playwright Test Environment with a singleton lifetime. | The same service collection, so additional registrations can be chained. |
| `PlaywrightTestEnvironmentRegistrar.AddPlaywrightTestEnvironmentAsScoped(services)` | Registers Playwright Test Environment with a scoped lifetime. | The same service collection, so additional registrations can be chained. |
| `PlaywrightSessionOptions.ReuseBrowserContextAcrossSessions` | Gets or sets a value indicating whether reuse browser context across sessions. | Gets or sets a value indicating whether reuse browser context across sessions. |
| `PlaywrightSessionOptions.ReusePageAcrossSessions` | Gets or sets a value indicating whether reuse page across sessions. | Gets or sets a value indicating whether reuse page across sessions. |
| `PlaywrightTestHostOptions.SolutionFileName` | Gets or sets solution file name. | Gets or sets solution file name. |
| `PlaywrightTestHostOptions.ProjectRelativePath` | Gets or sets project relative path. | Gets or sets project relative path. |
| `PlaywrightTestHostOptions.ApplicationName` | Gets or sets application name. | Gets or sets application name. |
| `PlaywrightTestHostOptions.Restore` | Gets or sets a value indicating whether restore. | Gets or sets a value indicating whether restore. |
| `PlaywrightTestHostOptions.Build` | Gets or sets a value indicating whether build. | Gets or sets a value indicating whether build. |
| `PlaywrightTestHostOptions.BuildConfiguration` | Gets or sets build configuration. | Gets or sets build configuration. |
| `PlaywrightTestHostOptions.ReuseBrowserContextAcrossSessions` | Gets or sets a value indicating whether reuse browser context across sessions. | Gets or sets a value indicating whether reuse browser context across sessions. |
| `PlaywrightTestHostOptions.ReusePageAcrossSessions` | Gets or sets a value indicating whether reuse page across sessions. | Gets or sets a value indicating whether reuse page across sessions. |
| `PlaywrightTestHostRuntime.BaseUrl` | Gets or sets base url. | Gets or sets base url. |
| `PlaywrightTestHostRuntime.Playwright` | Gets or sets playwright. | Gets or sets playwright. |
| `PlaywrightTestHostRuntime.Browser` | Gets or sets browser. | Gets or sets browser. |
| `PlaywrightTestHostRuntime.SharedContext` | Gets or sets shared context. | Gets or sets shared context. |

## Practical notes

- Cancellation stops pending work; it does not undo work that has already completed.
- Dispose instances you own when their scope ends so held resources can be released.
