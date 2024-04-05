using library_management.Data.ViewModel;

namespace library_management.Data.Repository
{
    public interface ICategoryRepository
    {
        Task<int> AddCategoryAsunc(CategoryVM categoryVM);
        Task<int> DeleteCategoryAsunc(Guid id);
        Task<List<CategoryVM>> GetAllCategory();
        Task<CategoryVM> GetCategoryDetails(Guid id);
        Task<int> UpdateCategoryAsunc(Guid id, CategoryVM categoryVM);
    }
}