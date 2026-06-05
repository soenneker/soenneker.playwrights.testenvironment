namespace Soenneker.Playwrights.TestEnvironment.Options;

/// <summary>
/// Represents the playwright test host options.
/// </summary>
public sealed class PlaywrightTestHostOptions
{
    /// <summary>
    /// Gets or sets solution file name.
    /// </summary>
    public required string SolutionFileName { get; init; }

    /// <summary>
    /// Gets or sets project relative path.
    /// </summary>
    public required string ProjectRelativePath { get; init; }

    /// <summary>
    /// Gets or sets application name.
    /// </summary>
    public string ApplicationName { get; init; } = "application";

    /// <summary>
    /// Gets or sets a value indicating whether restore.
    /// </summary>
    public bool Restore { get; init; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether build.
    /// </summary>
    public bool Build { get; init; } = true;

    /// <summary>
    /// Gets or sets build configuration.
    /// </summary>
    public string BuildConfiguration { get; init; } = "Release";

    /// <summary>
    /// Gets or sets a value indicating whether reuse browser context across sessions.
    /// </summary>
    public bool ReuseBrowserContextAcrossSessions { get; init; }

    /// <summary>
    /// Gets or sets a value indicating whether reuse page across sessions.
    /// </summary>
    public bool ReusePageAcrossSessions { get; init; }
}
