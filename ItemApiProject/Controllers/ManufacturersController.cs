using ItemApiProject.Dtos;
using ItemApiProject.Models;
using ItemApiProject.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItemApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManufacturersController : Controller
    {
        private IManufacturerRepository _manufacturerRepository;
        public ManufacturersController(IManufacturerRepository manufacturerRepository)
        {
            _manufacturerRepository = manufacturerRepository;
        }

        //api/manufacturers
        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ManufacturerDto>))]
        public IActionResult GetManufacturers()
        {
            var manufacturers = _manufacturerRepository.GetManufacturers();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var manufacturersDto = new List<ManufacturerDto>();
            foreach (var manufacturer in manufacturers)
            {
                manufacturersDto.Add(new ManufacturerDto
                {
                    Id = manufacturer.Id,
                    Name = manufacturer.Name
                });

            }
            return Ok(manufacturersDto);
        }

        //api/manufacturers/manufacturerId
        [HttpGet("{manufacturerId}", Name = "GetManufacturer")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(ManufacturerDto))]
        public IActionResult GetManufacturer(int manufacturerId)
        {
            if (!_manufacturerRepository.ManufacturerExists(manufacturerId))
                return NotFound();
            var manufacturer = _manufacturerRepository.GetManufacturer(manufacturerId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var manufacturerDto = new ManufacturerDto()
            {
                Id = manufacturer.Id,
                Name = manufacturer.Name
            };

            return Ok(manufacturerDto);
        }


        //api/manufacturers/manufacturerId/items
        [HttpGet("{manufacturerId}/items")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ManufacturerDto>))]
        public IActionResult GetAllItemsForManufacturer(int manufacturerId)
        {
            if (!_manufacturerRepository.ManufacturerExists(manufacturerId))
                return NotFound();

            var items = _manufacturerRepository.GetAllItemsForManufacturer(manufacturerId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var itemsDto = new List<ItemDto>();

            foreach(var item in items)
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

        //api/manufacturers/items/itemId
        [HttpGet("items/{itemId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ManufacturerDto>))]
        public IActionResult GetAllManufacturersForItem(int itemId)
        {
            var manufacturers = _manufacturerRepository.GetAllManufacturersForItem(itemId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var manufacturersDto = new List<ManufacturerDto>();
            foreach (var manufacturer in manufacturers)
            {
                manufacturersDto.Add(new ManufacturerDto()
                {
                    Id = manufacturer.Id,
                    Name = manufacturer.Name
                });
            }
            return Ok(manufacturersDto);
        }

        //api/manufacturers
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Manufacturer))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult CreateManufacturer([FromBody] Manufacturer manufacturerToCreate)
        {
            if (manufacturerToCreate == null)
                return BadRequest(ModelState);

            var manufacturer = _manufacturerRepository.GetManufacturers()
                            .Where(c => c.Name.Trim().ToUpper() == manufacturerToCreate.Name.Trim().ToUpper())
                            .FirstOrDefault();
            if (manufacturer != null)
            {
                ModelState.AddModelError("", $"Производитель {manufacturerToCreate.Name} уже существует");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_manufacturerRepository.CreateManufacturer(manufacturerToCreate))
            {
                ModelState.AddModelError("", $"Что-то пошло не так при попытку сохранить {manufacturerToCreate.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetManufacturer", new { manufacturerId = manufacturerToCreate.Id }, manufacturerToCreate);
        }


        //api/manufacturers/manufacturerId
        [HttpPut("{manufacturerId}")]
        [ProducesResponseType(204)] //no content
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult UpdateManufacturer(int manufacturerId, [FromBody] Manufacturer updatedManufacturerInfo)
        {
            if (updatedManufacturerInfo == null)
                return BadRequest(ModelState);

            if (manufacturerId != updatedManufacturerInfo.Id)
                return BadRequest(ModelState);

            if (!_manufacturerRepository.ManufacturerExists(manufacturerId))
                return NotFound();

            if (_manufacturerRepository.IsDuplicateManufacturerName(manufacturerId, updatedManufacturerInfo.Name))
            {
                ModelState.AddModelError("", $"Производитель {updatedManufacturerInfo.Name} уже существует");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_manufacturerRepository.UpdateManufacturer(updatedManufacturerInfo))
            {
                ModelState.AddModelError("", $"Что-то пошло не так с обновлением {updatedManufacturerInfo.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }


        //api/manufacturers/manufacturerId
        [HttpDelete("{manufacturerId}")]
        [ProducesResponseType(204)] //no content
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public IActionResult DeleteManufacturer(int manufacturerId)
        {
            if (!_manufacturerRepository.ManufacturerExists(manufacturerId))
                return NotFound();

            var manufacturerToDelete = _manufacturerRepository.GetManufacturer(manufacturerId);

            if (_manufacturerRepository.GetAllItemsForManufacturer(manufacturerId).Count() > 0)
            {
                ModelState.AddModelError("", $"Производитель {manufacturerToDelete} " +
                                            "не может быть удален, потому что используется по крайней мере одним товаром");
                return StatusCode(409, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_manufacturerRepository.DeleteManufacturer(manufacturerToDelete))
            {
                ModelState.AddModelError("", $"Что-то пошло не так при удалении {manufacturerToDelete.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }



    }
}
