using FakeStore.BddTests.Config;
using FakeStore.BddTests.Models;
using RestSharp;

namespace FakeStore.BddTests.Clients;
public sealed class CartsClient
{
    private readonly FakeStoreApiClient _apiClient;

    public CartsClient(FakeStoreApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public Task<RestResponse> GetCartListAsync()
    {
        return _apiClient.GetAsync("/carts");
    }

    public Task<RestResponse> GetCartByCartIdAsync(int cartId)
    {
        return _apiClient.GetAsync($"/carts/{cartId}");
    }

    public Task<RestResponse> GetCartByUserIdAsync(int userId)
    {
        return _apiClient.GetAsync($"/carts/user/{userId}");
    }
}
