namespace CICDExample.Api.Models;

public record Item(int Id, string Name);

public record CreateItemRequest(string Name);
