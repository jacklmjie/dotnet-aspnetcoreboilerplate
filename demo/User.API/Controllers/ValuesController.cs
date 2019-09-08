using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using User.API.Data;
using Microsoft.EntityFrameworkCore;

namespace User.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private UserContext _useContext;
        public ValuesController(UserContext userContext)
        {
            _useContext = userContext;
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            return _useContext.Users.SingleOrDefault(x => x.Name == "jack.li").Name;
        }
    }
}
