using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Evently.Server.Features.HealthChecks.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public sealed class HealthChecksController(HealthCheckService healthCheckService) : ControllerBase
{
	private readonly Dictionary<HealthStatus, string> _statuses = new()
	{
		{ HealthStatus.Healthy, "Healthy" },
		{ HealthStatus.Unhealthy, "Unhealthy" },
		{ HealthStatus.Degraded, "Degraded" },
	};

	[HttpGet(Name = "HealthCheck")]
	public async Task<ActionResult> GetHealthcheck()
	{
		HealthReport healthReport = await healthCheckService.CheckHealthAsync();

		Dictionary<string, string> statuses = healthReport.Entries.ToDictionary(
			keySelector: key => key.Key,
			elementSelector: value => _statuses[value.Value.Status]
		);
		statuses["Server"] = "Healthy";
		return Ok(statuses);
	}

	[HttpGet("middlewares/error-middleware", Name = "TestErrorMiddleware")]
	public Task<ActionResult> TestErrorMiddleware()
	{
		throw new ArgumentException("Test Error Middleware");
	}
}
