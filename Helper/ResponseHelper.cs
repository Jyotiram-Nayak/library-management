using Microsoft.AspNetCore.Mvc;

namespace library_management.Helper
{
    public static class ResponseHelper
    {
        public static IActionResult GenerateResponse(bool success, string message, object data = null)
        {
            return new JsonResult(new { success, message, data });
        }
    }
}
