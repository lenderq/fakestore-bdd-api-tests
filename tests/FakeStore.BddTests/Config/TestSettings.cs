namespace FakeStore.BddTests.Config;

public sealed class TestSettings
{
    public string BaseUrl { get; }
    public int TimeoutSeconds { get; }
    public string EnvironmentName { get; }

    private TestSettings(string baseUrl, int timeoutSecond, string environmentName)
    {
        BaseUrl = baseUrl;
        TimeoutSeconds = timeoutSecond;
        EnvironmentName = environmentName;
    }

    public static TestSettings Load()
    {
        var baseUrl = Environment.GetEnvironmentVariable("FAKESTORE_BASE_URL") ?? "https://fakestoreapi.com";

        var environmentName = Environment.GetEnvironmentVariable("TEST_ENVIRONMENT") ?? "local";

        var timeoutRaw = Environment.GetEnvironmentVariable("FAKESTORE_TIMEOUT_SECONDS");

        var timeoutSeconds = int.TryParse(timeoutRaw, out var parsedTimeout) ? parsedTimeout : 30;

        return new TestSettings(baseUrl, timeoutSeconds, environmentName);
    }
}