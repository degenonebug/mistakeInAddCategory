using ItemApiProject.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItemGUI.ViewModels
{
    public class ItemCategoriesManufacturersViewModel
    {
        public ItemDto Item { get; set; }
        public IEnumerable<CategoryDto> Categories {get; set;}
        public IEnumerable<ManufacturerDto> Manufacturers { get; set; }
    }
}
