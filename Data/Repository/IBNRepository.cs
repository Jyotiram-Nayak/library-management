
using library_management.Data.ViewModel;

namespace library_management.Data.Repository
{
    public interface IBNRepository
    {
        Task<int> AddISBN(Guid bookId);
        Task<int> UpdateISBNAsync(string isbn, string userId, bool isIssue);
        Task<BookBNVM> GetISBNDetailsAsync(string isbn);
        Task<List<BookBNVM>> GetISBNAllAsync();
    }
}