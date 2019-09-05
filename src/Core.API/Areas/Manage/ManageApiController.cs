using Microsoft.AspNetCore.Mvc;

namespace Core.API.Areas.Manage
{
    /// <summary>
    /// 后台
    /// </summary>
    [Area("Manage")]
    [ApiController]
    [Route("api/[area]/[controller]")]
    [ApiConventionType(typeof(MyAppConventions))]
    public abstract class ManageApiController : ControllerBase
    {
    }
}