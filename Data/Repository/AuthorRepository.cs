using AutoMapper;
using library_management.Data.Model;
using library_management.Data.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace library_management.Data.Repository
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public AuthorRepository(AppDbContext context, UserManager<ApplicationUser> userManager,
            IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<List<AuthorsVM>> GetAllAuthorsAsync()
        {
            var authors = await _context.Authors.ToListAsync();
            return _mapper.Map<List<AuthorsVM>>(authors);
        }
        public async Task<AuthorsVM> GetAuthorByIdAsync(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            return _mapper.Map<AuthorsVM>(author);
        }
        public async Task<int> AddAuthorAsync(AuthorsVM authorsVM)
        {
            var author = _mapper.Map<Authors>(authorsVM);
            _context.Authors.Add(author);
            return await _context.SaveChangesAsync();
        }
        public async Task<int> UpdateAuthorAsync(int id,AuthorsVM authorsVM)
        {
            var author = await _context.Authors.FindAsync(id);
            _mapper.Map<Authors>(author);
            return await _context.SaveChangesAsync();
        }
        public async Task<int> DeleteAuthorAsync(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            _context.Authors.Remove(author);
            return await _context.SaveChangesAsync();
        }
    }
}
