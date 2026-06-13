using FakeStore.BddTests.Models;
using RestSharp;

namespace FakeStore.BddTests.Clients;

public sealed class AuthClient
{
    private readonly FakeStoreApiClient _apiClient;

    public AuthClient(FakeStoreApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public Task<RestResponse> PostLoginAsync(LoginRequest request)
    {
        return _apiClient.PostAsync("/auth/login", request);
    }
}
