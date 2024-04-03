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

        [ForeignKey("ApplicationUser")]
        public string? UserId { get; set; }
        public ApplicationUser User { get; set; }
        [Required]
        [ForeignKey("Books")]
        public int BookId { get; set; }
        public Books Book { get; set; }
        [Required]
        public bool isIssue { get; set; } = false;
    }
}
