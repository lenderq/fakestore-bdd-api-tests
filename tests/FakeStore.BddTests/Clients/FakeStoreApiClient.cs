using System.Diagnostics;
using Serilog;
using FakeStore.BddTests.Config;
using RestSharp;

namespace FakeStore.BddTests.Clients;

public sealed class FakeStoreApiClient
{
    private readonly RestClient _client;

    public FakeStoreApiClient(TestSettings settings)
    {
        var options = new RestClientOptions(settings.BaseUrl)
        {
            ThrowOnAnyError = false,
            Timeout = TimeSpan.FromSeconds(settings.TimeoutSeconds)
        };

        _client = new RestClient(options);
    }

    public Task<RestResponse> GetAsync(string resource)
    {
        var request = new RestRequest(resource, Method.Get);

        return ExecuteAsync(request);
    }

    public Task<RestResponse> PostAsync(string resource, object body)
    {
        var request = new RestRequest(resource, Method.Post);
        request.AddJsonBody(body);

        return ExecuteAsync(request);
    }

    public Task<RestResponse> PutAsync(string resource, object body)
    {
        var request = new RestRequest(resource, Method.Put);
        request.AddJsonBody(body);

        return ExecuteAsync(request);
    }

    public Task<RestResponse> PatchAsync(string resource, object body)
    {
        var request = new RestRequest(resource, Method.Patch);
        request.AddJsonBody(body);

        return ExecuteAsync(request);
    }

    public Task<RestResponse> DeleteAsync(string resource)
    {
        var request = new RestRequest(resource, Method.Delete);

        return ExecuteAsync(request);
    }

    private async Task<RestResponse> ExecuteAsync(RestRequest request)
    {
        var stopwatch = Stopwatch.StartNew();

        Log.Information("Sending HTTP request: {Method} {Resource}", request.Method, request.Resource);

        var response = await _client.ExecuteAsync(request);

        stopwatch.Stop();

        Log.Information("Received HTTP response: {Method} {Resource} -> {StatusCode} {StatusDescription}" +
            " in {ElapsedMilliseconds} ms", request.Method, request.Resource, (int)response.StatusCode,
            response.StatusCode, stopwatch.ElapsedMilliseconds);

        if (!response.IsSuccessful)
        {
            Log.Warning("HTTP request was not successful: {Method} {Resource}." +
                " ResponseStatus: {ResponseStatus}. ErrorMessage: {ErrorMessage}",
                request.Method, request.Resource, response.ResponseStatus, response.ErrorMessage);
        }
        return response;
    }
}

