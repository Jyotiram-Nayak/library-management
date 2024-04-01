using System.ComponentModel.DataAnnotations;

namespace library_management.Data.ViewModel
{
    public class AuthorsVM
    {
        [Required]
        public int AuthorId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Biography { get; set; }
    }
}
