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
    /// Executes the initialize operation.
    /// </summary>
    /// <param name="projectPath">The project path.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    ValueTask Initialize(string projectPath, CancellationToken cancellationToken);

    /// <summary>
    /// Creates session.
    /// </summary>
    /// <param name="sessionOptions">The session options.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task containing the result of the operation.</returns>
    ValueTask<BrowserSession> CreateSession(PlaywrightSessionOptions? sessionOptions = null, CancellationToken cancellationToken = default);
}