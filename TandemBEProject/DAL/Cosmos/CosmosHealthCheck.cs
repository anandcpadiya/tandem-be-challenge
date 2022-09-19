using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace TandemBEProject.DAL.Cosmos
{
    public class CosmosHealthCheck : IHealthCheck
    {
        private readonly IDbService _dbService;

        public CosmosHealthCheck(IDbService dbService)
        {
            _dbService = dbService;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            return await _dbService.IsHealthy()
                ? await Task.FromResult(HealthCheckResult.Healthy("Healthy"))
                : await Task.FromResult(HealthCheckResult.Unhealthy("Unhealthy"));
        }
    }
}
