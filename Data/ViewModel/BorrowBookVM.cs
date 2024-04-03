using System.ComponentModel.DataAnnotations;

namespace library_management.Data.ViewModel
{
    public class BorrowBookVM
    {
        [Required]
        public int BorrowId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public int BookId { get; set; }
        [Required]
        public string ISBN { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime BorrowDate { get; set; } = DateTime.Now;
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DueDate { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ReturnDate { get; set; }
    }
}
