using ItemApiProject.Dtos;
using ItemApiProject.Models;
using ItemApiProject.Services;
using ItemGUI.Components;
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
    public class HomeController : Controller
    {
        IItemRepositoryGUI _itemRepository;
        ICategoryRepositoryGUI _categoryRepository;
        IManufacturerRepositoryGUI _manufacturerRepository;

        public HomeController(IItemRepositoryGUI itemRepository, ICategoryRepositoryGUI categoryRepository,
                                IManufacturerRepositoryGUI manufacturerRepository)
        {
            _itemRepository = itemRepository;
            _categoryRepository = categoryRepository;
            _manufacturerRepository = manufacturerRepository;
        }

        public IActionResult Index()
        {
            var items = _itemRepository.GetItems();

            if (items.Count() <= 0)
            {
                ViewBag.Message = "Проблема с извлечением товаров из базы данных или товаров нету";
            }

            var itemCategoriesManufacturersViewModel = new List<ItemCategoriesManufacturersViewModel>();

            foreach (var i in items)
            {
                var categories = _categoryRepository.GetAllCategoriesForItem(i.Id).ToList();

                if (categories.Count() <= 0)
                {
                    ModelState.AddModelError("", "Ошибка при получении категории");
                }
                var manufacturers = _manufacturerRepository.GetAllManufacturersForItem(i.Id).ToList();

                if (categories.Count() <= 0)
                {
                    ModelState.AddModelError("", "Ошибка при получении производителей");
                }

                itemCategoriesManufacturersViewModel.Add(new ItemCategoriesManufacturersViewModel
                {
                    Item = i,
                    Categories = categories,
                    Manufacturers = manufacturers
                });

            }

            return View(itemCategoriesManufacturersViewModel);
        }

        public IActionResult GetItemById(int itemId)
        {
            var completeItemViewModel = new CompleteItemViewModel();
            var item = _itemRepository.GetItemById(itemId);
            if (item == null)
            {
                ModelState.AddModelError("", "Ошика при получении товара");
                item = new ItemDto();
            }
            var categories = _categoryRepository.GetAllCategoriesForItem(itemId);
            if (categories.Count() <= 0)
            {
                ModelState.AddModelError("", "Ошибка при получении категории");

            }
            var manufacturers = _manufacturerRepository.GetAllManufacturersForItem(itemId);
            if (manufacturers.Count() <= 0)
            {
                ModelState.AddModelError("", "Ошибка при получении производителей");

            }
            completeItemViewModel.Item = item;
            completeItemViewModel.Categories = categories;
            completeItemViewModel.Manufacturers = manufacturers;

            if (!ModelState.IsValid)
            {
                ViewBag.ItemMessage = "Ошибка при получении полноценного досье товара";
            }
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            return View(completeItemViewModel);
        }


        [HttpGet]
        public IActionResult CreateItem()
        {
            var manufacturers = _manufacturerRepository.GetManufacturers();
            var categories = _categoryRepository.GetCategories();



            var manufacturerList = new ManufacturersList(manufacturers.ToList());
            var categoryList = new CategoriesList(categories.ToList());

            var createUItem = new CreateItemViewModel
            {
                ManufacturerSelectListItems = manufacturerList.GetManufacturersList(),
                CategorySelectListItems = categoryList.GetCategoriesList()
            };

            return View(createUItem);

        }

        [HttpPost]
        public IActionResult CreateItem(IEnumerable<int> CategoryIds, IEnumerable<int> ManufacturerIds,
            CreateItemViewModel itemToCreate)
        {
            using (var client = new HttpClient())
            {
                var item = new Item()
                {
                    Id = itemToCreate.Item.Id,
                    Description = itemToCreate.Item.Description,
                    Title = itemToCreate.Item.Title,
                    Price = itemToCreate.Item.Price,
                    Color = itemToCreate.Item.Color,
                    Weight = itemToCreate.Item.Weight,
                    Size = itemToCreate.Item.Size
                    

                    //books?authId=?&authId=?&catId=?
                };

                var uriParameters = GetManufacturersCategoriesUri(ManufacturerIds.ToList(), CategoryIds.ToList());

                client.BaseAddress = new Uri("http://localhost:62352/api/");
                var responseTask = client.PostAsJsonAsync($"items?{uriParameters}", item);
                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTaskNewItem = result.Content.ReadAsAsync<Item>();
                    readTaskNewItem.Wait();

                    var newItem = readTaskNewItem.Result;

                    TempData["SuccessMessage"] = $"Item {item.Title} was successfully created.";
                    return RedirectToAction("GetItemById", new { itemId = newItem.Id });
                }

                
            }

            var categoryList = new CategoriesList(_categoryRepository.GetCategories().ToList());
            var manufacturerList = new ManufacturersList(_manufacturerRepository.GetManufacturers().ToList());
            itemToCreate.CategorySelectListItems = categoryList.GetCategoriesList(CategoryIds.ToList());
            itemToCreate.ManufacturerSelectListItems = manufacturerList.GetManufacturersList(ManufacturerIds.ToList());
            itemToCreate.CategoryIds = CategoryIds.ToList();
            itemToCreate.ManufacturerIds = ManufacturerIds.ToList();

            return View(itemToCreate);
        }

        [HttpGet]
        public IActionResult DeleteItem(int itemId)
        {
            var itemDto = _itemRepository.GetItemById(itemId);

            return View(itemDto);
        }



        [HttpPost]
        public IActionResult DeleteItem(int itemId, string itemTitle)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:60039/api/");
                var responseTask = client.DeleteAsync($"items/{itemId}");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = $"Item {itemTitle} was successfully deleted.";

                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", "Some kind of error. Item not deleted!");
            }

            var itemDto = _itemRepository.GetItemById(itemId);
            return View(itemDto);
        }

        private string GetManufacturersCategoriesUri(List<int> categoryIds, List<int> manufacturerIds)
        {
            var uri = "";
            foreach (var categoryId in categoryIds)
            {
                uri += $"catId={categoryId}&";
            }

            foreach (var manufacturerId in manufacturerIds)
            {
                uri += $"manufacturerId={manufacturerId}&";
            }

            return uri;
        }
    }
}
