using ItemApiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItemApiProject.Services
{
    public class ItemRepository : IItemRepository
    {
        private ItemDbContext _itemContext;
        public ItemRepository(ItemDbContext itemContext)
        {
            _itemContext = itemContext;
        }

        public bool CreateItem(List<int> manufacturersId, List<int> categoriesId, Item item)
        {
            var manufacturers = _itemContext.Manufacturers.Where(a => manufacturersId.Contains(a.Id)).ToList();
            var categories = _itemContext.Categories.Where(c => categoriesId.Contains(c.Id)).ToList();

            

            foreach (var manufacturer in manufacturers)
            {
                var itemManufacturer = new ItemManufacturer()
                {
                    Manufacturer = manufacturer,
                    Item = item
                };
                _itemContext.Add(itemManufacturer);
            }

            foreach (var category in categories)
            {
                var itemCategory = new ItemCategory()
                {
                    Category = category,
                    Item = item
                };
                _itemContext.Add(itemCategory);
            }

            _itemContext.Add(item);

            return Save();
        }


        public bool DeleteItem(Item item)
        {
            _itemContext.Remove(item);
            return Save();
        }

        public Item GetItem(int ItemId)
        {
            return _itemContext.Items.Where(i => i.Id == ItemId).FirstOrDefault();
        }

        public ICollection<Item> GetItems()
        {
            return _itemContext.Items.OrderBy(i => i.Title).ToList();
        }

        public bool ItemExists(int ItemId)
        {
            return _itemContext.Items.Any(i => i.Id == ItemId);
        }

        public bool Save()
        {
            var saved = _itemContext.SaveChanges();
            return saved >= 0 ? true : false;
        }

        public bool UpdateItem(List<int> categoriesId, List<int> manufacturersId, Item item)
        {
            var manufacturers = _itemContext.Manufacturers.Where(m => manufacturersId.Contains(m.Id)).ToList();
            var categories = _itemContext.Categories.Where(c => categoriesId.Contains(c.Id)).ToList();

            var itemManufacturersToDelete = _itemContext.ItemManufacturers.Where(i => i.ItemId == item.Id);
            var itemCategoriesToDelete = _itemContext.ItemCategories.Where(i => i.ItemId == item.Id);

            _itemContext.RemoveRange(itemManufacturersToDelete);
            _itemContext.RemoveRange(itemCategoriesToDelete);   

            foreach (var manufacturer in manufacturers)
            {
                var itemManufacturer = new ItemManufacturer()
                {
                    Manufacturer = manufacturer,
                    Item = item
                };
                _itemContext.Add(itemManufacturer);
            }

            foreach (var category in categories)
            {
                var itemCategory = new ItemCategory()
                {
                    Category = category,
                    Item = item
                };
                _itemContext.Add(itemCategory);
            }

            _itemContext.Update(item);

            return Save();  
        }
    }
}
