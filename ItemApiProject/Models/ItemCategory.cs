using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItemApiProject.Models
{
    public class ItemCategory
    {
        public int ItemId { get; set; }
        public Item Item { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
