namespace Soenneker.Playwrights.TestEnvironment.Options;

/// <summary>
/// Overrides page and browser-context reuse for one test session.
/// </summary>
public sealed class PlaywrightSessionOptions
{
    /// <summary>
    /// Reuses the environment's shared browser context when <c>true</c>. A null value uses the host default.
    /// </summary>
    public bool? ReuseBrowserContextAcrossSessions { get; init; }

    /// <summary>
    /// Reuses the environment's shared page when <c>true</c>. This also implies context reuse. A null value uses the host default.
    /// </summary>
    public bool? ReusePageAcrossSessions { get; init; }
}
