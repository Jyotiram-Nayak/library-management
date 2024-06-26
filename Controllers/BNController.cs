﻿using Azure;
using library_management.Data.Repository;
using library_management.Data.ViewModel.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace library_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = UserRoles.Admin)]
    public class BNController : ControllerBase
    {
        private readonly IBNRepository _iSBNRepository;
        private readonly IBookRepository _bookRepository;
        private object response;

        public BNController(IBNRepository iSBNRepository,
            IBookRepository bookRepository)
        {
            _iSBNRepository = iSBNRepository;
            _bookRepository = bookRepository;
        }
        [HttpGet("get-all-bn")]
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
        [HttpPost("add-bn/{bookId}")]
        public async Task<IActionResult> AddISBNNumber([FromRoute] Guid bookId)
        {
            var book = await _bookRepository.GetBookByIdAsync(bookId,null,null,null);
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
        [HttpPut("update-bn/{isbnno}")]
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
