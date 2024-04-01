using library_management.Data.ViewModel;

namespace library_management.Data.Repository
{
    public interface IAuthorRepository
    {
        Task<int> AddAuthor(AuthorsVM authorsVM);
        Task<List<AuthorsVM>> GetAllAuthors();
    }
}