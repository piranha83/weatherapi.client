using Microsoft.Extensions.Options;

namespace WeatherApi.Client;

///<inheritdoc/>
internal sealed class WeatherHandler(IOptions<WeatherOptions> options) : DelegatingHandler
{
    ///<inheritdoc/>
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Add("key", options.Value.Token);

        return await base.SendAsync(request, cancellationToken);
    }
}