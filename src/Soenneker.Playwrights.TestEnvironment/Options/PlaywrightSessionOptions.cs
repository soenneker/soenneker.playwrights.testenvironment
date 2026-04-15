namespace Soenneker.Playwrights.TestEnvironment.Options;

public sealed class PlaywrightSessionOptions
{
    public bool? ReuseBrowserContextAcrossSessions { get; init; }

    public bool? ReusePageAcrossSessions { get; init; }
}
