using ItemApiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItemApiProject.Services
{
    public class ManufacturerRepository : IManufacturerRepository
    {
        private ItemDbContext _manufacturerContext;
        public ManufacturerRepository(ItemDbContext manufacturerContext)
        {
            _manufacturerContext = manufacturerContext;
        }

        public bool CreateManufacturer(Manufacturer manufacturer)
        {
            _manufacturerContext.Add(manufacturer);
            return Save();
        }

        public bool DeleteManufacturer(Manufacturer manufacturer)
        {
            _manufacturerContext.Remove(manufacturer);
            return Save();
        }

        public ICollection<Item> GetAllItemsForManufacturer(int manufacturerId)
        {
            return _manufacturerContext.ItemManufacturers.Where(m => m.ManufacturerId == manufacturerId).Select(m => m.Item).ToList();
        }

        public ICollection<Manufacturer> GetAllManufacturersForItem(int itemId)
        {
            return _manufacturerContext.ItemManufacturers.Where(i => i.ItemId == itemId).Select(m => m.Manufacturer).ToList();
        }

        public Manufacturer GetManufacturer(int manufacturerId)
        {
            return _manufacturerContext.Manufacturers.Where(m => m.Id == manufacturerId).FirstOrDefault();
        }

        public ICollection<Manufacturer> GetManufacturers()
        {
            return _manufacturerContext.Manufacturers.OrderBy(m => m.Name).ToList();
        }

        public bool IsDuplicateManufacturerName(int manufacturerId, string manufacturerName)
        {
            var manufacturer = _manufacturerContext.Categories.Where(m => m.Name.Trim().ToUpper() == manufacturerName.Trim().ToUpper()
                                                && m.Id != manufacturerId).FirstOrDefault();
            return manufacturer == null ? false : true;

        }

        public bool ManufacturerExists(int manufacturerId)
        {
            return _manufacturerContext.Manufacturers.Any(m => m.Id == manufacturerId);
        }

        public bool Save()
        {
            var saved = _manufacturerContext.SaveChanges();
            return saved >= 0 ? true : false;
        }

        public bool UpdateManufacturer(Manufacturer manufacturer)
        {
            _manufacturerContext.Update(manufacturer);
            return Save();
        }
    }
}
