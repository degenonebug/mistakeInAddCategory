
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ItemApiProject.Models
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(60, ErrorMessage="Название категории не может превышать более 60 символов")]
        public string Name { get; set; }
        public virtual ICollection<ItemCategory> ItemCategories { get; set; }
    }
}
