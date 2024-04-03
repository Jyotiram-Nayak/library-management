using library_management.Data.ViewModel;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace library_management.Data.Repository
{
    public interface IBookRepository
    {
        Task<List<BooksVM>> GetAllBooksAsync();
        Task<int> AddBooksAsync(BooksVM booksVM);
        Task<BooksVM> GetBookByIdAsync(int id);
        Task<int> UpdateBookByIdAsync(int id, BooksVM booksVM);
        Task<int> DeleteBookById(int id);
        Task<int> UpdateBookQtyAsync(int bookId, bool isAdd);
        Task<int> UpdateAvailableQty(int bookId, bool isAdd);
    }
}