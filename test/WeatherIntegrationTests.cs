using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WeatherApi.Client.Test;

[TestClass]
[TestCategory("Integration")]
public sealed class WeatherIntegrationTests
{
    ServiceProvider? sp;
    IServiceScope? scope;

    [TestInitialize]
    public void Initialize()
    {
        var config = new ConfigurationBuilder();
        config.AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["WeatherApi:Url"] = "https://api.weatherapi.com/",
        })
        .AddJsonFile("appsettings.test.json", optional: true);

        var sc = new ServiceCollection();
        sc.AddWeatherApiService(config.Build());

        sp = sc.BuildServiceProvider(true);
        scope = sp.CreateScope();
    }

    [TestCleanup]
    public void Cleanup()
    {
        scope?.Dispose();
        sp?.Dispose();
    }

    [TestMethod]
    [DataRow(55.752, 37.616)]
    public async Task GetCurrentTest(double latitude, double longitude)
    {
        // Arrange
        var weatherClient = scope!.ServiceProvider.GetRequiredService<IWeatherClient>()!;

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
        Assert.AreEqual(latitude, response.Location.Lat);
        Assert.AreEqual(longitude, response.Location.Lon);
    }

    [TestMethod]
    [DataRow(55.752, 37.616)]
    public async Task GetForecastTest(double latitude, double longitude)
    {
        // Arrange
        var weatherClient = scope!.ServiceProvider.GetRequiredService<IWeatherClient>()!;

        var request = new GetForecastRequest
        {
            Q = $"{latitude},{longitude}",
            Days = 3,
            Hour = 22,
            Alerts = GetForecastRequestAlerts.No,
            Aqi = GetForecastRequestAqi.No,
            Pollen = GetForecastRequestPollen.No,
            Lang = GetForecastRequestLang.Ru,
            Et0 = GetForecastRequestEt0.No,
        };

        // Act
        var response = await weatherClient.GetForecastAsync(request, default);

        // Assets
        Assert.IsNotNull(response);
        Assert.AreEqual(latitude, response.Location.Lat);
        Assert.AreEqual(longitude, response.Location.Lon);
    }

    [TestMethod]
    [DataRow(55.752, 37.616)]
    [Ignore("API key is limited to get history data")]
    public async Task GetHistoryTest(double latitude, double longitude)
    {
        // Arrange
        var weatherClient = scope!.ServiceProvider.GetRequiredService<IWeatherClient>()!;

        var request = new GetHistoryRequest
        {
            Q = $"{latitude},{longitude}",
            Dt = "2026-01-01 00:00",
            End_dt = "2026-01-01 00:30",
            Hour = 22,
            Tp = 30,
            Solar = GetHistoryRequestSolar.No,
            Wind100kph = GetHistoryRequestWind100kph.No,
            Wind100mph = GetHistoryRequestWind100mph.No,
            Et0 = GetHistoryRequestEt0.No,
            Aqi = GetHistoryRequestAqi.No,
            Pollen = GetHistoryRequestPollen.No,
            Lang = GetHistoryRequestLang.Ru,
        };

        // Act
        var response = await weatherClient.GetHistoryAsync(request, default);

        // Assets
        Assert.IsNotNull(response);
        Assert.AreEqual(latitude, response.Location.Lat);
        Assert.AreEqual(longitude, response.Location.Lon);
    }

    [TestMethod]
    [DataRow(55.752, 37.616)]
    public async Task GetAlertsTest(double latitude, double longitude)
    {
        // Arrange
        var weatherClient = scope!.ServiceProvider.GetRequiredService<IWeatherClient>()!;

        var request = new GetAlertsRequest
        {
            Q = $"{latitude},{longitude}",
        };

        // Act
        var response = await weatherClient.GetAlertsAsync(request, default);

        // Assets
        Assert.IsNotNull(response);
        Assert.AreEqual(latitude, response.Location.Lat);
        Assert.AreEqual(longitude, response.Location.Lon);
    }
}
