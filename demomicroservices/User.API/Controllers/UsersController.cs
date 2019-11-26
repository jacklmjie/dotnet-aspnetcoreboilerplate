using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using User.API.Data;
using User.API.Filters;

namespace User.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseController
    {
        private UserContext _useContext;
        private ILogger<UsersController> _logger;
        public UsersController(UserContext userContext, ILogger<UsersController> logger)
        {
            _useContext = userContext;
            _logger = logger;
        }

        [Route("")]
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var user = _useContext.Users
                .AsNoTracking()
                .Include(x => x.Properties)
                .SingleOrDefault(x => x.Id == UserIdentity.UserId);

            if (user == null)
                throw new UserOperationException($"错误的用户上下文Id{UserIdentity.UserId}");

            return Ok(user);
        }
    }
}
