using FakeStore.BddTests.Clients;
using FakeStore.BddTests.Config;
using FakeStore.BddTests.Context;
using FakeStore.BddTests.Models;
using System.Net;
using FluentAssertions;
using Reqnroll;
using System.Text.Json;

namespace FakeStore.BddTests.StepDefinitions;

[Binding]
public sealed class AuthSteps
{
    private readonly ApiScenarioContext _context;
    private readonly AuthClient _authClient;

    public AuthSteps(ApiScenarioContext context)
    {
        _context = context;
        var settings = TestSettings.Load();

        var apiClient = new FakeStoreApiClient(settings);
        _authClient = new AuthClient(apiClient);
    }

    [When("I login with valid credentials")]
    public async Task WhenILoginWithValidCredentials()
    {
        LoginRequest request = new LoginRequest
        {
            Username = "mor_2314", 
            Password = "83r5^_"
        };

        _context.LastResponse = await _authClient.PostLoginAsync(request);
    }

    [Then("the login response should be returned successfully")]
    public void ThenTheLoginResponseShouldBeReturnedSuccessfully()
    {
        _context.LastResponse.Should().NotBeNull();
        _context.LastResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        _context.LastResponse.Content.Should().NotBeNullOrWhiteSpace();
    }

    [Then("the login response should contain an auth token")]
    public void ThenTheLoginResponseShouldContainAnAuthToken()
    {
        _context.LastResponse.Should().NotBeNull();
        _context.LastResponse.Content.Should().NotBeNullOrWhiteSpace();

        using var document = JsonDocument.Parse(_context.LastResponse.Content!);

        document.RootElement.ValueKind.Should().Be(JsonValueKind.Object);

        document.RootElement.GetProperty("token").GetString().Should().NotBeNullOrWhiteSpace();
    }

    [When("I login with invalid credentials")]
    public async Task WhenILoginWithInvalidCredentials()
    {
        LoginRequest request = new LoginRequest
        {
            Username = "-33",
            Password = "83r5^_"
        };

        _context.LastResponse = await _authClient.PostLoginAsync(request);
    }

    [Then("the login request should be rejected")]
    public void ThenTheLoginRequestShouldBeRejected()
    {
        _context.LastResponse.Should().NotBeNull();
        _context.LastResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}