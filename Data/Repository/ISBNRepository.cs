using AutoMapper;
using library_management.Data.Model;
using library_management.Data.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace library_management.Data.Repository
{
    public class ISBNRepository : IISBNRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IBookRepository _bookRepository;

        public ISBNRepository(AppDbContext context,
            IMapper mapper,
            IBookRepository bookRepository)
        {
            _context = context;
            _mapper = mapper;
            this._bookRepository = bookRepository;
        }
        public async Task<int> AddISBN(int bookId)
        {
            BookISBNVM bookISBN = new BookISBNVM();
            bookISBN.ISBN = Guid.NewGuid().ToString();
            bookISBN.BookId = bookId;
            _context.BooksISBN.Add(_mapper.Map<BooksISBN>(bookISBN));
            var result = await _context.SaveChangesAsync();
            await _bookRepository.UpdateBookQtyAsync(bookId, true);
            return result;
        }
        public async Task<int> UpdateISBNAsync(string isbnno, string userId, bool isIssue)
        {
            var bookISBN = await _context.BooksISBN.Where(x => x.ISBN == isbnno).FirstOrDefaultAsync();
            bookISBN.UserId = userId;
            bookISBN.isIssue = isIssue;
            var result = await _context.SaveChangesAsync();
            return result;

        }
        public async Task<BookISBNVM> GetISBNDetailsAsync(string isbn)
        {
            var isbndetails = await _context.BooksISBN.Where(x => x.ISBN == isbn).Select(x => new BookISBNVM
            {
                Id = x.Id,
                UserId = x.UserId,
                BookId = x.BookId,
                isIssue = x.isIssue,
                ISBN = x.ISBN
            }).FirstOrDefaultAsync();
            return isbndetails;
            //return _mapper.Map<BookISBNVM>(isbndetails);
        }
        public async Task<List<BookISBNVM>> GetISBNAllAsync()
        {
            var isbndetails = await _context.BooksISBN.ToListAsync();
            return _mapper.Map<List<BookISBNVM>>(isbndetails);
        }
    }
}
