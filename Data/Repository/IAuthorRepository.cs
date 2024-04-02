using library_management.Data.ViewModel;

namespace library_management.Data.Repository
{
    public interface IAuthorRepository
    {
        Task<int> AddAuthorAsync(AuthorsVM authorsVM);
        Task<List<AuthorsVM>> GetAllAuthorsAsync();
        Task<AuthorsVM> GetAuthorByIdAsync(int id);
        Task<int> UpdateAuthorAsync(int id, AuthorsVM authorsVM);
        Task<int> DeleteAuthorAsync(int id);
    }
}