using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DemoWebApi
{
    /// <summary>
    /// redis健康检查
    /// </summary>
    public class RedisHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            //doing some redis check things.
            return Task.FromResult(HealthCheckResult.Healthy());
        }
    }
}
