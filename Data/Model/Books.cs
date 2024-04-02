using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace library_management.Data.Model
{
    public class Books
    {
        [Key]
        [Required]
        public int BookId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public int AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public Authors Authors { get; set; }
        [Required]
        public DateTime PublicationDate { get; set; }
        [Required]
        public int CategoryID { get; set; }
        [Required]
        public int AvailableCopies { get; set; }
        [Required]
        public int TotalCopies { get; set; }
    }
}
