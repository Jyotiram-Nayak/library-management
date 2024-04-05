using AutoMapper;
using library_management.Data.Model;
using library_management.Data.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace library_management.Data.Repository
{
    public class BNRepository : IBNRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IBookRepository _bookRepository;

        public BNRepository(AppDbContext context,
            IMapper mapper,
            IBookRepository bookRepository)
        {
            _context = context;
            _mapper = mapper;
            this._bookRepository = bookRepository;
        }
        public async Task<int> AddISBN(Guid bookId)
        {
            BookBNVM bookISBN = new BookBNVM();
            bookISBN.ISBN = Guid.NewGuid().ToString();
            bookISBN.BookId = bookId;
            _context.BooksBN.Add(_mapper.Map<BooksBN>(bookISBN));
            var result = await _context.SaveChangesAsync();
            await _bookRepository.UpdateBookQtyAsync(bookId, true);
            return result;
        }
        public async Task<int> UpdateISBNAsync(string isbnno, string userId, bool isIssue)
        {
            var bookISBN = await _context.BooksBN.Where(x => x.ISBN == isbnno).FirstOrDefaultAsync();
            bookISBN.UserId = userId;
            bookISBN.isIssue = isIssue;
            var result = await _context.SaveChangesAsync();
            return result;

        }
        public async Task<BookBNVM> GetISBNDetailsAsync(string isbn)
        {
            var isbndetails = await _context.BooksBN.Where(x => x.ISBN == isbn).Select(x => new BookBNVM
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
        public async Task<List<BookBNVM>> GetISBNAllAsync()
        {
            var isbndetails = await _context.BooksBN.ToListAsync();
            return _mapper.Map<List<BookBNVM>>(isbndetails);
        }
    }
}
