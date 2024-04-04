using library_management.Data.Model;
using library_management.Data.ViewModel.Authentication;
using library_management.Data.ViewModel.Email;
using library_management.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace library_management.Data.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailServices _emailServices;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserServices _userServices;

        public AuthRepository(UserManager<ApplicationUser> userManager,
            IConfiguration configuration,IEmailServices emailServices,
            SignInManager<ApplicationUser> signInManager,
            IUserServices userServices)
        {
            _userManager = userManager;
            _configuration = configuration;
            _emailServices = emailServices;
            _signInManager = signInManager;
            this._userServices = userServices;
        }
        /// <summary>
        /// user Registration method
        /// </summary>
        /// <param name="registerVM"></param>
        /// <returns></returns>
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
                case "Member":
                    await _userManager.AddToRoleAsync(newUser, UserRoles.Member);
                    break;
                case "Author":
                    await _userManager.AddToRoleAsync(newUser, UserRoles.Author);
                    break;
                default:
                    await _userManager.AddToRoleAsync(newUser, UserRoles.Member);
                    break;
            }
            await _emailServices.SendEmailConfirmationAsync(newUser);
            return result;
        }
        public async Task<IdentityResult> ConfirmEmail(string uid, string token)
        {
            return await _userManager.ConfirmEmailAsync(await _userManager.FindByIdAsync(uid), token);
        }
        /// <summary>
        /// user Login Method
        /// </summary>
        /// <param name="loginVM"></param>
        /// <returns></returns>
        public async Task<SignInResult> LoginAsync(LoginVM loginVM)
        {
            return await _signInManager.PasswordSignInAsync(loginVM.Email, loginVM.Password, false, false);
        }
        /// <summary>
        /// Generate jwt bearer token 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<string> GenerateJWTTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var roles = _userManager.GetRolesAsync(user).Result.ToList();
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };
            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user;
        }
        public async Task<IdentityResult> ChangePasswordAsync(ChangePasswordVM changePassword)
        {
            var uid = _userServices.GetUserId();
            var user = await _userManager.FindByIdAsync(uid);
            if (user == null)
            {
                return null;
            }
            return await _userManager.ChangePasswordAsync(user, changePassword.CurrentPassword, changePassword.NewPassword);
        }
    }
}
