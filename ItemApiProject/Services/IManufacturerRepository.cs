using ItemApiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItemApiProject.Services
{
    public interface IManufacturerRepository
    {
        ICollection<Manufacturer> GetManufacturers();
        Manufacturer GetManufacturer(int manufacturerId);
        ICollection<Manufacturer> GetAllManufacturersForItem(int itemId);
        ICollection<Item> GetAllItemsForManufacturer(int manufacturerId);
        bool ManufacturerExists(int manufacturerId);
        bool IsDuplicateManufacturerName(int manufacturerId, string manufacturerName);

        bool CreateManufacturer(Manufacturer manufacturer);
        bool UpdateManufacturer(Manufacturer manufacturer);
        bool DeleteManufacturer(Manufacturer manufacturer);
        bool Save();
    }
}
