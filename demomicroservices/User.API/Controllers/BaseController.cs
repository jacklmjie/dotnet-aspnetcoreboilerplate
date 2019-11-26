using Microsoft.AspNetCore.Mvc;
using User.API.Entity.Dtos;

namespace User.API.Controllers
{
    public class BaseController : ControllerBase
    {
        protected UserIdentity UserIdentity => new UserIdentity() { UserId = 1, Name = "jack.li" };
    }
}
