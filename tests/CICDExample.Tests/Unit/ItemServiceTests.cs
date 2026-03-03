using CICDExample.Api.Services;

namespace CICDExample.Tests.Unit;

public class ItemServiceTests
{
    private readonly ItemService _service = new();

    [Fact]
    public void Create_ShouldAddItemWithIncrementalId()
    {
        var item1 = _service.Create("First");
        var item2 = _service.Create("Second");

        Assert.Equal(1, item1.Id);
        Assert.Equal(2, item2.Id);
    }

    [Fact]
    public void GetAll_ShouldReturnAllCreatedItems()
    {
        _service.Create("A");
        _service.Create("B");

        Assert.Equal(2, _service.GetAll().Count);
    }

    [Fact]
    public void GetById_WithExistingId_ShouldReturnItem()
    {
        var created = _service.Create("Test");

        var found = _service.GetById(created.Id);

        Assert.NotNull(found);
        Assert.Equal(created.Name, found.Name);
    }

    [Fact]
    public void GetById_WithNonExistingId_ShouldReturnNull()
    {
        var result = _service.GetById(999);

        Assert.Null(result);
    }

    [Fact]
    public void Delete_WithExistingId_ShouldRemoveItemAndReturnTrue()
    {
        var item = _service.Create("ToDelete");

        var result = _service.Delete(item.Id);

        Assert.True(result);
        Assert.Empty(_service.GetAll());
    }

    [Fact]
    public void Delete_WithNonExistingId_ShouldReturnFalse()
    {
        var result = _service.Delete(999);

        Assert.False(result);
    }
}
