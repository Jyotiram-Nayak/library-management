using library_management.Data.Model;
using library_management.Data.ViewModel.Authentication;
using library_management.Data.ViewModel.Email;
using library_management.Services;
using Microsoft.AspNetCore.Identity;

namespace library_management.Data.Services
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailServices _emailServices;

        public AuthRepository(UserManager<ApplicationUser> userManager,
            IConfiguration configuration,IEmailServices emailServices)
        {
            _userManager = userManager;
            _configuration = configuration;
            _emailServices = emailServices;
        }
        public async Task<IdentityResult> RegisterAsync(RegisterVM registerVM)
        {
            ApplicationUser newUser = new ApplicationUser
            {
                FirstName = registerVM.FirstName,
                LastName = registerVM.LastName,
                Email = registerVM.Email,
                UserName = registerVM.Email,
            };
            IdentityResult result = await _userManager.CreateAsync(newUser, registerVM.Password);
            if (!result.Succeeded)
            {
                return result;
            }
            switch (registerVM.Role)
            {
                case "Admin":
                    await _userManager.AddToRoleAsync(newUser, UserRoles.Admin);
                    break;
                case "User":
                    await _userManager.AddToRoleAsync(newUser, UserRoles.Member);
                    break;
                case "Author":
                    await _userManager.AddToRoleAsync(newUser, UserRoles.Author);
                    break;
                default:
                    await _userManager.AddToRoleAsync(newUser, UserRoles.Member);
                    break;
            }
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            if (!string.IsNullOrEmpty(token))
            {
                await SendEmailConfirmationAsync(newUser, token);
            }
            return result;
        }
        public async Task<IdentityResult> ConfirmEmail(string uid, string token)
        {
            return await _userManager.ConfirmEmailAsync(await _userManager.FindByIdAsync(uid), token);
        }
        private async Task SendEmailConfirmationAsync(ApplicationUser applicationUser,string token)
        {
            string appDomain = _configuration.GetSection("Application:AppDomain").Value;
            string confirmLink = _configuration.GetSection("Application:EmailConfirmation").Value;
            EmailMessage emailMessage = new EmailMessage
            {
                Subject = "ConfirmEmail",
                ToEmails = new List<string>() { applicationUser.Email },
                PlaceHolders = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{UserName}}",applicationUser.FirstName),
                    new KeyValuePair<string, string>("{{Link}}",string.Format(appDomain+confirmLink,applicationUser.Id,token))
                }
            };
            await _emailServices.SendEmailConfirmationMessage(emailMessage);
        }

    }
}
