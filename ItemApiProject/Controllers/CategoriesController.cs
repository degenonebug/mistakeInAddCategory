using ItemApiProject.Dtos;
using ItemApiProject.Models;
using ItemApiProject.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ItemApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : Controller
    {
        private ICategoryRepository _categoryRepository;
        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        //api/categories
        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CategoryDto>))]
        public IActionResult GetCategories()
        {
            var categories = _categoryRepository.GetCategories();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoriesDto = new List<CategoryDto>();
            foreach (var category in categories)
            {
                categoriesDto.Add(new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name
                });

            }
            return Ok(categoriesDto);
        }


        //api/categories/categoryId
        [HttpGet("{categoryId}", Name = "GetCategory")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(CategoryDto))]
        public IActionResult GetCategory(int categoryId)
        {
            if (!_categoryRepository.CategoryExists(categoryId))
                return NotFound();
            var category = _categoryRepository.GetCategory(categoryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoryDto = new CategoryDto()
            {
                Id = category.Id,
                Name = category.Name
            };


            return Ok(categoryDto);
        }


        //api/categories/categoryId/items
        [HttpGet("{categoryId}/items")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CategoryDto>))]
        public IActionResult GetAllItemsForCategory(int categoryId)
        {
            if (!_categoryRepository.CategoryExists(categoryId))
                return NotFound();

            var items = _categoryRepository.GetAllItemsForCategory(categoryId);

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
                    Price = item.Price,
                    Color = item.Color,
                    Weight = item.Weight,
                    Size = item.Size
                });
            }
            return Ok(itemsDto);
        }

        //api/categories/items/itemId
        [HttpGet("items/{itemId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CategoryDto>))]
        public IActionResult GetAllCategoriesForItem(int itemId)
        {
            var categories = _categoryRepository.GetAllCategoriesForItem(itemId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoriesDto = new List<CategoryDto>();
            foreach(var category in categories)
            {
                categoriesDto.Add(new CategoryDto()
                {
                    Id = category.Id,
                    Name = category.Name
                });
            }
            return Ok(categoriesDto);
        }

        //api/categories
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Category))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult CreateCategory([FromBody] Category categoryToCreate)
        {
            if (categoryToCreate == null)
                return BadRequest(ModelState);

            var category = _categoryRepository.GetCategories()
                            .Where(c => c.Name.Trim().ToUpper() == categoryToCreate.Name.Trim().ToUpper())
                            .FirstOrDefault();
            if (category != null)
            {
                ModelState.AddModelError("", $"Категория {categoryToCreate.Name} уже существует");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);  

            if(!_categoryRepository.CreateCategory(categoryToCreate))
            {
                ModelState.AddModelError("", $"Что-то пошло не так при попытку сохранить {categoryToCreate.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetCategory", new { categoryId = categoryToCreate.Id }, categoryToCreate);
        }

        //api/categories/categoryId
        [HttpPut("{categoryId}")]
        [ProducesResponseType(204)] //no content
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult UpdateCategory(int categoryId, [FromBody]Category updatedCategoryInfo)
        {
            if (updatedCategoryInfo == null)
                return BadRequest(ModelState);

            if (categoryId != updatedCategoryInfo.Id)
                return BadRequest(ModelState);

            if (!_categoryRepository.CategoryExists(categoryId))
                return NotFound();

            if (_categoryRepository.IsDuplicateCategoryName(categoryId, updatedCategoryInfo.Name))
            {
                ModelState.AddModelError("", $"Категория {updatedCategoryInfo.Name} уже существует");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_categoryRepository.UpdateCategory(updatedCategoryInfo))
            {
                ModelState.AddModelError("", $"Что-то пошло не так с обновлением {updatedCategoryInfo.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        //api/categories/categoryId
        [HttpDelete("{categoryId}")]
        [ProducesResponseType(204)] //no content
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public IActionResult DeleteCategory(int categoryId)
        {
            if (!_categoryRepository.CategoryExists(categoryId))
                return NotFound();

            var categoryToDelete = _categoryRepository.GetCategory(categoryId);

            if(_categoryRepository.GetAllItemsForCategory(categoryId).Count() > 0)
            {
                ModelState.AddModelError("", $"Категория {categoryToDelete} " +
                                            "не может быть удалена, потому что используется по крайней мере одним товаром");
                return StatusCode(409, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if(!_categoryRepository.DeleteCategory(categoryToDelete))
            {
                ModelState.AddModelError("", $"Что-то пошло не так при удалении {categoryToDelete.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
