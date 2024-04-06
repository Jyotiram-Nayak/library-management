
using library_management.Data.ViewModel;

namespace library_management.Data.Repository
{
    public interface IBorrowRepository
    {
        Task<int> BorrowBookAsync(Guid bookId, Guid isbn);
        Task<int> ReturnBookAsync(Guid bookId);
    }
}