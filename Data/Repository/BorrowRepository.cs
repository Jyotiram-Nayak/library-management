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
        public async Task<int> BorrowBookAsync(int bookId)
        {
            var userId = _userServices.GetUserId();
            if (userId == null) { return 0; }
            BorrowBookVM borrow = new BorrowBookVM();
            borrow.UserId = userId;
            borrow.BookId = bookId;
            borrow.DueDate = DateTime.Now.AddDays(7);
            await _context.Borrowings.AddAsync(_mapper.Map<Borrowings>(borrow));
            return await _context.SaveChangesAsync();
        }
    }
}
