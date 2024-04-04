using AutoMapper;
using library_management.Data.Model;
using library_management.Data.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace library_management.Data.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CategoryRepository(AppDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CategoryVM>> GetAllCategory()
        {
            var category = await _context.Category.ToListAsync();
            return _mapper.Map<List<CategoryVM>>(category);
        }
        public async Task<CategoryVM> GetCategoryDetails(int id)
        {
            var category = await _context.Category.FindAsync(id);
            return _mapper.Map<CategoryVM>(category);
        }
        public async Task<int> AddCategoryAsunc(CategoryVM categoryVM)
        {
            await _context.Category.AddAsync(_mapper.Map<Category>(categoryVM));
            return await _context.SaveChangesAsync();
        }
        public async Task<int> UpdateCategoryAsunc(int id, CategoryVM categoryVM)
        {
            var category = await _context.Category.FindAsync(id);
            if (category != null)
            {
                _mapper.Map<Category>(categoryVM);
            }
            return await _context.SaveChangesAsync();
        }
        public async Task<int> DeleteCategoryAsunc(int id)
        {
            var category = await _context.Category.FindAsync(id);
            if (category != null)
            {
                _context.Category.Remove(category);
            }
            return await _context.SaveChangesAsync();
        }
    }
}