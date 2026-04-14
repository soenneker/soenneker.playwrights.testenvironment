using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;
using Soenneker.Extensions.String;
using Soenneker.Extensions.Task;
using Soenneker.Extensions.ValueTask;
using Soenneker.Playwrights.Installation.Abstract;
using Soenneker.Playwrights.Session;
using Soenneker.Playwrights.TestEnvironment.Abstract;
using Soenneker.Utils.Delay;
using Soenneker.Utils.Dotnet.Abstract;
using Soenneker.Utils.HttpClientCache.Abstract;
using Soenneker.Utils.Network.Abstract;

namespace Soenneker.Playwrights.TestEnvironment;

///<inheritdoc cref="IPlaywrightTestEnvironment"/>
public class PlaywrightTestEnvironment : IPlaywrightTestEnvironment
{
    private readonly IDotnetUtil _dotnetUtil;
    private readonly INetworkUtil _networkUtil;
    private readonly IHttpClientCache _httpClientCache;
    private readonly IPlaywrightInstallationUtil _playwrightInstallationUtil;
    private readonly PlaywrightFixtureOptions _options;
    private readonly PlaywrightFixtureRuntime _runtime;
    private readonly ILogger<PlaywrightTestEnvironment> _logger;

    public PlaywrightTestEnvironment(IDotnetUtil dotnetUtil, INetworkUtil networkUtil, IHttpClientCache httpClientCache,
        IPlaywrightInstallationUtil playwrightInstallationUtil, PlaywrightFixtureOptions options, PlaywrightFixtureRuntime runtime,
        ILogger<PlaywrightTestEnvironment> logger)
    {
        _dotnetUtil = dotnetUtil;
        _networkUtil = networkUtil;
        _httpClientCache = httpClientCache;
        _playwrightInstallationUtil = playwrightInstallationUtil;
        _options = options;
        _runtime = runtime;
        _logger = logger;
    }

    public string BaseUrl { get; private set; } = null!;

    public async ValueTask Initialize(string projectPath, CancellationToken cancellationToken)
    {
        _runtime.BaseUrl = $"http://127.0.0.1:{_networkUtil.GetFreePort()}/";
        BaseUrl = _runtime.BaseUrl;

        await _playwrightInstallationUtil.EnsureInstalled(cancellationToken)
                                         .NoSync();

        _runtime.Playwright = await Playwright.CreateAsync()
                                              .NoSync();

        _runtime.Browser = await LaunchBrowser()
            .NoSync();

        await StartProject(projectPath, cancellationToken)
            .NoSync();
    }

    public async ValueTask<BrowserSession> CreateSession()
    {
        if (_runtime.Browser is null)
            throw new InvalidOperationException("Browser has not been initialized.");

        IBrowserContext context = await _runtime.Browser.NewContextAsync(new BrowserNewContextOptions
                                                {
                                                    BaseURL = _runtime.BaseUrl
                                                })
                                                .NoSync();

        IPage page = await context.NewPageAsync()
                                  .NoSync();

        return new BrowserSession(context, page);
    }

    private async ValueTask<IBrowser> LaunchBrowser()
    {
        if (_runtime.Playwright is null)
            throw new InvalidOperationException("Playwright has not been initialized.");

        var options = new BrowserTypeLaunchOptions
        {
            Headless = true,
            Channel = "chromium"
        };

        return await _runtime.Playwright.Chromium.LaunchAsync(options)
                             .NoSync();
    }

    private async ValueTask StartProject(string projectPath, CancellationToken cancellationToken)
    {
        string trimmedBaseUrl = _runtime.BaseUrl.TrimEnd('/');

        _logger.LogInformation("Starting {ApplicationName} from {ProjectPath} on {BaseUrl}", _options.ApplicationName, projectPath, _runtime.BaseUrl);

        _runtime.DemoProcess = await _dotnetUtil.Start(projectPath, urls: trimmedBaseUrl, outputCallback: line => CaptureProjectOutput(line, isError: false),
                                                    errorCallback: line => CaptureProjectOutput(line, isError: true),
                                                    configuration: _options.BuildConfiguration, cancellationToken: cancellationToken)
                                                .NoSync();

        if (_runtime.DemoProcess is null)
            throw new InvalidOperationException($"Failed to start the '{_options.ApplicationName}' project '{projectPath}'.");

        await WaitForProjectReady(cancellationToken)
            .NoSync();
    }

    private async ValueTask WaitForProjectReady(CancellationToken cancellationToken)
    {
        HttpClient client = await _httpClientCache.Get(GetType().FullName ?? nameof(PlaywrightTestEnvironment),
            cancellationToken: cancellationToken).NoSync();

        Exception? lastException = null;

        int port = new Uri(_runtime.BaseUrl).Port;

        const int maxAttempts = 120;

        for (var attempt = 0; attempt < maxAttempts; attempt++)
        {
            _logger.LogInformation("Waiting for {ApplicationName} readiness, attempt {Attempt}/{MaxAttempts} at {BaseUrl}",
                _options.ApplicationName, attempt + 1, maxAttempts, _runtime.BaseUrl);

            if (_runtime.DemoProcess is not null && _runtime.DemoProcess.HasExited)
            {
                throw new InvalidOperationException(
                    $"{_options.ApplicationName} exited before becoming ready. Exit code: {_runtime.DemoProcess.ExitCode}{Environment.NewLine}");
            }

            try
            {
                // First, wait for the server socket to actually be listening.
                if (!_networkUtil.IsPortBusy(port))
                {
                    await DelayUtil.Delay(1000, _logger, cancellationToken).NoSync();
                    continue;
                }

                // Once the port is open, an HTTP response of any kind is enough to prove
                // Kestrel is alive and accepting requests. Do not require 200 OK.
                using var request = new HttpRequestMessage(HttpMethod.Get, _runtime.BaseUrl);

                using HttpResponseMessage response = await client.SendAsync(request,
                        HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                    .NoSync();

                _logger.LogInformation("{ApplicationName} is ready at {BaseUrl}. Status code: {StatusCode}",
                    _options.ApplicationName, _runtime.BaseUrl, (int)response.StatusCode);

                return;
            }
            catch (HttpRequestException ex)
            {
                lastException = ex;
            }
            catch (TaskCanceledException ex) when (!cancellationToken.IsCancellationRequested)
            {
                lastException = ex;
            }
            catch (Exception ex)
            {
                lastException = ex;
            }

            await DelayUtil.Delay(1000, _logger, cancellationToken).NoSync();
        }

        throw new TimeoutException(
            $"Timed out waiting for {_options.ApplicationName} at {_runtime.BaseUrl}.{Environment.NewLine}" +
            $"Last startup error: {lastException?.Message ?? "None"}{Environment.NewLine}");
    }

    private void CaptureProjectOutput(string line, bool isError)
    {
        if (line.IsNullOrWhiteSpace())
            return;

        string prefix = _options.ApplicationName.ToLowerInvariantFast()
                                .Replace(' ', '-');
        string formatted = isError ? $"[{prefix}:err] {line}" : $"[{prefix}] {line}";

        if (isError)
            _logger.LogWarning("{Output}", formatted);
        else
            _logger.LogInformation("{Output}", formatted);
    }

    public async ValueTask DisposeAsync()
    {
        Exception? exception = null;

        try
        {
            if (_runtime.Browser is not null)
                await _runtime.Browser.DisposeAsync()
                              .NoSync();
        }
        catch (Exception ex)
        {
            exception = ex;
        }

        try
        {
            _runtime.Playwright?.Dispose();
        }
        catch (Exception ex) when (exception is null)
        {
            exception = ex;
        }

        try
        {
            if (_runtime.DemoProcess is not null)
            {
                try
                {
                    if (!_runtime.DemoProcess.HasExited)
                        _runtime.DemoProcess.Kill(entireProcessTree: true);
                }
                catch (InvalidOperationException)
                {
                }

                _runtime.DemoProcess.Dispose();
                _runtime.DemoProcess = null;
            }
        }
        catch (Exception ex) when (exception is null)
        {
            exception = ex;
        }

        await _httpClientCache.Remove(GetType()
                                  .FullName ?? nameof(PlaywrightTestEnvironment))
                              .NoSync();

        if (exception is not null)
            throw exception;
    }
}