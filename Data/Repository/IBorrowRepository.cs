
using library_management.Data.ViewModel;

namespace library_management.Data.Repository
{
    public interface IBorrowRepository
    {
        Task<int> BorrowBookAsync(int bookId, string isbn);
        Task<List<BorrowBookVM>> FilterBorrowedBooks(int? AuthorId, int? BookId);
    }
}