using ItemApiProject.Dtos;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItemGUI.ViewModels
{
    public class CreateItemViewModel
    {
        public ItemDto Item { get; set; }

        

        public List<int> CategoryIds { get; set; }
        public List<SelectListItem> CategorySelectListItems { get; set; }

        public List<int> ManufacturerIds { get; set; }
        public List<SelectListItem> ManufacturerSelectListItems { get; set; }
    }
}
