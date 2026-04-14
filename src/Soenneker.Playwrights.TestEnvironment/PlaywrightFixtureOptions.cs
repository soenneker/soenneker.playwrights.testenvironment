namespace Soenneker.Playwrights.TestEnvironment;

public sealed class PlaywrightFixtureOptions
{
    public required string SolutionFileName { get; init; }

    public required string ProjectRelativePath { get; init; }

    public string ApplicationName { get; init; } = "application";

    public string BuildConfiguration { get; init; } = "Release";
}
