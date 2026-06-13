using FakeStore.BddTests.Context;
using Reqnroll;
using Serilog;

namespace FakeStore.BddTests.Hooks;

[Binding]
public sealed class TestHooks
{
    private readonly ScenarioContext _scenarioContext;

    public TestHooks(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;        
    }

    [BeforeTestRun]
    public static void BeforeTestRun()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .WriteTo.File("logs/test-run-.log", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        Log.Information("Test run started");
    }

    [BeforeScenario]
    public void BeforeScenario()
    {
        var scenarioTitle = _scenarioContext.ScenarioInfo.Title;
        var tags = string.Join(", ", _scenarioContext.ScenarioInfo.Tags);

        Log.Information("Starting scenario: {ScenarioTitle}", scenarioTitle);
        Log.Information("Scenario tags: {scenarioTags}", tags);
        Log.Information("Scenario started at: {StartedAt}", DateTimeOffset.Now);
    }

    [AfterScenario]
    public void AfterScenario()
    {
        var scenarioTitle = _scenarioContext.ScenarioInfo.Title;

        if(_scenarioContext.TestError is null)
        {
            Log.Information("Scenario finished successfully: {ScenarioTitle}", scenarioTitle);
        }
        else
        {
            Log.Error(_scenarioContext.TestError, "Scenario failed", scenarioTitle);
        }
    }

    [AfterTestRun]
    public static void AfterTestRun()
    {
        Log.Information("Test run finished");
        Log.CloseAndFlush();
    }
}
