using System.ComponentModel.DataAnnotations;

namespace library_management.Data.ViewModel
{
    public class AuthorsVM
    {
        public Guid AuthorId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Biography { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        
    }
}
