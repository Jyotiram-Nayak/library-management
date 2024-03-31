using System.Security.Claims;

namespace library_management.Services
{
    public class UserServices : IUserServices
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public UserServices(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        public string GetUserId()
        {
            return _contextAccessor.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
