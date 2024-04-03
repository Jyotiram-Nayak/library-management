using Azure;
using library_management.Data.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace library_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ISBNController : ControllerBase
    {
        private readonly IISBNRepository _iSBNRepository;
        private readonly IBookRepository _bookRepository;
        private object response;

        public ISBNController(IISBNRepository iSBNRepository,
            IBookRepository bookRepository)
        {
            _iSBNRepository = iSBNRepository;
            _bookRepository = bookRepository;
        }
        [HttpGet("get-all-isbn")]
        public async Task<IActionResult> GetAllISBN()
        {
            var result = await _iSBNRepository.GetISBNAllAsync();
            if (result == null)
            {
                response = new { success = false, message = "Failed fetch isbn number.", data = result };
                return Unauthorized(response);
            };
            response = new { success = true, message = "BookISBN added successfully...", data = result };
            return Ok(response);
        }
        [HttpPost("add-isbn/{bookId}")]
        public async Task<IActionResult> AddISBNNumber([FromRoute] int bookId)
        {
            var book = await _bookRepository.GetBookByIdAsync(bookId);
            if (book == null) { return BadRequest(); }
            var result = await _iSBNRepository.AddISBN(bookId);
            if (result == 0)
            {
                response = new { success = false, message = "Failed to add isbn number.", data = result };
                return Unauthorized(response);
            };
            response = new { success = true, message = "BookISBN added successfully...", data = result };
            return Ok(response);
        }
        [HttpPut("update-isbn/{isbnno}")]
        public async Task<IActionResult> UpdateISBN(string isbnno, [FromQuery]string userId, [FromQuery] bool isIssue)
        {
            var result = await _iSBNRepository.UpdateISBNAsync(isbnno,userId,isIssue);
            if (result == 0)
            {
                response = new { success = false, message = "Failed to update isbn number.", data = result };
                return Unauthorized(response);
            };
            response = new { success = true, message = "BookISBN updated successfully...", data = result };
            return Ok(response);
        }
    }
}
