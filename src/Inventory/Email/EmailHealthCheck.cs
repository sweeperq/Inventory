using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Inventory.Email;

public class EmailHealthCheck(IEmailService emailService) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        return await emailService.CanConnectAsync(cancellationToken)
            ? HealthCheckResult.Healthy("Email service is healthy.")
            : HealthCheckResult.Unhealthy("Email service is unhealthy.");
    }
}
