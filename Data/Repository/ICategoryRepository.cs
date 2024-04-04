using library_management.Data.ViewModel;

namespace library_management.Data.Repository
{
    public interface ICategoryRepository
    {
        Task<int> AddCategoryAsunc(CategoryVM categoryVM);
        Task<int> DeleteCategoryAsunc(int id);
        Task<List<CategoryVM>> GetAllCategory();
        Task<CategoryVM> GetCategoryDetails(int id);
        Task<int> UpdateCategoryAsunc(int id, CategoryVM categoryVM);
    }
}