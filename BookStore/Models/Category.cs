using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required (ErrorMessage = "Name is required")]
        [MaxLength(40)]
        public string Name { get; set; }

        [Range(1,100)]
        public int DisplayOrder { get; set; }
    }
}
