using Microsoft.AspNetCore.Mvc;
using User.API.Entity.Dtos;

namespace User.API.Controllers
{
    public class BaseController : ControllerBase
    {
        public IdentityModel Identity => new IdentityModel() { UserId = 1, Name = "jack.li" };
    }
}
