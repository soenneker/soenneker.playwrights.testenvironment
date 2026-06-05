namespace Soenneker.Playwrights.TestEnvironment.Options;

/// <summary>
/// Represents the playwright session options.
/// </summary>
public sealed class PlaywrightSessionOptions
{
    /// <summary>
    /// Gets or sets a value indicating whether reuse browser context across sessions.
    /// </summary>
    public bool? ReuseBrowserContextAcrossSessions { get; init; }

    /// <summary>
    /// Gets or sets a value indicating whether reuse page across sessions.
    /// </summary>
    public bool? ReusePageAcrossSessions { get; init; }
}
