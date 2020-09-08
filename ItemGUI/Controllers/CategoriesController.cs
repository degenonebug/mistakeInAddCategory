using ItemApiProject.Dtos;
using ItemApiProject.Models;
using ItemApiProject.Services;
using ItemGUI.Services;
using ItemGUI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ItemGUI.Controllers
{
    public class CategoriesController : Controller
    {
        ICategoryRepositoryGUI _categoryRepository;
        public CategoriesController(ICategoryRepositoryGUI categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public IActionResult Index()
        {
            var categories = _categoryRepository.GetCategories();

            if(categories.Count() <= 0)
            {
                ViewBag.Message = "Проблема с извлечением категории с базы данных или категория не существует";
            }

            return View(categories);
        }

        public IActionResult GetCategoryById(int categoryId)
        {
            var category = _categoryRepository.GetCategoryById(categoryId);

            if (category == null)
            {
                ModelState.AddModelError("", "Ошибка при получении категории");
                ViewBag.Message = $"Проблема с извлечением категории с Id {categoryId} " + 
                    $"из базы данных (возможно такая категория не существует)";
                category = new CategoryDto();
            }

            var items = _categoryRepository.GetAllItemsForCategory(categoryId);
            if (items.Count() <= 0)
            {
                ViewBag.ItemMessage = $"В { category.Name} категории нету товаров";
            }

            var itemCategoryViewModel = new CategoryItemsViewModel()
            {
                Category = category,
                Items = items
            };

            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            return View(itemCategoryViewModel);
        }

        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateCategory(Category category)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:62352/api/");
                var responseTask = client.PostAsJsonAsync("categories", category);
                responseTask.Wait();

                var result = responseTask.Result;
                if(result.IsSuccessStatusCode)
                {
                    var newCategoryTask = result.Content.ReadAsAsync<Category>();
                    newCategoryTask.Wait();

                    var newCategory = newCategoryTask.Result;
                    TempData["SuccessMessage"] = $"Категория {newCategory.Name} была успешно создана";

                    return RedirectToAction("GetCategoryById", new { categoryId = newCategory.Id });

                }

                if ((int)result.StatusCode == 422)
                {
                    ModelState.AddModelError("", "Категория уже создана");
                }
                else
                {
                    ModelState.AddModelError("", "Ошибка. Категория не создалась");
                }
            }

            return View();
        }
    }
}
