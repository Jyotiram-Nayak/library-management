
using library_management.Data.ViewModel;

namespace library_management.Data.Repository
{
    public interface IISBNRepository
    {
        Task<int> AddISBN(int bookId);
        Task<int> UpdateISBNAsync(string isbn, string userId, bool isIssue);
        Task<BookISBNVM> GetISBNDetailsAsync(string isbn);
        Task<List<BookISBNVM>> GetISBNAllAsync();
    }
}