using ItemApiProject.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItemGUI.Services
{
    public interface IManufacturerRepositoryGUI
    {
        IEnumerable<ManufacturerDto> GetManufacturers();
        ManufacturerDto GetManufacturerById(int manufacturerId);
        IEnumerable<ManufacturerDto> GetAllManufacturersForItem(int itemId);
        IEnumerable<ItemDto> GetItemsForManufacturer(int manufacturerId);
    }
}
