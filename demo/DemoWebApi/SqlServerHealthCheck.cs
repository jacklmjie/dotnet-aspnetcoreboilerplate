using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DemoWebApi
{
    /// <summary>
    /// sql健康检查
    /// https://www.cnblogs.com/uoyo/p/12396644.html
    /// </summary>
    public class SqlServerHealthCheck : IHealthCheck
    {
        SqlConnection _connection;

        public string Name => "sql";

        public SqlServerHealthCheck(SqlConnection connection)
        {
            _connection = connection;
        }

        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _connection.Open();
            }
            catch (SqlException)
            {
                return Task.FromResult(HealthCheckResult.Unhealthy("From Sql Serve"));
            }

            return Task.FromResult(HealthCheckResult.Healthy());
        }
    }
}
