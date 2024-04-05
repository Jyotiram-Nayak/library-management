using System.ComponentModel.DataAnnotations;

namespace library_management.Data.Model
{
    public class Authors
    {
        [Key]
        [Required]
        public Guid AuthorId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Biography { get; set; }
        public ICollection<Books> Books { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        [Required]
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
