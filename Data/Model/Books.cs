using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace library_management.Data.Model
{
    public class Books
    {
        [Key]
        [Required]
        public Guid BookId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public Guid AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public Authors Authors { get; set; }
        [Required]
        public Guid ISBN { get; set; }
        [Required]
        public DateTime PublicationDate { get; set; }
        [Required]
        public Guid CategoryID { get; set; }
        [ForeignKey("CategoryID")]
        public Category Category { get; set; }
        [Required]
        public int AvailableCopies { get; set; }
        [Required]
        public int TotalCopies { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
