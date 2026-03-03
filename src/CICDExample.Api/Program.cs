using CICDExample.Api.Models;
using CICDExample.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ItemService>();

var app = builder.Build();

app.MapGet("/health", () => Results.Ok(new { status = "healthy" }))
    .WithName("GetHealth");

app.MapGet("/items", (ItemService service) =>
    Results.Ok(service.GetAll()))
    .WithName("GetItems");

app.MapGet("/items/{id:int}", (int id, ItemService service) =>
{
    var item = service.GetById(id);
    return item is null ? Results.NotFound() : Results.Ok(item);
})
.WithName("GetItemById");

app.MapPost("/items", (CreateItemRequest request, ItemService service) =>
{
    var item = service.Create(request.Name);
    return Results.Created($"/items/{item.Id}", item);
})
.WithName("CreateItem");

app.MapDelete("/items/{id:int}", (int id, ItemService service) =>
{
    var deleted = service.Delete(id);
    return deleted ? Results.NoContent() : Results.NotFound();
})
.WithName("DeleteItem");

app.Run();

public partial class Program { }
