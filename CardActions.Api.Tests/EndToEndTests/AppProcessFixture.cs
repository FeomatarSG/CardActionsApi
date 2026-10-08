using System.Diagnostics;

namespace CardActions.Api.Tests.EndToEndTests
{
    /// <summary>
    /// Starts the application as a separate process (a real Kestrel server on a real port)
    /// once for the whole test class and shuts it down afterwards.
    /// </summary>
    public class AppProcessFixture : IAsyncLifetime
    {
        private const string BaseUrl = "http://localhost:5299";

        private Process? _app;

        public HttpClient Client { get; } = new() { BaseAddress = new Uri(BaseUrl) };

        public async Task InitializeAsync()
        {
            // The test project references the application, so its compiled .dll
            // (together with appsettings.json) sits in the same folder as the tests.
            var startInfo = new ProcessStartInfo("dotnet", $"CardActions.Api.dll --urls {BaseUrl}")
            {
                WorkingDirectory = AppContext.BaseDirectory,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            // The application is tested with production configuration.
            startInfo.Environment["ASPNETCORE_ENVIRONMENT"] = "Production";

            _app = Process.Start(startInfo);

            // The server needs a moment to start, so the request is retried until it answers anything.
            for (var attempt = 0; attempt < 50; attempt++)
            {
                if (_app!.HasExited)
                {
                    throw new InvalidOperationException("The application exited during startup (for example the port is taken).");
                }

                try
                {
                    await Client.GetAsync("/");
                    return;
                }
                catch (HttpRequestException)
                {
                    await Task.Delay(200);
                }
            }

            throw new TimeoutException($"The application did not answer at {BaseUrl}.");
        }

        public Task DisposeAsync()
        {
            Client.Dispose();
            _app?.Kill(entireProcessTree: true);
            _app?.Dispose();
            return Task.CompletedTask;
        }
    }
}
