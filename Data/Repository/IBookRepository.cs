using library_management.Data.ViewModel;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace library_management.Data.Repository
{
    public interface IBookRepository
    {
        Task<List<BooksVM>> GetAllBooksAsync();
        Task<int> AddBooksAsync(BooksVM booksVM);
        Task<List<BooksVM>> GetBookByIdAsync(Guid? bookId, Guid? authorId, Guid? isbn, Guid? categoryId);
        Task<int> UpdateBookByIdAsync(Guid id, BooksVM booksVM);
        Task<int> DeleteBookById(Guid id);
        Task<int> UpdateBookQtyAsync(Guid bookId, bool isAdd);
        Task<int> UpdateAvailableQty(Guid bookId, bool isAdd);
    }
}