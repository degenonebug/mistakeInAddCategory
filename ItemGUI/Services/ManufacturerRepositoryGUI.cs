using ItemApiProject.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ItemGUI.Services
{
    public class ManufacturerRepositoryGUI : IManufacturerRepositoryGUI
    {
        public IEnumerable<ItemDto> GetItemsForManufacturer(int manufacturerId)
        {
            IEnumerable<ItemDto> items = new List<ItemDto>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:62352/api/");

                var response = client.GetAsync($"manufacturers/{manufacturerId}/items");
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

        public ManufacturerDto GetManufacturerById(int manufacturerId)
        {
            ManufacturerDto manufacturer = new ManufacturerDto();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:62352/api/");

                var response = client.GetAsync($"manufacturers/{manufacturerId}");
                response.Wait();

                var result = response.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<ManufacturerDto>();
                    readTask.Wait();

                    manufacturer = readTask.Result;
                }
            }
            return manufacturer;
        }

        public IEnumerable<ManufacturerDto> GetAllManufacturersForItem(int itemId)
        {
            IEnumerable<ManufacturerDto> manufacturers = new List<ManufacturerDto>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:62352/api/");

                var response = client.GetAsync($"manufacturers/items/{itemId}");
                response.Wait();

                var result = response.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<ManufacturerDto>>();
                    readTask.Wait();

                    manufacturers = readTask.Result;
                }
            }
            return manufacturers;
        }

        public IEnumerable<ManufacturerDto> GetManufacturers()
        {
            IEnumerable<ManufacturerDto> manufacturers = new List<ManufacturerDto>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:62352/api/");

                var response = client.GetAsync("manufacturers");
                response.Wait();

                var result = response.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<ManufacturerDto>>();
                    readTask.Wait();

                    manufacturers = readTask.Result;
                }
            }
            return manufacturers;
        }
    }
}
