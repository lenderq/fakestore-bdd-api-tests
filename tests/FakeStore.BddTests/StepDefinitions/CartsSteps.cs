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
public sealed class CartsSteps
{
    private readonly ApiScenarioContext _context;
    private readonly CartsClient _cartClient;

    public CartsSteps(ApiScenarioContext context)
    {
        _context = context;
        var settings = TestSettings.Load();

        var apiClient = new FakeStoreApiClient(settings);
        _cartClient = new CartsClient(apiClient);
    }

    [When("I request the cart list")]
    public async Task WhenIRequestTheCartList()
    {
        _context.LastResponse = await _cartClient.GetCartListAsync();
    }

    [Then("the cart list should be returned successfully")]
    public void ThenTheCartListShouldBeReturnedSuccessfully()
    {
        _context.LastResponse.Should().NotBeNull();
        _context.LastResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        _context.LastResponse.Content.Should().NotBeNullOrWhiteSpace();
    }

    [Then("the cart list should contain valid information")]
    public void ThenTheCartListShouldContainValidInformation()
    {
        _context.LastResponse.Should().NotBeNull();
        _context.LastResponse.Content.Should().NotBeNullOrWhiteSpace();

        using var document = JsonDocument.Parse(_context.LastResponse.Content!);

        document.RootElement.ValueKind.Should().Be(JsonValueKind.Array);

        var cartElements = document.RootElement.EnumerateArray().ToList();
        cartElements.Should().NotBeEmpty();

        foreach (var cartElement in cartElements)
        {
            cartElement.GetProperty("id").GetInt32().Should().BeGreaterThan(0);
            cartElement.GetProperty("userId").GetInt32().Should().BeGreaterThan(0);

            var dateElement = cartElement.GetProperty("date");
            dateElement.ValueKind.Should().Be(JsonValueKind.String);
            dateElement.GetString().Should().NotBeNullOrWhiteSpace();
            dateElement.TryGetDateTimeOffset(out var cartDate).Should().BeTrue();
            cartDate.Year.Should().BeGreaterThan(2000);

            var productsElement = cartElement.GetProperty("products");
            productsElement.Should().NotBeNull();
            productsElement.ValueKind.Should().Be(JsonValueKind.Array);

            var productsArray = productsElement.EnumerateArray().ToList();
            productsArray.Should().HaveCountGreaterThan(0);

            foreach (var product in productsArray)
            {
                product.GetProperty("productId").GetInt32().Should().BeGreaterThan(0);
                product.GetProperty("quantity").GetInt32().Should().BeGreaterThan(0);
            }
        }
    }

    [When ("I request the single cart with id {int}")]
    public async Task WhenIRequestTheSingleCartWithId(int cartId)
    {
        _context.LastResponse = await _cartClient.GetCartByCartIdAsync(cartId);
    }

    [Then("the cart should be returned successfully")]
    public void ThenTheCartShouldBeReturnedSuccessfully()
    {
        _context.LastResponse.Should().NotBeNull();
        _context.LastResponse!.StatusCode.Should().Be(HttpStatusCode.OK);
        _context.LastResponse.Content.Should().NotBeNullOrWhiteSpace();
    }

    [Then ("the cart id should be {int}")]
    public void ThenTheCartIdShouldBe(int expectedCartId)
    {
        _context.LastResponse.Should().NotBeNull();
        _context.LastResponse.Content.Should().NotBeNullOrWhiteSpace();

        using var document = JsonDocument.Parse(_context.LastResponse.Content!);

        document.RootElement.ValueKind.Should().Be(JsonValueKind.Object);

        var actualCartId = document.RootElement.GetProperty("id").GetInt32();

        actualCartId.Should().Be(expectedCartId);
    }

    [Then ("the cart should contain valid information")]
    public void ThenTheCartShouldContainValidInformation()
    {
        _context.LastResponse.Should().NotBeNull();
        _context.LastResponse.Content.Should().NotBeNullOrWhiteSpace();

        using var document = JsonDocument.Parse(_context.LastResponse.Content!);

        document.RootElement.ValueKind.Should().Be(JsonValueKind.Object);

        var cart = document.RootElement;

        cart.GetProperty("id").GetInt32().Should().BeGreaterThan(0);
        cart.GetProperty("userId").GetInt32().Should().BeGreaterThan(0);

        var dateElement = cart.GetProperty("date");
        dateElement.ValueKind.Should().Be(JsonValueKind.String);
        dateElement.GetString().Should().NotBeNullOrWhiteSpace();
        dateElement.TryGetDateTimeOffset(out var cartDate).Should().BeTrue();
        cartDate.Year.Should().BeGreaterThan(2000);

        var productsElement = cart.GetProperty("products");
        productsElement.Should().NotBeNull();
        productsElement.ValueKind.Should().Be(JsonValueKind.Array);

        var productsArray = productsElement.EnumerateArray().ToList();
        productsArray.Should().HaveCountGreaterThan(0);

        foreach (var product in productsArray)
        {
            product.GetProperty("productId").GetInt32().Should().BeGreaterThan(0);
            product.GetProperty("quantity").GetInt32().Should().BeGreaterThan(0);
        }
    }

    [When("I request the cart list with user id {int}")]
    public async Task WhenIRequestTheSingleCartWithUserId(int userId)
    {
        _context.LastResponse = await _cartClient.GetCartByUserIdAsync(userId);
    }

    [Then("all returned carts should belong to user {int}")]
    public void ThenTheUserIdShouldBe(int expectedUserId)
    {
        _context.LastResponse.Should().NotBeNull();
        _context.LastResponse.Content.Should().NotBeNullOrWhiteSpace();

        using var document = JsonDocument.Parse(_context.LastResponse.Content!);

        document.RootElement.ValueKind.Should().Be(JsonValueKind.Array);

        var carts = document.RootElement.EnumerateArray().ToList();
        carts.Should().NotBeEmpty();

        foreach (var cart in carts)
        {
            var actualUserId = cart.GetProperty("userId").GetInt32();
            actualUserId.Should().Be(expectedUserId);
        }
    }
}