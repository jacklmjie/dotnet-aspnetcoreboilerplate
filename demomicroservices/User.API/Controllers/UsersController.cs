using System.Linq;
using Microsoft.AspNetCore.Mvc;
using User.API.Data;

namespace User.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseController
    {
        private UserContext _useContext;
        public UsersController(UserContext userContext)
        {
            _useContext = userContext;
        }

        [Route("")]
        [HttpGet]
        public ActionResult<string> Get()
        {
            var user = _useContext.Users
                .SingleOrDefault(x => x.Id == Identity.UserId);

            if (user == null)
                return NotFound();

            return Ok(user);
        }
    }
}
