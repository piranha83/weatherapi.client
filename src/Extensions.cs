using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;

namespace WeatherApi.Client;

/// <summary>
/// Extensions.
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Adds a WeatherService service to the specified IServiceCollection.
    /// </summary>
    /// <param name="serviceCollection">The Microsoft.Extensions.DependencyInjection.IServiceCollection.</param>
    /// <param name="configuration">A delegate that is used to configure an System.Net.Http.HttpClient.</param>
    /// <returns>The Microsoft.Extensions.DependencyInjection.IServiceCollection.</returns>
    public static IServiceCollection AddWeatherApiService(
            this IServiceCollection serviceCollection,
            IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(serviceCollection);
        ArgumentNullException.ThrowIfNull(configuration);

        serviceCollection.AddTransient<WeatherHandler>().AddHttpClient<IWeatherClient, WeatherClient>((sp, client) =>
        {
            var options = sp.GetRequiredService<IOptions<WeatherOptions>>();
            ArgumentNullException.ThrowIfNullOrWhiteSpace(options.Value?.Url);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(options.Value?.Token);

            client.BaseAddress = new Uri(options.Value.Url);
            client.DefaultRequestHeaders.UserAgent.ParseAdd("WeatherApi");
        })
        .AddHttpMessageHandler<WeatherHandler>()
        .AddPolicyHandler(Create())
        .SetHandlerLifetime(Timeout.InfiniteTimeSpan)
        .ConfigurePrimaryHttpMessageHandler(sp => Create(sp.GetRequiredService<IOptions<WeatherOptions>>()));

        serviceCollection.Configure<WeatherOptions>(configuration.GetSection("WeatherApi"));

        return serviceCollection;
    }

    private static IAsyncPolicy<HttpResponseMessage> Create()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }

    private static HttpMessageHandler Create(IOptions<WeatherOptions> options)
    {
        return new SocketsHttpHandler
        {
            EnableMultipleHttp2Connections = options.Value.Settings.EnableMultipleConnections ?? true,
            MaxConnectionsPerServer = options.Value.Settings.MaxConnections ?? 100,
            PooledConnectionIdleTimeout = TimeSpan.FromSeconds(options.Value.Settings.PooledConnectionIdleSeconds ?? 120),
            PooledConnectionLifetime = TimeSpan.FromSeconds(options.Value.Settings.PooledConnectionSeconds ?? 300),
            ConnectTimeout = TimeSpan.FromSeconds(options.Value.Settings.ConnectTimeoutSeconds ?? 30),
            UseCookies = false,
            UseProxy = options.Value.Settings.Proxy != null,
            Proxy = options.Value.Settings.Proxy != null ? new WebProxy(options.Value.Settings.Proxy, true)
            {
                Credentials = options.Value.Settings.ProxyUserName != null && options.Value.Settings.ProxyPassword != null
                    ? new NetworkCredential(options.Value.Settings.ProxyUserName, options.Value.Settings.ProxyPassword)
                    : null,
            } : null,
        };
    }
}