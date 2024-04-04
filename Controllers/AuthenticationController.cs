using library_management.Data.Model;
using library_management.Data.Repository;
using library_management.Data.ViewModel.Authentication;
using library_management.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace library_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly IUserServices _userServices;
        private readonly UserManager<ApplicationUser> _userManager;
        private object responce;

        public AuthenticationController(IAuthRepository authRepository,
            IUserServices userServices,
            UserManager<ApplicationUser> userManager)
        {
            _authRepository = authRepository;
            _userServices = userServices;
            _userManager = userManager;
        }
        /// <summary>
        /// Registration
        /// </summary>
        /// <param name="registerVM"></param>
        /// <returns></returns>
        [HttpPost("register-user")]
        public async Task<IActionResult> Register([FromBody] RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Please, provide all required fields.");
            }
            var user = await _authRepository.GetUserByEmailAsync(registerVM.Email);
            if(user != null)
            {
                return BadRequest(new { success = true, message = "The email address is already registered. Please try logging in or use a different email." });
            }
            var result = await _authRepository.RegisterAsync(registerVM);
            if (!result.Succeeded)
            {
                return BadRequest("User not created");
            }
            responce = new { success = true, message = "SignIn successfully.", data = result };
            return Ok(responce);
        }
        /// <summary>
        /// Email confirmation after registration
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet("confirm-email")]
        public async Task<IActionResult> SendConfirmEmail([FromQuery] string uid, [FromQuery] string token)
        {
            if (string.IsNullOrEmpty(uid) || string.IsNullOrEmpty(token))
            {
                return BadRequest();
            }
            token = token.Replace(" ", "+");
            var result = await _authRepository.ConfirmEmail(uid, token);
            if (!result.Succeeded)
            {
                return Unauthorized();
            }
            return Ok("Thank you for varification");
        }
        /// <summary>
        /// Login User
        /// </summary>
        /// <param name="loginVM"></param>
        /// <returns></returns>
        [HttpPost("login-user")]
        public async Task<IActionResult> Login([FromBody] LoginVM loginVM)
        {
            var result = await _authRepository.LoginAsync(loginVM);
            if (!result.Succeeded)
            {
                return Unauthorized(new { success = false, message = "SignIn failed." });
            }
            var token = await _authRepository.GenerateJWTTokenAsync(loginVM.Email);
            var uid = _userServices.GetUserId();
            responce = new { success = true, message = "SignIn successfully.", data = new { uid, token } };
            return Ok(responce);
        }
        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM changePassword)
        {
            var result = await _authRepository.ChangePasswordAsync(changePassword);
            if (!result.Succeeded)
            {
                return Unauthorized();
            }
            return Ok(result);
        }
    }
}
