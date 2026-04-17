using System.Net;
using System.Net.Http.Json;
using Moq;
using Moq.Protected;

namespace WeatherApi.Client.Test;

[TestClass]
[TestCategory("Unit")]
public sealed class WeatherUnitTests
{
    [TestMethod]
    [DataRow(55.7569, 37.6151)]
    public async Task GetCurrentTest(double latitude, double longitude)
    {
        // Arrange
        using var httpClient = CreateHttpClientMoq(new GetCurrentResponse());
        var weatherClient = new WeatherClient(httpClient);

        var request = new GetCurrentRequest
        {
            Q = $"{latitude},{longitude}",
            Aqi = GetCurrentRequestAqi.No,
            Pollen = GetCurrentRequestPollen.No,
            Lang = GetCurrentRequestLang.Ru,
        };

        // Act
        var response = await weatherClient.GetCurrentAsync(request, default);

        // Assets
        Assert.IsNotNull(response);
    }

    [TestMethod]
    [DataRow(55.7569, 37.6151)]
    public async Task GetForecastTest(double latitude, double longitude)
    {
        // Arrange
        using var httpClient = CreateHttpClientMoq(new GetForecastResponse());
        var weatherClient = new WeatherClient(httpClient);

        var request = new GetForecastRequest
        {
            Q = $"{latitude},{longitude}",
            Days = 3,
            Alerts = GetForecastRequestAlerts.No,
            Aqi = GetForecastRequestAqi.No,
            Pollen = GetForecastRequestPollen.No,
            Lang = GetForecastRequestLang.Ru,
        };

        // Act
        var response = await weatherClient.GetForecastAsync(request, default);

        // Assets
        Assert.IsNotNull(response);
    }

    [TestMethod]
    [DataRow(55.7569, 37.6151)]
    public async Task GetHistoryTest(double latitude, double longitude)
    {
        // Arrange
        using var httpClient = CreateHttpClientMoq(new GetHistoryResponse());
        var weatherClient = new WeatherClient(httpClient);

        var request = new GetHistoryRequest
        {
            Q = $"{latitude},{longitude}",
            Aqi = GetHistoryRequestAqi.No,
            Pollen = GetHistoryRequestPollen.No,
            Lang = GetHistoryRequestLang.Ru,
        };

        // Act
        var response = await weatherClient.GetHistoryAsync(request, default);

        // Assets
        Assert.IsNotNull(response);
    }

    [TestMethod]
    [DataRow(55.7569, 37.6151)]
    public async Task GetAlertsTest(double latitude, double longitude)
    {
        // Arrange
        using var httpClient = CreateHttpClientMoq(new GetAlertsResponse());
        var weatherClient = new WeatherClient(httpClient);

        var request = new GetAlertsRequest
        {
            Q = $"{latitude},{longitude}",
        };

        // Act
        var response = await weatherClient.GetAlertsAsync(request, default);

        // Assets
        Assert.IsNotNull(response);
    }

    private static HttpClient CreateHttpClientMoq(object result)
    {
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
              "SendAsync",
              ItExpr.IsAny<HttpRequestMessage>(),
              ItExpr.IsAny<CancellationToken>()
           )
           .ReturnsAsync(new HttpResponseMessage
           {
               StatusCode = HttpStatusCode.OK,
               Content = JsonContent.Create(result),
           });

        var client = new HttpClient(handlerMock.Object) { BaseAddress = new Uri("https://test.com") };
        return client;
    }
}
