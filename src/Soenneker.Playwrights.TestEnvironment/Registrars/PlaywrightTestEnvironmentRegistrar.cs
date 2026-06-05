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
    /// Adds playwright test environment as singleton.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The result of the operation.</returns>
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
    /// Adds playwright test environment as scoped.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The result of the operation.</returns>
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