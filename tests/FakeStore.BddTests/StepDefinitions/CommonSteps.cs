using Reqnroll;
using FakeStore.BddTests.Config;
using FakeStore.BddTests.Context;
using FluentAssertions;

namespace FakeStore.BddTests.StepDefinitions;

[Binding]
public sealed class CommonSteps
{
    private readonly TestSettings _settings;

    public CommonSteps(ApiScenarioContext context)
    {
        _settings = TestSettings.Load();
    }

    [Given("the Fake Store API is available")]
    public void GivenTheFakeStoreAPIisAvailable()
    {
        _settings.BaseUrl.Should().NotBeNullOrWhiteSpace();
        _settings.TimeoutSeconds.Should().BeGreaterThan(0);
    }
}
