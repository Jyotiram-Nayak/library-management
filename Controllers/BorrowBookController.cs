using Azure;
using library_management.Data;
using library_management.Data.Repository;
using library_management.Data.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace library_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowBookController : ControllerBase
    {
        private readonly IBorrowRepository _borrowRepository;
        private object response;
        public BorrowBookController(IBorrowRepository borrowRepository)
        {
            _borrowRepository = borrowRepository;
        }
        [HttpPost("borrow-book/{bookId}")]
        public async Task<IActionResult> BorrowBook(int bookId, [FromQuery] string isbn)
        {
            var result = await _borrowRepository.BorrowBookAsync(bookId,isbn);
            if (result == 0)
            {
                response = new { success = false, message = "Failed to Issue Book.", data = result };
                return Unauthorized(response);
            };
            response = new { success = true, message = "Book issue successfully...", data = result };
            return Ok(response);
        }
    }
}
