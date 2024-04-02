using AutoMapper;
using library_management.Data.Model;
using library_management.Data.ViewModel;

namespace library_management.Data.Repository
{
    public class ISBNRepository : IISBNRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ISBNRepository(AppDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int> AddISBN(int bookId)
        {
            BookISBNVM bookISBN = new BookISBNVM();
            bookISBN.ISBN = Guid.NewGuid().ToString();
            bookISBN.BookId = bookId;
            _context.BooksISBN.Add(_mapper.Map<BooksISBN>(bookISBN));
            return await _context.SaveChangesAsync();
        }
    }
}
