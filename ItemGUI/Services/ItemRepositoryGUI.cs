using ItemApiProject.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ItemGUI.Services
{
    public class ItemRepositoryGUI : IItemRepositoryGUI
    {
        public ItemDto GetItemById(int itemId)
        {
            ItemDto item = new ItemDto();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:62352/api/");

                var response = client.GetAsync($"items/{itemId}");
                response.Wait();

                var result = response.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<ItemDto>();
                    readTask.Wait();

                    item = readTask.Result;
                }
            }
            return item;
        }
    

        public IEnumerable<ItemDto> GetItems()
        {
            IEnumerable<ItemDto> items = new List<ItemDto>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:62352/api/");

                var response = client.GetAsync("items");
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
    }
}
