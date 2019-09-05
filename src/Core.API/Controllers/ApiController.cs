using Microsoft.AspNetCore.Mvc;

namespace Core.API.Controllers
{
    /// <summary>
    /// WebApi控制器基类
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [ApiConventionType(typeof(MyAppConventions))]
    public abstract class ApiController : ControllerBase
    {

    }
}