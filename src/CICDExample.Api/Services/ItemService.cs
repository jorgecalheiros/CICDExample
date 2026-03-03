using CICDExample.Api.Models;

namespace CICDExample.Api.Services;

public class ItemService
{
    private readonly List<Item> _items = [];
    private int _nextId = 1;

    public IReadOnlyList<Item> GetAll() => _items.AsReadOnly();

    public Item? GetById(int id) => _items.FirstOrDefault(i => i.Id == id);

    public Item Create(string name)
    {
        var item = new Item(_nextId++, name);
        _items.Add(item);
        return item;
    }

    public bool Delete(int id)
    {
        var item = _items.FirstOrDefault(i => i.Id == id);
        if (item is null) return false;
        _items.Remove(item);
        return true;
    }
}
