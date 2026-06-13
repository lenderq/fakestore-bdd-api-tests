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
public sealed class ProductsSteps
{
	private readonly ApiScenarioContext _context;
	private readonly ProductsClient _productClient;

	public ProductsSteps(ApiScenarioContext context)
	{
		_context = context;
		var settings = TestSettings.Load();

		var apiClient = new FakeStoreApiClient(settings);
		_productClient = new ProductsClient(apiClient);
	}

	[When("I request the product catalog")]
	public async Task WhenIRequestTheProductCatalog()
	{
		_context.LastResponse = await _productClient.GetProductCatalogAsync();
	}

	[Then("the product catalog should be returned successfully")]
	public void ThenTheProductCatalogShouldBeReturnedSuccessfully()
	{
		_context.LastResponse.Should().NotBeNull();

		_context.LastResponse!.StatusCode.Should().Be(HttpStatusCode.OK);
		_context.LastResponse.Content.Should().NotBeNullOrWhiteSpace();
	}

	[Then("the product catalog should contain valid product information")]
	public void ThenTheProductCatalogShouldContainValidProductInformation()
	{
		_context.LastResponse.Should().NotBeNull();
		_context.LastResponse!.Content.Should().NotBeNullOrWhiteSpace();

		using var document = JsonDocument.Parse(_context.LastResponse.Content!);

		document.RootElement.ValueKind.Should().Be(JsonValueKind.Array);

		var products = document.RootElement.EnumerateArray().ToList();

		products.Should().NotBeEmpty();

		foreach (var product in products)
		{
			product.GetProperty("id").GetInt32().Should().BeGreaterThan(0);
			product.GetProperty("title").GetString().Should().NotBeNullOrWhiteSpace();
			product.GetProperty("price").GetDecimal().Should().BeGreaterThan(0);
			product.GetProperty("category").GetString().Should().NotBeNullOrWhiteSpace();
		}
	}

	[When("I request product with id {int}")]
	public async Task WhenIRequestProductWithId(int productId)
	{
		_context.LastResponse = await _productClient.GetProductByIdAsync(productId);
	}

	[Then("the product should be returned successfully")]
	public void ThenTheProductShouldBeReturnedSuccessfully()
	{
		_context.LastResponse.Should().NotBeNull();

		_context.LastResponse!.StatusCode.Should().Be(HttpStatusCode.OK);
		_context.LastResponse.Content.Should().NotBeNullOrWhiteSpace();
	}

	[Then("the product id should be {int}")]
	public void ThenTheProductIdShouldBe(int expectedProductId)
	{
		_context.LastResponse.Should().NotBeNull();
		_context.LastResponse!.Content.Should().NotBeNullOrWhiteSpace();

		using var document = JsonDocument.Parse(_context.LastResponse.Content!);

		document.RootElement.ValueKind.Should().Be(JsonValueKind.Object);

		var actualProductId = document.RootElement.GetProperty("id").GetInt32();

		actualProductId.Should().Be(expectedProductId);
	}

	[Then("the product should contain valid product information")]
	public void ThenTheProductShouldContainValidProductInformation()
	{
		_context.LastResponse.Should().NotBeNull();
		_context.LastResponse.Content.Should().NotBeNullOrWhiteSpace();

		using var document = JsonDocument.Parse(_context.LastResponse.Content!);

		var product = document.RootElement;
		product.ValueKind.Should().Be(JsonValueKind.Object);

		product.GetProperty("id").GetInt32().Should().BeGreaterThan(0);
		product.GetProperty("title").GetString().Should().NotBeNullOrWhiteSpace();
		product.GetProperty("price").GetDecimal().Should().BeGreaterThan(0);
		product.GetProperty("category").GetString().Should().NotBeNullOrWhiteSpace();
	}

	[When("I request the product catalog with limit {int}")]
	public async Task WhenIRequestTheProductCatalogWithLimit(int limit)
	{
		_context.LastResponse = await _productClient.GetProductWithLimitAsync(limit);
	}

	[Then("the product count should be {int}")]
	public void ThenTheProductCountShouldBe(int count)
	{
		_context.LastResponse.Should().NotBeNull();
		_context.LastResponse.Content.Should().NotBeNullOrWhiteSpace();

		using var document = JsonDocument.Parse(_context.LastResponse.Content!);

		document.RootElement.ValueKind.Should().Be(JsonValueKind.Array);

		var products = document.RootElement.EnumerateArray().ToList();

		products.Count.Should().Be(count);
	}

	[When("I request product categories")]
	public async Task WhenIRequestProductCategories()
	{
		_context.LastResponse = await _productClient.GetProductCategoriesAsync();
	}

	[Then("the product categories should be returned successfully")]
	public void ThenTheProductCategoriesShouldBeReturnedSuccessfully()
	{
		_context.LastResponse.Should().NotBeNull();
		_context.LastResponse.StatusCode.Should().Be(HttpStatusCode.OK);
		_context.LastResponse.Content.Should().NotBeNullOrWhiteSpace();
	}

	[Then("the product categories should contain valid information")]
	public void ThenTheProductCategoriesShouldContainValidInformation()
	{
        _context.LastResponse.Should().NotBeNull();
        _context.LastResponse.Content.Should().NotBeNullOrWhiteSpace();

		using var document = JsonDocument.Parse(_context.LastResponse.Content!);

		document.RootElement.ValueKind.Should().Be(JsonValueKind.Array);

		var products = document.RootElement.EnumerateArray().ToList();

		foreach (var product in products)
		{
			product.GetString().Should().NotBeNullOrWhiteSpace();
		}
    }

	[When("I create a product")]
	public async Task WhenICreateAProduct()
	{
		var request = new CreateOrUpdateProductRequest
		{
			Title = "Test product",
			Price = 13.5m,
			Description = "Test description",
			Image = "https://i.pravatar.cc",
			Category = "electronics"
		}; 

		_context.CreateOrPutProductRequest = request;
		_context.LastResponse = await _productClient.PostProductAsync(request);
	}

	[Then("the product should be created successfully")]
	public void ThenTheProductShouldBeCreatedSuccessfully()
	{
		_context.LastResponse.Should().NotBeNull();
		_context.LastResponse.StatusCode.Should().Be(HttpStatusCode.Created);
		_context.LastResponse.Content.Should().NotBeNullOrWhiteSpace();
	}

	[Then("the product should contain reliable information sent about the product")]
	public void ThenTheProductShouldContainReliableInformationSentAboutTheProduct()
	{
		_context.LastResponse.Should().NotBeNull();
		_context.LastResponse.Content.Should().NotBeNullOrWhiteSpace();

		var expectedProduct = _context.CreateOrPutProductRequest!;

		using var document = JsonDocument.Parse(_context.LastResponse.Content!);

		document.RootElement.ValueKind.Should().Be(JsonValueKind.Object);

		var product = document.RootElement;

		product.GetProperty("id").GetInt32().Should().BeGreaterThan(0);
		product.GetProperty("title").GetString().Should().Be(expectedProduct.Title);
		product.GetProperty("price").GetDecimal().Should().Be(expectedProduct.Price);
		product.GetProperty("description").GetString().Should().Be(expectedProduct.Description);
		product.GetProperty("category").GetString().Should().Be(expectedProduct.Category);
	}

	[When("I update a product with id {int}")]
	public async Task WhenIUpdateAProductWithId(int productId)
	{
        var request = new CreateOrUpdateProductRequest
        {
            Title = "Test product",
            Price = 13.5m,
            Description = "Test description",
            Image = "https://i.pravatar.cc",
            Category = "electronics"
        };

		_context.CreateOrPutProductRequest = request;

        _context.LastResponse = await _productClient.PutProductByIdAsync(productId, request);
	}

	[When("I delete a product with id {int}")]
	public async Task WhenIDeleteAProductWithId(int productId)
	{
		_context.LastResponse = await _productClient.DeleteProductByIdAsync(productId);
	}
}