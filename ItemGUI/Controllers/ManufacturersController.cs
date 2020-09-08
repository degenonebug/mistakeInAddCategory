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
    public class ManufacturersController : Controller
    {
        IManufacturerRepositoryGUI _manufacturerRepository;
        public ManufacturersController(IManufacturerRepositoryGUI manufacturerRepository)
        {
            _manufacturerRepository = manufacturerRepository;
        }

        public IActionResult Index()
        {
            var manufacturers = _manufacturerRepository.GetManufacturers();

            if(manufacturers.Count() <= 0)
            {
                ViewBag.Message = "Проблема с извлечением производителей с базы данных или производитель не существует";
            }

            return View(manufacturers);
        }

        public IActionResult GetManufacturerById(int manufacturerId)
        {
            var manufacturer = _manufacturerRepository.GetManufacturerById(manufacturerId);

            if (manufacturer == null)
            {
                ModelState.AddModelError("", "Ошибка при получении производителя");
                ViewBag.Message = $"Проблема с извлечением производителя с Id {manufacturerId} " + 
                    $"из базы данных (возможно такой производитель не существует)";
                manufacturer = new ManufacturerDto();
            }

            var items = _manufacturerRepository.GetItemsForManufacturer(manufacturerId);
            if (items.Count() <= 0)
            {
                ViewBag.ItemMessage = $"У { manufacturer.Name} производителя нету товаров";
            }

            var itemManufacturerViewModel = new ManufacturerItemsViewModel()
            {
                Manufacturer = manufacturer,
                Items = items
            };

            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            return View(itemManufacturerViewModel);
        }

        //[HttpGet]
        //public IActionResult CreateManufacturer()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public IActionResult CreateManufacturer(Manufacturer manufacturer)
        //{
        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri("http://localhost:62352/api/");
        //        var responseTask = client.PostAsJsonAsync("manufacturers", manufacturer);
        //        responseTask.Wait();

        //        var result = responseTask.Result;
        //        if (result.IsSuccessStatusCode)
        //        {
        //            var newManufacturerTask = result.Content.ReadAsAsync<Manufacturer>();
        //            newManufacturerTask.Wait();

        //            var newManufacturer = newManufacturerTask.Result;
        //            TempData["SuccessMessage"] = $"Производитель {newManufacturer.Name} был успешно создан";

        //            return RedirectToAction("GetManufacturerById", new { manufacturerId = newManufacturer.Id });

        //        }

        //        if ((int)result.StatusCode == 422)
        //        {
        //            ModelState.AddModelError("", "Категория уже создана");
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", "Ошибка. Категория не создалась");
        //        }
        //    }

        //    return View();
        //}
    }
}
