using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Core.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SlowRequestController : ControllerBase
    {
        private readonly ILogger _logger;

        public SlowRequestController(
            ILogger<SlowRequestController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 耗时请求取消
        /// https://www.cnblogs.com/sheng-jie/p/9660288.html
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("/slowtest")]
        public string Get(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting to do slow work");

            //HttpContext.RequestAborted == cancellationToken
            for (var i = 0; i < 10; i++)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    // stop!
                    cancellationToken.ThrowIfCancellationRequested();
                }
                // slow non-cancellable work
                Thread.Sleep(1000);
            }

            // slow async action, e.g. call external api
            //await Task.Delay(10_000, cancellationToken);

            var message = "Finished slow delay of 10 seconds.";

            _logger.LogInformation(message);

            return message;
        }
    }
}