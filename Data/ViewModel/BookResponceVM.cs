using library_management.Data.Model;
using System.ComponentModel.DataAnnotations;

namespace library_management.Data.ViewModel
{
    public class BookResponceVM
    {
        public Guid BookId { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public Guid AuthorId { get; set; }

        public Guid ISBN { get; set; }
        public int AvailableCopies { get; set; }
        public int TotalCopies { get; set; }
        public DateTime PublicationDate { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string? CreatedBy { get; set; } = null;
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public List<string> Categories { get; set; }
    }
}
