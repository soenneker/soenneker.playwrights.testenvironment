using System;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.Playwrights.Session;

namespace Soenneker.Playwrights.TestEnvironment.Abstract;

public interface IPlaywrightTestEnvironment : IAsyncDisposable
{
    string BaseUrl { get; }

    ValueTask Initialize(string projectPath, CancellationToken cancellationToken);

    ValueTask<BrowserSession> CreateSession();
}