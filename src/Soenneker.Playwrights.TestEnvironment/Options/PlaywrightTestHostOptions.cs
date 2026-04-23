namespace Soenneker.Playwrights.TestEnvironment.Options;

public sealed class PlaywrightTestHostOptions
{
    public required string SolutionFileName { get; init; }

    public required string ProjectRelativePath { get; init; }

    public string ApplicationName { get; init; } = "application";

    public bool Restore { get; init; } = true;

    public bool Build { get; init; } = true;

    public string BuildConfiguration { get; init; } = "Release";

    public bool ReuseBrowserContextAcrossSessions { get; init; }

    public bool ReusePageAcrossSessions { get; init; }
}
