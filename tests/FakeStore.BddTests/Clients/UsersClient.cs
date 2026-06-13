using RestSharp;

namespace FakeStore.BddTests.Clients;

public sealed class UsersClient
{
    private readonly FakeStoreApiClient _apiClient;

    public UsersClient(FakeStoreApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public Task<RestResponse> GetUsersListAsync()
    {
        return _apiClient.GetAsync("/users");
    }

    public Task<RestResponse> GetUserByIdAsync(int userId)
    {
        return _apiClient.GetAsync($"/users/{userId}");
    }
}