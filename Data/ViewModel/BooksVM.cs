using System.ComponentModel.DataAnnotations;

namespace library_management.Data.ViewModel
{
    public class BooksVM
    {
        [Required]
        public int BookId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public int AuthorId { get; set; }
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
