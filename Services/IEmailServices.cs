using library_management.Data.ViewModel.Email;

namespace library_management.Services
{
    public interface IEmailServices
    {
        Task SendEmailConfirmationMessage(EmailMessage emailMessage);
        Task SendEmailMessage(EmailMessage emailMessage);
    }
}