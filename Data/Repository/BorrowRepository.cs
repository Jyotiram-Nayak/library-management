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

        public BorrowRepository(AppDbContext context,
            IUserServices userServices,
            IMapper mapper,
            IBookRepository bookRepository)
        {
            _context = context;
            _userServices = userServices;
            _mapper = mapper;
            _bookRepository = bookRepository;
        }
        public async Task<int> BorrowBookAsync(Guid bookId, Guid isbn)
        {
            var book = await _bookRepository.GetBookByIdAsync(bookId);
            if (book == null || book.AvailableCopies < 1) { return 0; }
            var userId = _userServices.GetUserId();
            if (userId == null) { return 0; }
            BorrowBookVM borrow = new BorrowBookVM();
            borrow.UserId = userId;
            borrow.BookId = book.BookId;
            borrow.ISBN = book.ISBN;
            borrow.DueDate = DateTime.Now.AddDays(7);
            var status1 = await _context.Borrowings.AddAsync(_mapper.Map<Borrowings>(borrow));
            var result = await _context.SaveChangesAsync();
            var status2 = await _bookRepository.UpdateAvailableQty(bookId, false);
            return result;
        }

        public async Task<int> ReturnBookAsync(Guid bookId)
        {
            var borrow = await _context.Borrowings.FirstOrDefaultAsync(b => b.BookId == bookId);
            if (borrow != null && borrow?.ReturnDate == null) 
            { 
                borrow.ReturnDate = DateTime.Now;
            }
            var result =await _context.SaveChangesAsync();
            if(result != 0) 
            { 
                var status2 = await _bookRepository.UpdateAvailableQty(bookId, true);
            }
            return result;
        }
    }
}
