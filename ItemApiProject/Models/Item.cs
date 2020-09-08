using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ItemApiProject.Models
{
    public class Item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(200, ErrorMessage = "Название товара не может превышать более 200 символов")]
        public string Title { get; set; }

        [MaxLength(2000, ErrorMessage = "Описание товара не может превышать более 3000 символов")]
        public string Description { get; set; }

        [Required]
        public double Price { get; set; }
        [Required]
        public string Color { get; set; }
        public double Weight { get; set; }
        public double Size { get; set; }

        public virtual ICollection<ItemCategory> ItemCategories { get; set; }
        public virtual ICollection<ItemManufacturer> ItemManufacturers { get; set; }
    }
}
