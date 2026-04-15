using System;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.Playwrights.Session;
using Soenneker.Playwrights.TestEnvironment;
using Soenneker.Playwrights.TestEnvironment.Options;

namespace Soenneker.Playwrights.TestEnvironment.Abstract;

public interface IPlaywrightTestEnvironment : IAsyncDisposable
{
    string BaseUrl { get; }

    ValueTask Initialize(string projectPath, CancellationToken cancellationToken);

    ValueTask<BrowserSession> CreateSession(PlaywrightSessionOptions? sessionOptions = null);
}