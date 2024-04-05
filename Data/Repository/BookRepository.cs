using AutoMapper;
using library_management.Data.Model;
using library_management.Data.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;
using static System.Reflection.Metadata.BlobBuilder;
using library_management.Services;


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
        public async Task<List<BooksVM>> GetBookByIdAsync(Guid? bookId,Guid? authorId,Guid? isbn,Guid? categoryId)
        {
            var query = _context.Books.AsQueryable();
            if (bookId.HasValue)
                query = query.Where(x => x.BookId == bookId);

            if (authorId.HasValue)
                query = query.Where(x => x.AuthorId == authorId);

            if (isbn.HasValue)
                query = query.Where(x => x.ISBN == isbn);

            if (categoryId.HasValue)
                query = query.Where(x => x.CategoryID == categoryId);

            var books = await query.ToListAsync();
            return _mapper.Map<List<BooksVM>>(books);
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
    }
}
