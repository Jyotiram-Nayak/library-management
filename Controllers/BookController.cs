using Azure;
using library_management.Data.Repository;
using library_management.Data.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace library_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private object response;
        public BookController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }
        [HttpGet("get-all-books")]
        public async Task<IActionResult> GetAllBooks()
        {
            var result = await _bookRepository.GetAllBooksAsync();
            if (result == null)
            {
                response = new{success = false,message = "somthing went wrong...",data = new { result }};
                return Unauthorized(response);
            };
            response = new{success = true,message = "Books fetched successfully...",data = new { result }};
            return Ok(response);
        }
        [HttpGet("get-book-details/{id}")]
        public async Task<IActionResult> GetBookDetails([FromRoute]int id)
        {
            var result = await _bookRepository.GetBookByIdAsync(id);
            if (result == null)
            {
                response = new { success = false, message = "somthing went wrong...", data = new { result } };
                return Unauthorized(response);
            };
            response = new { success = true, message = "Book fetched successfully...", data = new { result } };
            return Ok(response);
        }

        [HttpPost("add-book")]
        public async Task<IActionResult> AddBook([FromBody] BooksVM booksVM)
        {
            var result = await _bookRepository.AddBooksAsync(booksVM);
            if (result == 0)
            {
                response = new { success = false,message = "somthing went wrong...",data = new { result }};
                return Unauthorized(response);
            };
            response = new{success = true,message = "Book added successfully...",data = new { result }};
            return Ok(response);
        }
        [HttpPut("update-book/{id}")]
        public async Task<IActionResult> UpdateBookById([FromRoute] int id, [FromBody] BooksVM booksVM)
        {
            var result = await _bookRepository.UpdateBookByIdAsync(id, booksVM);
            if (result == 0)
            {
                response = new{success = false,message = "somthing went wrong...",data = new { result }};
                return BadRequest(response);
            };
            response = new{success = true,message = "Book updated successfully...",data = new { result }};
            return Ok(response);
        }
        [HttpDelete("delete-book/{id}")]
        public async Task<IActionResult> DeleteBookById(int id)
        {
            var result = await _bookRepository.DeleteBookById(id);
            if (result == 0)
            {
                response = new{success = false,message = "somthing went wrong...",data = new { result }};
                return BadRequest(response);
            };
            response = new{success = true,message = "Book deleted successfully...",data = new { result }};
            return Ok(response);
        }
    }
}
