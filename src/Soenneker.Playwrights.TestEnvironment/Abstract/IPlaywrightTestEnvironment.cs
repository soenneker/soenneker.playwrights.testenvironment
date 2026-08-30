using System;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.Playwrights.Session;
using Soenneker.Playwrights.TestEnvironment.Options;

namespace Soenneker.Playwrights.TestEnvironment.Abstract;

/// <summary>
/// Starts an application and a headless Playwright browser for integration tests, then creates browser sessions against that application.
/// </summary>
public interface IPlaywrightTestEnvironment : IAsyncDisposable
{
    /// <summary>
    /// Gets the loopback URL assigned to the application after initialization.
    /// </summary>
    string BaseUrl { get; }

    /// <summary>
    /// Installs Playwright if necessary, launches Chromium, starts the application project, and waits for it to accept HTTP requests.
    /// </summary>
    /// <param name="projectPath">Path to the application project file.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the Playwright Test Environment is ready for use.</returns>
    ValueTask Initialize(string projectPath, CancellationToken cancellationToken);

    /// <summary>
    /// Creates a page and context according to the configured reuse policy.
    /// </summary>
    /// <param name="sessionOptions">Optional per-session overrides for page and context reuse.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A session whose disposal respects whether its page or context is shared.</returns>
    ValueTask<BrowserSession> CreateSession(PlaywrightSessionOptions? sessionOptions = null, CancellationToken cancellationToken = default);
}
