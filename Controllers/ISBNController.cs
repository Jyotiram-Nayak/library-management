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
        [HttpPost("add-isbn/{bookId}")]
        public async Task<IActionResult> AddISBNNumber([FromRoute] int bookId)
        {
            var book = await _bookRepository.GetBookByIdAsync(bookId);
            if (book == null) { return BadRequest(); }
            var result = await _iSBNRepository.AddISBN(bookId);
            if (result == 0)
            {
                response = new { success = false, message = "Failed to add author.", data = result };
                return Unauthorized(response);
            };
            response = new { success = true, message = "BookISBN added successfully...", data = result };
            return Ok(response);
        }
    }
}
