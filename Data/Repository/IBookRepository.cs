using library_management.Data.ViewModel;

namespace library_management.Data.Repository
{
    public interface IBookRepository
    {
        Task<List<BooksVM>> GetAllBooksAsync();
        Task<int> AddBooksAsync(BooksVM booksVM);
        Task<BooksVM> GetBookByIdAsync(int id);
        Task<int> UpdateBookByIdAsync(int id, BooksVM booksVM);
        Task<int> DeleteBookById(int id);

    }
}