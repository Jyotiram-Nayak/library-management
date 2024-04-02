
namespace library_management.Data.Repository
{
    public interface IBorrowRepository
    {
        Task<int> BorrowBookAsync(int bookId);
    }
}