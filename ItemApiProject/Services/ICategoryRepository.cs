using ItemApiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItemApiProject.Services
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetCategories();
        Category GetCategory(int categoryId);
        ICollection<Category> GetAllCategoriesForItem(int itemId);
        ICollection<Item> GetAllItemsForCategory(int categoryId);
        bool CategoryExists(int categoryId);
        bool IsDuplicateCategoryName(int categoryId, string categoryName);

        bool CreateCategory(Category category);
        bool UpdateCategory(Category category);
        bool DeleteCategory(Category category);
        bool Save();

    }
}
