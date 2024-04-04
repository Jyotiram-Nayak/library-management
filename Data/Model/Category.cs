using System.ComponentModel.DataAnnotations;

namespace library_management.Data.Model
{
    public class Category
    {
        [Key]
        [Required] 
        public int Id { get; set; }
        [Required] 
        public string Name { get; set; }
        [Required]
        public bool IsActive { get; set; } = true;
    }
}
