using System.Net;
using System.Net.Http.Json;
using CICDExample.Api.Models;
using Microsoft.AspNetCore.Mvc.Testing;

namespace CICDExample.Tests.Integration;

public class ItemsApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ItemsApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetHealth_ShouldReturnOk()
    {
        var response = await _client.GetAsync("/health");

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task GetItems_ShouldReturnOkWithList()
    {
        var response = await _client.GetAsync("/items");

        response.EnsureSuccessStatusCode();
        var items = await response.Content.ReadFromJsonAsync<List<Item>>();
        Assert.NotNull(items);
    }

    [Fact]
    public async Task PostItem_ShouldReturnCreatedWithItem()
    {
        var response = await _client.PostAsJsonAsync("/items", new { name = "Integration Test Item" });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var item = await response.Content.ReadFromJsonAsync<Item>();
        Assert.NotNull(item);
        Assert.Equal("Integration Test Item", item.Name);
    }

    [Fact]
    public async Task GetItem_WithNonExistingId_ShouldReturnNotFound()
    {
        var response = await _client.GetAsync("/items/99999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteItem_WithNonExistingId_ShouldReturnNotFound()
    {
        var response = await _client.DeleteAsync("/items/99999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task PostAndGetItem_ShouldReturnCreatedItem()
    {
        var postResponse = await _client.PostAsJsonAsync("/items", new { name = "Round Trip Item" });
        var created = await postResponse.Content.ReadFromJsonAsync<Item>();

        var getResponse = await _client.GetAsync($"/items/{created!.Id}");

        getResponse.EnsureSuccessStatusCode();
        var found = await getResponse.Content.ReadFromJsonAsync<Item>();
        Assert.Equal(created.Id, found!.Id);
        Assert.Equal("Round Trip Item", found.Name);
    }

    [Fact]
    public async Task PostAndDeleteItem_ShouldReturnNoContent()
    {
        var postResponse = await _client.PostAsJsonAsync("/items", new { name = "To Be Deleted" });
        var created = await postResponse.Content.ReadFromJsonAsync<Item>();

        var deleteResponse = await _client.DeleteAsync($"/items/{created!.Id}");

        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
    }
}
