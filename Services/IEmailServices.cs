using library_management.Data.Model;
using library_management.Data.ViewModel.Email;

namespace library_management.Services
{
    public interface IEmailServices
    {
        Task SendEmailConfirmationAsync(ApplicationUser user);
    }
}