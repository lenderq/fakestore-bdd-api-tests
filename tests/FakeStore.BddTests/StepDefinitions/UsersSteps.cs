using Reqnroll;
using System.Net;
using System.Text.Json;
using FakeStore.BddTests.Clients;
using FakeStore.BddTests.Config;
using FakeStore.BddTests.Context;
using FluentAssertions;
using FakeStore.BddTests.Models;

namespace FakeStore.BddTests.StepDefinitions;

[Binding]
public sealed class UsersSteps
{
    private readonly ApiScenarioContext _context;
    private readonly UsersClient _usersClient;

    public UsersSteps(ApiScenarioContext context)
    {
        _context = context;
        var settings = TestSettings.Load();

        FakeStoreApiClient apiClient = new FakeStoreApiClient(settings);
        _usersClient = new UsersClient(apiClient);
    }

    [When("I request the user list")]
    public async Task WhenIRequestTheUsersList()
    {
        _context.LastResponse = await _usersClient.GetUsersListAsync();
    }

    [Then("the user list should be returned successfully")]
    public void ThenTheUsersListShouldBeReturnedSuccessfully()
    {
        _context.LastResponse.Should().NotBeNull();
        _context.LastResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        _context.LastResponse.Content.Should().NotBeNullOrWhiteSpace();
    }

    [Then("the user list should contain valid information")]
    public void ThenTheUsersListShouldContainValidInformation()
    {
        _context.LastResponse.Should().NotBeNull();
        _context.LastResponse.Content.Should().NotBeNullOrWhiteSpace();

        using var document = JsonDocument.Parse(_context.LastResponse.Content!);

        document.RootElement.ValueKind.Should().Be(JsonValueKind.Array);

        var users = document.RootElement.EnumerateArray().ToList();
        users.Should().NotBeEmpty();

        foreach (var user in users)
        {
            AssertUserHasValidInformation(user);
        }
    }

    [When("I request the user with id {int}")]
    public async Task WhenIRequestTheUserWithId(int userId)
    {
        _context.LastResponse = await _usersClient.GetUserByIdAsync(userId);
    }

    [Then("the user should be returned successfully")]
    public void ThenTheUserShouldBeReturnedSuccessfully()
    {
        _context.LastResponse.Should().NotBeNull();
        _context.LastResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        _context.LastResponse.Content.Should().NotBeNullOrWhiteSpace();
    }

    [Then("the user id should be {int}")]
    public void ThenTheUserIdShouldBe(int expectedUserId)
    {
        _context.LastResponse.Should().NotBeNull();
        _context.LastResponse.Content.Should().NotBeNullOrWhiteSpace();

        using var document = JsonDocument.Parse(_context.LastResponse.Content!);

        document.RootElement.ValueKind.Should().Be(JsonValueKind.Object);

        var actualId = document.RootElement.GetProperty("id").GetInt32();
        actualId.Should().Be(expectedUserId);
    }

    [Then("the user should contain valid information")]
    public void ThenTheUserShouldContainValidInformation()
    {
        _context.LastResponse.Should().NotBeNull();
        _context.LastResponse.Content.Should().NotBeNullOrWhiteSpace();

        using var document = JsonDocument.Parse(_context.LastResponse.Content!);

        document.RootElement.ValueKind.Should().Be(JsonValueKind.Object);

        AssertUserHasValidInformation(document.RootElement);
    }

    private static void AssertUserHasValidInformation(JsonElement user)
    {
        user.Should().NotBeNull();

        user.GetProperty("address").ValueKind.Should().Be(JsonValueKind.Object);

        var address = user.GetProperty("address");
        address.Should().NotBeNull();
        address.GetProperty("city").GetString().Should().NotBeNullOrWhiteSpace();
        address.GetProperty("street").GetString().Should().NotBeNullOrWhiteSpace();
        address.GetProperty("number").GetInt32().Should().BeGreaterThan(0);
        address.GetProperty("zipcode").GetString().Should().NotBeNullOrWhiteSpace();

        address.GetProperty("geolocation").ValueKind.Should().Be(JsonValueKind.Object);

        var geolocation = address.GetProperty("geolocation");
        geolocation.GetProperty("lat").GetString().Should().NotBeNullOrWhiteSpace();
        geolocation.GetProperty("long").GetString().Should().NotBeNullOrWhiteSpace();

        user.GetProperty("id").GetInt32().Should().BeGreaterThan(0);

        var email = user.GetProperty("email").GetString();
        email.Should().NotBeNullOrWhiteSpace();
        email.Should().Contain("@");

        user.GetProperty("username").GetString().Should().NotBeNullOrWhiteSpace();

        user.GetProperty("name").ValueKind.Should().Be(JsonValueKind.Object);

        var name = user.GetProperty("name");
        name.Should().NotBeNull();

        name.GetProperty("firstname").GetString().Should().NotBeNullOrWhiteSpace();
        name.GetProperty("lastname").GetString().Should().NotBeNullOrWhiteSpace();

        user.GetProperty("phone").GetString().Should().NotBeNullOrWhiteSpace();
    }
}
