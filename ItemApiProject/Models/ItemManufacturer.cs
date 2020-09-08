using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItemApiProject.Models
{
    public class ItemManufacturer
    {
        public int ItemId { get; set; }
        public Item Item { get; set; }

        public int ManufacturerId { get; set; }
        public Manufacturer Manufacturer { get; set; }
    }
}
