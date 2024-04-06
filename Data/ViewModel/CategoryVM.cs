using library_management.Data.Model;
using System.ComponentModel.DataAnnotations;

namespace library_management.Data.ViewModel
{
    public class CategoryVM
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
