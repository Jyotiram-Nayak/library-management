using AutoMapper;
using library_management.Data.Model;
using library_management.Data.ViewModel;
using library_management.Services;

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
            if (book == null) { return 0; }
            var isbnDetails = await _iSBNRepository.GetISBNDetailsAsync(isbn);
            if (isbnDetails == null) { return 0; }
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
    }
}
