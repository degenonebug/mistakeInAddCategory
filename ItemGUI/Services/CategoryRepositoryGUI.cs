using ItemApiProject.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ItemGUI.Services
{
    public class CategoryRepositoryGUI : ICategoryRepositoryGUI
    {
        public IEnumerable<CategoryDto> GetAllCategoriesForItem(int itemId)
        {
            IEnumerable<CategoryDto> categories = new List<CategoryDto>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:62352/api/");

                var response = client.GetAsync($"categories/items/{itemId}");
                response.Wait();

                var result = response.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<CategoryDto>>();
                    readTask.Wait();

                    categories = readTask.Result;
                }
            }
            return categories;
        }

        public IEnumerable<CategoryDto> GetCategories()
        {
            IEnumerable<CategoryDto> categories = new List<CategoryDto>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:62352/api/");

                var response = client.GetAsync("categories");
                response.Wait();

                var result = response.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<CategoryDto>>();
                    readTask.Wait();

                    categories = readTask.Result;
                }
            }
            return categories;
        }

        public IEnumerable<ItemDto> GetAllItemsForCategory(int categoryId)
        {
            IEnumerable<ItemDto> items = new List<ItemDto>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:62352/api/");

                var response = client.GetAsync($"categories/{categoryId}/items");
                response.Wait();

                var result = response.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<ItemDto>>();
                    readTask.Wait();

                    items = readTask.Result;
                }
            }
            return items;
        }

        public CategoryDto GetCategoryById(int categoryId)
        {
            CategoryDto category = new CategoryDto();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:62352/api/");

                var response = client.GetAsync($"categories/{categoryId}");
                response.Wait();

                var result = response.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<CategoryDto>();
                    readTask.Wait();

                    category = readTask.Result;
                }
            }
            return category;
        }
    }
}
