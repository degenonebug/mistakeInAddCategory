using ItemApiProject.Dtos;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItemGUI.Components
{
    public class ManufacturersList
    {
        private List<ManufacturerDto> _allManufacturers = new List<ManufacturerDto>();

        public ManufacturersList(List<ManufacturerDto> allManufacturers)
        {
            _allManufacturers = allManufacturers;
        }

        public List<SelectListItem> GetManufacturersList()
        {
            var items = new List<SelectListItem>();
            foreach (var manufacturer in _allManufacturers)
            {
                items.Add(new SelectListItem
                {
                    Text = manufacturer.Name,
                    Value = manufacturer.Id.ToString(),
                    Selected = false
                });
            }

            return items;
        }

        public List<SelectListItem> GetManufacturersList(List<int> selectedManufacturers)
        {
            var items = new List<SelectListItem>();
            foreach (var manufacturer in _allManufacturers)
            {
                items.Add(new SelectListItem
                {
                    Text = manufacturer.Name,
                    Value = manufacturer.Id.ToString(),
                    Selected = selectedManufacturers.Contains(manufacturer.Id) ? true : false
                });
            }

            return items;
        }
    }

}
