namespace WeatherApi.Client;

/// <summary>
/// Options.
/// </summary>
internal sealed record WeatherOptions
{
    /// <summary>
    /// Weather url.
    /// </summary>
    public string Url { get; set; } = default!;

    /// <summary>
    /// Token.
    /// </summary>
    public string Token { get; set; } = default!;

    /// <summary>
    /// Settings.
    /// </summary>
    public Settings Settings { get; set; } = new();
}

/// <summary>
/// Settings.
/// </summary>
internal sealed record Settings
{
    /// <summary>
    /// Proxy.
    /// </summary>
    public string? Proxy { get; set; }

    /// <summary>
    /// Proxy user name.
    /// </summary>
    public string? ProxyUserName { get; set; }

    /// <summary>
    /// Proxy password.
    /// </summary>
    public string? ProxyPassword { get; set; }

    /// <summary>
    /// Connection timeout in seconds. Default 30.
    /// </summary>
    public int? ConnectTimeoutSeconds { get; set; }

    /// <summary>
    /// Pooled connection life time in seconds. Default 300.
    /// </summary>
    /// <value></value>
    public int? PooledConnectionSeconds { get; set; }

    /// <summary>
    /// Pooled connection ide time in seconds. Default 120.
    /// </summary>
    /// <value></value>
    public int? PooledConnectionIdleSeconds { get; set; }

    /// <summary>
    /// Max connections. Default 100.
    /// </summary>
    /// <value></value>
    public int? MaxConnections { get; set; }

    /// <summary>
    /// Indicates whether additional connections can be established to the same server.
    /// </summary>
    public bool? EnableMultipleConnections { get; set; }
}