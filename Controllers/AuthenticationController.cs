using library_management.Data.Services;
using library_management.Data.ViewModel.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace library_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthRepository _authService;

        public AuthenticationController(IAuthRepository authService)
        {
            _authService = authService;
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
            var result = await _authService.RegisterAsync(registerVM);
            if (!result.Succeeded)
            {
                return BadRequest("User not created");
            }
            return Ok("signup successful");
        }
        [HttpGet("confirm-email")]
        public async Task<IActionResult> SendConfirmEmail([FromQuery] string uid, [FromQuery] string token)
        {
            if (string.IsNullOrEmpty(uid) || string.IsNullOrEmpty(token))
            {
                return BadRequest();
            }
            token = token.Replace(" ", "+");
            var result = await _authService.ConfirmEmail(uid, token);
            if (!result.Succeeded)
            {
                return Unauthorized();
            }
            return Ok("Thank you for varification");
        }
    }
}
