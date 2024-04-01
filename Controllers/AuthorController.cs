using library_management.Data.Repository;
using library_management.Data.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace library_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorRepository _authorRepository;
        private object response;

        public AuthorController(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }
        /// <summary>
        /// Get all Authors Details
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-all-authors")]
        public async Task<IActionResult> GetAllAuthors()
        {
            var result = await _authorRepository.GetAllAuthors();
            if (result == null)
            {
                response = new
                {
                    success = false,
                    message = "Data not found...",
                    data = new { result }
                };
                return Unauthorized(response);
            };
            response = new
            {
                success = true,
                message = "Data fetched successfully...",
                data = new { result }
            };
            return Ok(response);
        }
        /// <summary>
        /// Add Author
        /// </summary>
        /// <param name="authorsVM"></param>
        /// <returns></returns>
        [HttpPost("add-author")]
        public async Task<IActionResult> AddAuthor([FromBody] AuthorsVM authorsVM)
        {
            var result = await _authorRepository.AddAuthor(authorsVM);
            if (result == 0)
            {
                response = new
                {
                    success = false,
                    message = "Not inserted...",
                    data = new { result }
                };
                return Unauthorized(response);
            };
            response = new
            {
                success = true,
                message = "Data inserted...",
                data = new { result }
            };
            return Ok(response);
        }
    }
}
