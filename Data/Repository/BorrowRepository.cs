using AutoMapper;
using library_management.Data.Model;
using library_management.Data.ViewModel;
using library_management.Services;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace library_management.Data.Repository
{
    public class BorrowRepository : IBorrowRepository
    {
        private readonly AppDbContext _context;
        private readonly IUserServices _userServices;
        private readonly IMapper _mapper;
        private readonly IBookRepository _bookRepository;
        private readonly IISBNRepository _iSBNRepository;

        public BorrowRepository(AppDbContext context,
            IUserServices userServices,
            IMapper mapper,
            IBookRepository bookRepository,
            IISBNRepository iSBNRepository)
        {
            _context = context;
            _userServices = userServices;
            _mapper = mapper;
            _bookRepository = bookRepository;
            _iSBNRepository = iSBNRepository;
        }
        public async Task<int> BorrowBookAsync(int bookId, string isbn)
        {
            var book = await _bookRepository.GetBookByIdAsync(bookId);
            if (book == null || book.AvailableCopies < 1) { return 0; }
            var isbnDetails = await _iSBNRepository.GetISBNDetailsAsync(isbn);
            if (isbnDetails == null || isbnDetails.isIssue == true) { return 0; }
            var userId = _userServices.GetUserId();
            if (userId == null) { return 0; }
            BorrowBookVM borrow = new BorrowBookVM();
            borrow.UserId = userId;
            borrow.BookId = bookId;
            borrow.ISBN = isbn;
            borrow.DueDate = DateTime.Now.AddDays(7);
            var status1 = await _context.Borrowings.AddAsync(_mapper.Map<Borrowings>(borrow));
            var result = await _context.SaveChangesAsync();
            var status2 = await _bookRepository.UpdateAvailableQty(bookId, false);
            var status3 = await _iSBNRepository.UpdateISBNAsync(isbn, userId, true);
            return result;
        }
        public async Task<List<BorrowBookVM>> FilterBorrowedBooks(int? AuthorId,int? BookId)
        {
            var query =_context.Borrowings.Include(x => x.Book).AsQueryable();

            if (AuthorId.HasValue)
            {
                query = query.Where(b => b.Book.AuthorId == AuthorId);
            }

            if (BookId.HasValue)
            {
                query = query.Where(b => b.BookId == BookId);
            }

            var borrowedBooks = await query.ToListAsync();

            return _mapper.Map<List<BorrowBookVM>>(borrowedBooks);
        }
    }
}
