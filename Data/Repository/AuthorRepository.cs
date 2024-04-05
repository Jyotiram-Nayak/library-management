using AutoMapper;
using library_management.Data.Model;
using library_management.Data.ViewModel;
using library_management.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace library_management.Data.Repository
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IUserServices _userServices;

        public AuthorRepository(AppDbContext context, UserManager<ApplicationUser> userManager,
            IMapper mapper,
            IUserServices userServices)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
            _userServices = userServices;
        }
        public async Task<List<AuthorsVM>> GetAllAuthorsAsync()
        {
            var authors = await _context.Authors.ToListAsync();
            return _mapper.Map<List<AuthorsVM>>(authors);
        }
        public async Task<AuthorsVM> GetAuthorByIdAsync(Guid id)
        {
            var author = await _context.Authors.FindAsync(id);
            return _mapper.Map<AuthorsVM>(author);
        }
        public async Task<int> AddAuthorAsync(AuthorsVM authorsVM)
        {
            authorsVM.CreatedDate = DateTime.Now;
            authorsVM.CreatedBy = _userServices.GetUserId();
            authorsVM.UpdatedDate = null;
            authorsVM.UpdatedBy = null;
            var author = _mapper.Map<Authors>(authorsVM);
            _context.Authors.Add(author);
            return await _context.SaveChangesAsync();
        }
        public async Task<int> UpdateAuthorAsync(Guid id, AuthorsVM authorsVM)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author != null)
            {
                authorsVM.UpdatedDate = DateTime.Now;
                authorsVM.UpdatedBy = _userServices.GetUserId();
                _mapper.Map<Authors>(authorsVM);
            }
            return await _context.SaveChangesAsync();
        }
        public async Task<int> DeleteAuthorAsync(Guid id)
        {
            var author = await _context.Authors.FindAsync(id);
            _context.Authors.Remove(author);
            return await _context.SaveChangesAsync();
        }
    }
}
