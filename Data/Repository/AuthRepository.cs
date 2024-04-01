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

namespace library_management.Data.Services
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailServices _emailServices;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthRepository(UserManager<ApplicationUser> userManager,
            IConfiguration configuration,IEmailServices emailServices,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _configuration = configuration;
            _emailServices = emailServices;
            _signInManager = signInManager;
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
            await SendEmailConfirmationAsync(newUser);
            return result;
        }
        public async Task SendEmailConfirmationAsync(ApplicationUser user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            if (!string.IsNullOrEmpty(token))
            {
                //await SendEmailConfirmationAsync(user, token);
                string appDomain = _configuration.GetSection("Application:AppDomain").Value ?? "";
                string confirmLink = _configuration.GetSection("Application:EmailConfirmation").Value ?? "";
                EmailMessage emailMessage = new EmailMessage
                {
                    Subject = "ConfirmEmail",
                    ToEmails = new List<string>() { user.Email },
                    PlaceHolders = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{UserName}}",user.FirstName),
                    new KeyValuePair<string, string>("{{Link}}",string.Format(appDomain+confirmLink,user.Id,token))
                }
                };
                await _emailServices.SendEmailConfirmationMessage(emailMessage);
            }
        }
        /// <summary>
        /// Email varification=true is database
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<IdentityResult> ConfirmEmail(string uid, string token)
        {
            return await _userManager.ConfirmEmailAsync(await _userManager.FindByIdAsync(uid), token);
        }
        /// <summary>
        /// user EmailServices to send email for email confirmation 
        /// </summary>
        /// <param name="applicationUser"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        //private async Task SendEmailConfirmationAsync(ApplicationUser applicationUser,string token)
        //{
        //    string appDomain = _configuration.GetSection("Application:AppDomain").Value ?? "";
        //    string confirmLink = _configuration.GetSection("Application:EmailConfirmation").Value ?? "";
        //    EmailMessage emailMessage = new EmailMessage
        //    {
        //        Subject = "ConfirmEmail",
        //        ToEmails = new List<string>() { applicationUser.Email },
        //        PlaceHolders = new List<KeyValuePair<string, string>>()
        //        {
        //            new KeyValuePair<string, string>("{{UserName}}",applicationUser.FirstName),
        //            new KeyValuePair<string, string>("{{Link}}",string.Format(appDomain+confirmLink,applicationUser.Id,token))
        //        }
        //    };
        //    await _emailServices.SendEmailConfirmationMessage(emailMessage);
        //}

        /// <summary>
        /// user Login Method
        /// </summary>
        /// <param name="loginVM"></param>
        /// <returns></returns>
        public async Task<SignInResult> LoginAsync(LoginVM loginVM)
        {
            return await _signInManager.PasswordSignInAsync(loginVM.Email, loginVM.Password, false, false);

            //if (result.IsNotAllowed)
            //{
            //    return "Not Allow";
            //}
            //else if (!result.Succeeded)
            //{
            //    return null;
            //}
            //var user = await _userManager.FindByEmailAsync(loginVM.Email);
            //var roles = _userManager.GetRolesAsync(user).Result.ToList();
            //var authClaims = new List<Claim>
            //{
            //    new Claim(ClaimTypes.Name, loginVM.Email),
            //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            //    new Claim(ClaimTypes.NameIdentifier, user.Id),
            //};
            //foreach (var role in roles)
            //{
            //    authClaims.Add(new Claim(ClaimTypes.Role, role));
            //}

            //var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            //var token = new JwtSecurityToken(
            //    issuer: _configuration["JWT:ValidIssuer"],
            //    audience: _configuration["JWT:ValidAudience"],
            //    expires: DateTime.Now.AddDays(1),
            //    claims: authClaims,
            //    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            //);

            //return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<string> GenrateJWTTokenAsync(string email)
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
    }
}
