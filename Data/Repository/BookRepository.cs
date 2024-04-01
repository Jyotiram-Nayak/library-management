using AutoMapper;
using library_management.Data.Model;
using library_management.Data.ViewModel;
using Microsoft.EntityFrameworkCore;
using static System.Reflection.Metadata.BlobBuilder;

namespace library_management.Data.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public BookRepository(AppDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<BooksVM>> GetAllBooksAsync()
        {
            var books = await _context.Books.ToListAsync();
            return _mapper.Map<List<BooksVM>>(books);
        }
        public async Task<int> AddBooksAsync(BooksVM booksVM)
        {
            _context.Books.Add(_mapper.Map<Books>(booksVM));
            return await _context.SaveChangesAsync();
        }
        public async Task<BooksVM> GetBookByIdAsync(int id)
        {
            var books =await _context.Books.FindAsync(id);
            return _mapper.Map<BooksVM>(books);
        }
        public async Task<int> UpdateBookByIdAsync(int id,BooksVM booksVM)
        {
            var book = await _context.Books.Where(x=>x.BookId == id).FirstOrDefaultAsync();
            if (book != null)
            {
                _mapper.Map<Books>(booksVM);
            };
            var status = await _context.SaveChangesAsync();
            return status;
        }
        public async Task<int> DeleteBookById(int id)
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
    }
}
