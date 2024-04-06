using AutoMapper;
using library_management.Data.Model;
using library_management.Data.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;
using static System.Reflection.Metadata.BlobBuilder;
using library_management.Services;
using System.Net;
using System.Collections.Generic;


namespace library_management.Data.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserServices _userServices;

        public BookRepository(AppDbContext context, IMapper mapper,
            IUserServices userServices)
        {
            _context = context;
            _mapper = mapper;
            _userServices = userServices;
        }
        public async Task<List<BooksVM>> GetAllBooksAsync()
        {
            var books = await _context.Books.ToListAsync();
            return _mapper.Map<List<BooksVM>>(books);
        }
        public async Task<int> AddBooksAsync(BooksVM booksVM)
        {
            booksVM.BookId = Guid.NewGuid();
            booksVM.ISBN = Guid.NewGuid();
            booksVM.CreatedBy = _userServices.GetUserId();
            _context.Books.Add(_mapper.Map<Books>(booksVM));
            return await _context.SaveChangesAsync();
        }
        public async Task<BookResponceVM> GetBookByIdAsync(Guid? bookId)
        {
            //var books = await _context.Books.FindAsync(bookId);
            //return _mapper.Map<BooksVM>(books);
            var book = await _context.Books
            .Include(b => b.Authors)
            .Include(b => b.BookCategories).ThenInclude(bc => bc.Category)
            .FirstOrDefaultAsync(b => b.BookId == bookId);

            if (book == null)
            {
                return null;
            }

            var bookViewModel = new BookResponceVM
            {
                BookId = book.BookId,
                Title = book.Title,
                AuthorId = book.AuthorId,
                AuthorName = book.Authors.Name,
                ISBN = book.ISBN,
                PublicationDate = book.PublicationDate,
                AvailableCopies = book.AvailableCopies,
                TotalCopies = book.TotalCopies,
                CreatedDate = book.CreatedDate,
                CreatedBy = book.CreatedBy,
                UpdatedDate = book.UpdatedDate,
                UpdatedBy = book.UpdatedBy,
                Categories = book.BookCategories.Select(bc => bc.Category.Name).ToList()
            };
            return bookViewModel;
        }
        public async Task<int> UpdateBookByIdAsync(Guid id, BooksVM booksVM)
        {
            var book = await _context.Books.Where(x => x.BookId == id).FirstOrDefaultAsync();
            if (book != null)
            {
                booksVM.UpdatedDate = DateTime.Now;
                booksVM.UpdatedBy = _userServices.GetUserId();
                _mapper.Map<Books>(booksVM);
            };
            var status = await _context.SaveChangesAsync();
            return status;
        }
        public async Task<int> DeleteBookById(Guid id)
        {
            var book = _context.Books.Where(X => X.BookId == id).FirstOrDefault();
            if (book != null)
            {
                _context.Books.Remove(book);
                var status = await _context.SaveChangesAsync();
                return status;
            }
            return 0;
        }
        public async Task<int> UpdateBookQtyAsync(Guid bookId, bool isAdd)
        {
            var book = await _context.Books.FindAsync(bookId);
            if(book != null)
            {
                book.TotalCopies = book.TotalCopies + 1;
                book.AvailableCopies = book.AvailableCopies + 1;
            }
            //book.TotalCopies = isAdd == true ? book.TotalCopies + 1 : book.TotalCopies - 1;
            return await _context.SaveChangesAsync();
        }
        public async Task<int> UpdateAvailableQty(Guid bookId, bool isAdd)
        {
            var book = await _context.Books.FindAsync(bookId);
            book.AvailableCopies = isAdd == true ? book.AvailableCopies + 1 : book.AvailableCopies - 1;
            return await _context.SaveChangesAsync();
        }
        public async Task<int> AddCategoryToBookAsync(Guid bookId,Guid categoryId)
        {
            var existingEntity = await _context.BookCategorys.FirstOrDefaultAsync(bc => bc.BookId == bookId && bc.CategoryId == categoryId);
            if(existingEntity == null)
            {
                var bookCategory = new BookCategory
                {
                    Id = Guid.NewGuid(),
                    BookId = bookId,
                    CategoryId = categoryId
                };
                await _context.BookCategorys.AddAsync(bookCategory);
            }
            return await _context.SaveChangesAsync();
        }
        public async Task<List<BookResponceVM>> FilterBooksAsync(string? filterOn, string? filterString)
        {
            var query = _context.Books.Include(b => b.Authors).Include(b => b.BookCategories)
                .ThenInclude(bc => bc.Category).AsQueryable();

            if (!string.IsNullOrEmpty(filterOn) && !string.IsNullOrEmpty(filterString))
            {
                switch (filterOn.ToLower())
                {
                    case "title":
                        query = query.Where(b => b.Title.Contains(filterString));
                        break;
                    case "author":
                        query = query.Where(b => b.Authors.Name.Contains(filterString));
                        break;
                    case "category":
                        query = query.Where(b => b.BookCategories.Any(bc => bc.Category.Name.Contains(filterString)));
                        break;
                    default:
                        break;
                }
            }
            var filteredBooks = await query
            .Select(b => new BookResponceVM
            {
                BookId = b.BookId,
                Title = b.Title,
                AuthorId=b.AuthorId,
                ISBN=b.ISBN,
                AuthorName = b.Authors.Name,
                AvailableCopies = b.AvailableCopies,
                TotalCopies = b.TotalCopies,
                Categories = b.BookCategories.Select(bc => bc.Category.Name).ToList()
            })
            .ToListAsync();

            return filteredBooks;
        }
    }
}
