using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace library_management.Data.Model
{
    public class Borrowings
    {
        [Key]
        public int BorrowId { get; set; }
        [Required]
        [ForeignKey("AspNetUsers")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        [Required]
        [ForeignKey("Books")]
        public int BookId { get; set; }
        public Books Book { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public DateTime BorrowDate { get; set; } 
        [Required]
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
