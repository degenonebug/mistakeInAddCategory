using ItemApiProject.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItemGUI.Services
{
    public interface IItemRepositoryGUI
    {
        IEnumerable<ItemDto> GetItems();
        ItemDto GetItemById(int itemId);
    }
}
