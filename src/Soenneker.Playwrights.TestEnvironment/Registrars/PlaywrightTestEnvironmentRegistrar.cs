using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Playwrights.Installation.Registrars;
using Soenneker.Playwrights.TestEnvironment.Abstract;
using Soenneker.Utils.Dotnet.Registrars;
using Soenneker.Utils.File.Registrars;
using Soenneker.Utils.HttpClientCache.Registrar;
using Soenneker.Utils.Network.Registrars;

namespace Soenneker.Playwrights.TestEnvironment.Registrars;

/// <summary>
/// A utility library for configuration related operations
/// </summary>
public static class PlaywrightTestEnvironmentRegistrar
{
    /// <summary>
    /// Registers Playwright Test Environment with a singleton lifetime.
    /// </summary>
    /// <param name="services">Service collection that receives the registration.</param>
    /// <returns>The same service collection, so additional registrations can be chained.</returns>
    public static IServiceCollection AddPlaywrightTestEnvironmentAsSingleton(this IServiceCollection services)
    {
        services.AddNetworkUtilAsSingleton()
                .AddFileUtilAsSingleton()
                .AddDotnetUtilAsSingleton()
                .AddHttpClientCacheAsSingleton()
                .AddPlaywrightInstallationUtilAsSingleton();

        services.TryAddSingleton<PlaywrightTestHostRuntime>();
        services.TryAddSingleton<IPlaywrightTestEnvironment, PlaywrightTestEnvironment>();

        return services;
    }

    /// <summary>
    /// Registers Playwright Test Environment with a scoped lifetime.
    /// </summary>
    /// <param name="services">Service collection that receives the registration.</param>
    /// <returns>The same service collection, so additional registrations can be chained.</returns>
    public static IServiceCollection AddPlaywrightTestEnvironmentAsScoped(this IServiceCollection services)
    {
        services.AddNetworkUtilAsScoped()
                .AddFileUtilAsScoped()
                .AddDotnetUtilAsScoped()
                .AddHttpClientCacheAsSingleton()
                .AddPlaywrightInstallationUtilAsSingleton();

        services.TryAddSingleton<PlaywrightTestHostRuntime>();
        services.TryAddScoped<IPlaywrightTestEnvironment, PlaywrightTestEnvironment>();

        return services;
    }
}
