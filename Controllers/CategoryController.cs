using Azure;
using library_management.Data.Repository;
using library_management.Data.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace library_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        [HttpGet("get-all-category")]
        public async Task<IActionResult> GetAllCategory()
        {
            var result = await _categoryRepository.GetAllCategory();
            if (result == null)
            {
                return Ok(new { success = false, message = "Failed to fetch Categorys.", data = result });
            };
            return Ok(new { success = true, message = "Categorys fetched successfully...", data = result });
        }
        [HttpGet("category-details/{id}")]
        public async Task<IActionResult> GetCategoryDetails([FromRoute] Guid id)
        {
            var result = await _categoryRepository.GetCategoryDetails(id);
            if (result == null)
            {
                return BadRequest(new { success = false, message = "Failed to fetch Category.", data = result });
            };
            return Ok(new { success = true, message = "Category fetched successfully...", data = result });
        }
        [HttpPost("add-category")]
        public async Task<IActionResult> AddCategory([FromBody] CategoryVM categoryVM)
        {
            var result = await _categoryRepository.AddCategoryAsunc(categoryVM);
            if (result == 0)
            {
                return BadRequest(new { success = false, message = "Failed to add Category .", data = result });
            };
            return Ok(new { success = true, message = "Category added successfully...", data = result });
        }
        [HttpPost("update-category/{id}")]
        public async Task<IActionResult> UpdateCategory([FromRoute] Guid id,[FromForm] CategoryVM categoryVM)
        {
            var result = await _categoryRepository.UpdateCategoryAsunc(id,categoryVM);
            if (result == 0)
            {
                return BadRequest(new { success = false, message = "Failed to update Category", data = result });
            };
            return Ok(new { success = true, message = "Category updated successfully...", data = result });
        }
        [HttpPost("delete-category")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            var result = await _categoryRepository.DeleteCategoryAsunc(id);
            if (result == 0)
            {
                return BadRequest(new { success = false, message = "Failed to delete Category", data = result });
            };
            return Ok(new { success = true, message = "Category deleted successfully...", data = result });
        }
    }
}
