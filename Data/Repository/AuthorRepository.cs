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

        public AuthorRepository(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<List<AuthorsVM>> GetAllAuthors()
        {
            var authors = await _context.Authors.Select(x => new AuthorsVM
            {
                AuthorId = x.AuthorId,
                Name = x.Name,
                Biography = x.Biography
            }).ToListAsync();
            return authors;
        }
        public async Task<int> AddAuthor(AuthorsVM authorsVM)
        {
            var author = new Authors
            {
                Name = authorsVM.Name,
                Biography = authorsVM.Biography,
            };
            _context.Authors.Add(author);
            return await _context.SaveChangesAsync();
        }
    }
}
