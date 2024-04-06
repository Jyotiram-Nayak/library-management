using library_management.Data.ViewModel;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace library_management.Data.Repository
{
    public interface IBookRepository
    {
        Task<List<BooksVM>> GetAllBooksAsync();
        Task<BookResponceVM> GetBookByIdAsync(Guid? bookId);
        Task<int> AddBooksAsync(BooksVM booksVM);
        Task<int> UpdateBookByIdAsync(Guid id, BooksVM booksVM);
        Task<int> DeleteBookById(Guid id);
        Task<int> UpdateBookQtyAsync(Guid bookId, bool isAdd);
        Task<int> UpdateAvailableQty(Guid bookId, bool isAdd);
        Task<int> AddCategoryToBookAsync(Guid bookId, Guid categoryId);
        //Task<List<BooksVM>> FilterBooksAsync(Guid? bookId, Guid? authorId, Guid? isbn, Guid? categoryId);
        Task<List<BookResponceVM>> FilterBooksAsync(string? filterOn, string? filterString);
    }
}