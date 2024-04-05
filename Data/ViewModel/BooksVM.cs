using library_management.Services;
using System.ComponentModel.DataAnnotations;

namespace library_management.Data.ViewModel
{
    public class BooksVM
    {
        public Guid BookId { get; set; } 
        [Required]
        public string Title { get; set; }
        [Required]
        public Guid AuthorId { get; set; }
        [Required]
        public Guid ISBN { get; set; }
        [Required]
        public DateTime PublicationDate { get; set; }
        [Required]
        public Guid CategoryID { get; set; }
        [Required]
        public int AvailableCopies { get; set; }
        [Required]
        public int TotalCopies { get; set; }
        public DateTime CreatedDate { get; set; }=DateTime.Now;
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
