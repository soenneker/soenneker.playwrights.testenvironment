using System;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.Playwrights.Session;
using Soenneker.Playwrights.TestEnvironment.Options;

namespace Soenneker.Playwrights.TestEnvironment.Abstract;

/// <summary>
/// Defines the playwright test environment contract.
/// </summary>
public interface IPlaywrightTestEnvironment : IAsyncDisposable
{
    /// <summary>
    /// Gets base url.
    /// </summary>
    string BaseUrl { get; }

    /// <summary>
    /// Initializes the Playwright Test Environment so it is ready for use.
    /// </summary>
    /// <param name="projectPath">Path of the project to use.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the Playwright Test Environment is ready for use.</returns>
    ValueTask Initialize(string projectPath, CancellationToken cancellationToken);

    /// <summary>
    /// Creates session.
    /// </summary>
    /// <param name="sessionOptions">Session Options for the create session operation.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task whose result is the requested browser Session.</returns>
    ValueTask<BrowserSession> CreateSession(PlaywrightSessionOptions? sessionOptions = null, CancellationToken cancellationToken = default);
}
