using ItemApiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItemApiProject.Services
{
    public interface IItemRepository
    {
        ICollection<Item> GetItems();
        Item GetItem(int ItemId);
        bool ItemExists(int ItemId);

        bool CreateItem(List<int> categoriesId, List<int> manufacturersId, Item item);
        bool UpdateItem(List<int> categoriesId, List<int> manufacturersId, Item item);
        bool DeleteItem(Item item);
        bool Save();
    }
}
