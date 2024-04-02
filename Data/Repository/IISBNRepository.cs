
namespace library_management.Data.Repository
{
    public interface IISBNRepository
    {
        Task<int> AddISBN(int bookId);
    }
}