using Azure;
using library_management.Data;
using library_management.Data.Repository;
using library_management.Data.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Policy;

namespace library_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BorrowBookController : ControllerBase
    {
        private readonly IBorrowRepository _borrowRepository;
        private object? response;
        public BorrowBookController(IBorrowRepository borrowRepository)
        {
            _borrowRepository = borrowRepository;
        }
        [HttpPost("borrow-book/{bookId}")]
        public async Task<IActionResult> BorrowBook(Guid bookId, [FromQuery] Guid isbn)
        {
            var result = await _borrowRepository.BorrowBookAsync(bookId,isbn);
            if (result == 0)
            {
                response = new { success = false, message = "Failed to Issue Book.", data = result };
                return BadRequest(response);
            };
            response = new { success = true, message = "Book issue successfully...", data = result };
            return Ok(response);
        }
        [HttpPost("return-book/{bookId}")]
        public async Task<IActionResult> ReturnBook([FromRoute]Guid bookId)
        {
            var result = await _borrowRepository.ReturnBookAsync(bookId);
            if (result == 0)
            {
                response = new { success = false, message = "Failed to return Book.", data = result };
                return BadRequest(response);
            };
            response = new { success = true, message = "Book return successfully...", data = result };
            return Ok(response);
        }
    }
}
