using Azure;
using library_management.Data.Services;
using library_management.Data.ViewModel;
using library_management.Data.ViewModel.Authentication;
using library_management.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace library_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly IUserServices _userServices;
        private object responce;

        public AuthenticationController(IAuthRepository authRepository,
            IUserServices userServices)
        {
            _authRepository = authRepository;
            _userServices = userServices;
        }
        /// <summary>
        /// Registration
        /// </summary>
        /// <param name="registerVM"></param>
        /// <returns></returns>
        [HttpPost("register-user")]
        public async Task<IActionResult> Register([FromBody]RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Please, provide all required fields.");
            }
            var result = await _authRepository.RegisterAsync(registerVM);
            if (!result.Succeeded)
            {
                return BadRequest("User not created");
            }
            return Ok("signup successful");
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
            var token = await _authRepository.LoginAsync(loginVM);

            if (token == null)
            {
                return Unauthorized(new { success = false, message = "SignIn failed." });
            }
            var uid = _userServices.GetUserId();
            responce = new
            {
                success = true,
                message = "SignIn successfully.",
                result = new { uid, token }
            };
            return Ok(responce);
        }
    }
}
