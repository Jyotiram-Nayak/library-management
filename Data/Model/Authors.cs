using System.ComponentModel.DataAnnotations;

namespace library_management.Data.Model
{
    public class Authors
    {
        [Key]
        [Required]
        public int AuthorId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Biography { get; set; }
        //public ICollection<Books> Books { get; set; }
    }
}
