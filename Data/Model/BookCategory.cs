using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace library_management.Data.Model
{
    public class BookCategory
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("BookId")]
        public Books Book { get; set; }
        public Guid BookId { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        public Guid CategoryId { get; set; }
    }
}
