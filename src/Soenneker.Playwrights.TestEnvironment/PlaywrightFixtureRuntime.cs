using System.Diagnostics;
using Microsoft.Playwright;

namespace Soenneker.Playwrights.TestEnvironment;

public sealed class PlaywrightFixtureRuntime
{
    public string BaseUrl { get; set; } = null!;

    public IPlaywright? Playwright { get; set; }

    public IBrowser? Browser { get; set; }

    public Process? DemoProcess { get; set; }
}