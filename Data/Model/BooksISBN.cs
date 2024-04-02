using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace library_management.Data.Model
{
    public class BooksISBN
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string ISBN { get; set; }

        [ForeignKey("AspNetUsers")]
        public string? UserId { get; set; }
        [Required]
        [ForeignKey("Books")]
        public int BookId { get; set; }
        [Required]
        public bool isIssue { get; set; } = false;
    }
}
