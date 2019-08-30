using Core.API.Options;
using Core.IContract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Core.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly ILogger<IdentityController> _logger;
        private readonly JwtOption _jwtOption;
        private readonly IIdentityContract _identityContract;
        public IdentityController(ILogger<IdentityController> logger,
            IOptions<JwtOption> jwtOption,
            IIdentityContract identityContract)
        {
            _logger = logger;
            _jwtOption = jwtOption.Value;
            _identityContract = identityContract;
        }
    }
}