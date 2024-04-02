using library_management.Data.Model;
using library_management.Data.ViewModel.Authentication;
using Microsoft.AspNetCore.Identity;

namespace library_management.Data.Repository
{
    public interface IAuthRepository
    {
        Task<IdentityResult> RegisterAsync(RegisterVM registerVM);
        Task<IdentityResult> ConfirmEmail(string uid, string token);
        Task<SignInResult> LoginAsync(LoginVM loginVM);
        Task<string> GenerateJWTTokenAsync(string email);
        Task<ApplicationUser> GetUserByEmailAsync(string email);
    }
}