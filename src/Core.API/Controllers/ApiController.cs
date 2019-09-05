using Microsoft.AspNetCore.Mvc;

namespace Core.API.Controllers
{
    /// <summary>
    /// WebApi控制器基类
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ApiConventionType(typeof(MyAppConventions))]
    public abstract class ApiController : ControllerBase
    {

    }
}