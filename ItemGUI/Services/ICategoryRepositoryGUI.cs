using ItemApiProject.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItemGUI.Services
{
    public interface ICategoryRepositoryGUI
    {
        IEnumerable<CategoryDto> GetCategories();
        CategoryDto GetCategoryById(int categoryId);
        IEnumerable<CategoryDto> GetAllCategoriesForItem(int itemId);
        IEnumerable<ItemDto> GetAllItemsForCategory(int categoryId);

    }
}
