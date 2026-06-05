using System.Diagnostics;
using Microsoft.Playwright;

namespace Soenneker.Playwrights.TestEnvironment;

/// <summary>
/// Represents the playwright test host runtime.
/// </summary>
public sealed class PlaywrightTestHostRuntime
{
    /// <summary>
    /// Gets or sets base url.
    /// </summary>
    public string BaseUrl { get; set; } = null!;

    /// <summary>
    /// Gets or sets playwright.
    /// </summary>
    public IPlaywright? Playwright { get; set; }

    /// <summary>
    /// Gets or sets browser.
    /// </summary>
    public IBrowser? Browser { get; set; }

    /// <summary>
    /// Gets or sets shared context.
    /// </summary>
    public IBrowserContext? SharedContext { get; set; }

    /// <summary>
    /// Gets or sets shared page.
    /// </summary>
    public IPage? SharedPage { get; set; }

    /// <summary>
    /// Gets or sets demo process.
    /// </summary>
    public Process? DemoProcess { get; set; }
}