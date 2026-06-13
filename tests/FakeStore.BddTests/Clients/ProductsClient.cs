using FakeStore.BddTests.Models;
using RestSharp;

namespace FakeStore.BddTests.Clients;

public sealed class ProductsClient
{
    private readonly FakeStoreApiClient _apiClient;

    public ProductsClient(FakeStoreApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public Task<RestResponse> GetProductCatalogAsync()
    {
        return _apiClient.GetAsync("/products");
    }

    public Task<RestResponse> GetProductByIdAsync(int productId)
    {
        return _apiClient.GetAsync($"/products/{productId}");
    }

    public Task<RestResponse> GetProductWithLimitAsync(int limit)
    {
        return _apiClient.GetAsync($"/products?limit={limit}");
    }

    public Task<RestResponse> GetProductCategoriesAsync()
    {
        return _apiClient.GetAsync("/products/categories");
    }

    public Task<RestResponse> PostProductAsync(CreateOrUpdateProductRequest request)
    {
        return _apiClient.PostAsync("/products", request);
    }

    public Task<RestResponse> PutProductByIdAsync(int productId, CreateOrUpdateProductRequest request)
    {
        return _apiClient.PutAsync($"/products/{productId}", request);
    }

    public Task<RestResponse> DeleteProductByIdAsync(int productId)
    {
        return _apiClient.DeleteAsync($"/products/{productId}");
    }
}