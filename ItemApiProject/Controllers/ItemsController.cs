using ItemApiProject.Dtos;
using ItemApiProject.Models;
using ItemApiProject.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ItemApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : Controller
    {
        private IItemRepository _itemRepository;
        private IManufacturerRepository _manufacturerRepository;
        private ICategoryRepository _categoryRepository;
        public ItemsController(IItemRepository itemRepository, IManufacturerRepository manufacturerRepository,
            ICategoryRepository categoryRepository)
        {
            _itemRepository = itemRepository;
            _manufacturerRepository = manufacturerRepository;
            _categoryRepository = categoryRepository;
        }

        //api/items
        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ItemDto>))]
        public IActionResult GetItems()
        {
            var items = _itemRepository.GetItems();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var itemsDto = new List<ItemDto>();
            foreach (var item in items)
            {
                itemsDto.Add(new ItemDto
                {
                    Id = item.Id,
                    Title = item.Title,
                    Description = item.Description,
                    Color = item.Color,
                    Price = item.Price,
                    Weight = item.Weight,
                    Size = item.Size
                });

            }
            return Ok(itemsDto);
        }

        //api/items/itemId
        [HttpGet("{itemId}", Name = "GetItem")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(ItemDto))]
        public IActionResult GetItem(int itemId)
        {
            if (!_itemRepository.ItemExists(itemId))
                return NotFound();
            var item = _itemRepository.GetItem(itemId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var itemDto = new ItemDto()
            {
                Id = item.Id,
                Title = item.Title,
                Description = item.Description,
                Color = item.Color,
                Price = item.Price,
                Weight = item.Weight,
                Size = item.Size
            };

            return Ok(itemDto);
        }

        //api/Items?manuId=1&manuId=2&catId=1&catId=2
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Item))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult CreateItem([FromQuery] List<int> manuId, [FromQuery] List<int> catId,
                                        [FromBody] Item itemToCreate)
        {
            var statusCode = ValidateItem(manuId, catId, itemToCreate);

            if (!ModelState.IsValid)
                return StatusCode(statusCode.StatusCode);

            if (!_itemRepository.CreateItem(manuId, catId, itemToCreate))
            {
                ModelState.AddModelError("", $"Something went wrong saving the Item " +
                                            $"{itemToCreate.Title}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetItem", new { ItemId = itemToCreate.Id }, itemToCreate);
        }


        private StatusCodeResult ValidateItem(List<int> manuId, List<int> catId, Item item)
        {
            if (item == null || manuId.Count() <= 0 || catId.Count() <= 0) 
            {
                ModelState.AddModelError("", "Не включен в список: товар, производитель, или категория");
                return BadRequest();
            }

            foreach(var id in manuId)
            {
                if(!_manufacturerRepository.ManufacturerExists(id))
                {
                    ModelState.AddModelError("", "Производитель не найден");
                    return StatusCode(404);
                }
            }

            foreach (var id in catId)
            {
                if (!_categoryRepository.CategoryExists(id))
                {
                    ModelState.AddModelError("", "Категория не найдена");
                    return StatusCode(404);
                }
            }

            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Критическая ошибка");
                return BadRequest();
            }

            return NoContent();
        }

        //api/items/itemId
        [HttpDelete("{itemId}")]
        [ProducesResponseType(204)] //no content
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public IActionResult DeleteItem(int itemId)
        {
            if (!_itemRepository.ItemExists(itemId))
                return NotFound();

            var itemToDelete = _itemRepository.GetItem(itemId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_itemRepository.DeleteItem(itemToDelete))
            {
                ModelState.AddModelError("", $"Что-то пошло не так при удалении {itemToDelete.Title}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

    }
}
