using library_management.Data.ViewModel;

namespace library_management.Data.Repository
{
    public interface IAuthorRepository
    {
        Task<int> AddAuthorAsync(AuthorsVM authorsVM);
        Task<List<AuthorsVM>> GetAllAuthorsAsync();
        Task<AuthorsVM> GetAuthorByIdAsync(Guid id);
        Task<int> UpdateAuthorAsync(Guid id, AuthorsVM authorsVM);
        Task<int> DeleteAuthorAsync(Guid id);
    }
}