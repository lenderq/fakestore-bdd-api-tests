using FakeStore.BddTests.Models;
using RestSharp;

namespace FakeStore.BddTests.Context;

public sealed class ApiScenarioContext
{
    public RestResponse? LastResponse { get; set; }

    public CreateOrUpdateProductRequest? CreateOrPutProductRequest { get; set; }
}
