using library_management.Data.Repository;
using library_management.Data.ViewModel;
using library_management.Data.ViewModel.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace library_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Author)]
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
            var result = await _authorRepository.GetAllAuthorsAsync();
            if (result == null)
            {
                response = new { success = false, message = "Authors not found.", data = result };
                return Unauthorized(response);
            };
            response = new { success = true, message = "Authors fetched successfully...", data = result };
            return Ok(response);
        }
        /// <summary>
        /// Get author details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("get-author-details/{id}")]
        public async Task<IActionResult> GetAuthorById([FromRoute]int id)
        {
            var result = await _authorRepository.GetAuthorByIdAsync(id);
            if (result == null)
            {
                response = new { success = false, message = "Author not found.", data = result };
                return Ok(response);
            };
            response = new { success = true, message = "Author fetched successfully...", data = result };
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
            var result = await _authorRepository.AddAuthorAsync(authorsVM);
            if (result == 0)
            {
                response = new { success = false, message = "Failed to add author.", data = result };
                return Unauthorized(response);
            };
            response = new { success = true, message = "Author added successfully...", data = result };
            return Ok(response);
        }
        [HttpPut("update-author/{id}")]
        public async Task<IActionResult> UpdateAuthor([FromRoute]int id,AuthorsVM authorsVM)
        {
            var result = await _authorRepository.UpdateAuthorAsync(id, authorsVM); 
            if (result == 0)
            {
                response = new { success = false, message = "Failed to update author.", data = result };
                return Unauthorized(response);
            };
            response = new { success = true, message = "Author updated successfully...", data = result };
            return Ok(response);
        }
        [HttpPut("delete-author/{id}")]
        public async Task<IActionResult> DeleteAuthor([FromRoute] int id)
        {
            var result = await _authorRepository.DeleteAuthorAsync(id);
            if (result == 0)
            {
                response = new { success = false, message = "Failed to delete author.", data = result };
                return Unauthorized(response);
            };
            response = new { success = true, message = "Author deleted successfully...", data = result };
            return Ok(response);
        }
    }
}
