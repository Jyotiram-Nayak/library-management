
using library_management.Data.ViewModel;

namespace library_management.Data.Repository
{
    public interface IBorrowRepository
    {
        Task<int> BorrowBookAsync(Guid bookId, string isbn);
        Task<List<BorrowBookVM>> FilterBorrowedBooks(Guid? AuthorId, Guid? BookId);
    }
}