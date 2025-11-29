using Microsoft.AspNetCore.Diagnostics;

namespace Evently.Server.Common.Middlewares;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler {
	public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
		CancellationToken cancellationToken) {
		string exceptionMessage = exception.Message;
		logger.LogError(
			"Error Message: {exceptionMessage}, Time of occurrence {time}",
			exceptionMessage,
			DateTime.UtcNow);

		switch (exception) {
			case ArgumentException:
				httpContext.Response.StatusCode = 400;
				await httpContext.Response.WriteAsJsonAsync(value: new {
						Title = "Validation Error",
						Detail = exceptionMessage,
						Status = StatusCodes.Status400BadRequest,
					},
					cancellationToken);
				break;
			default:
				httpContext.Response.StatusCode = 500;
				await httpContext.Response.WriteAsJsonAsync(value: new {
						Title = "Server Error",
						Detail = "An unexpected error occurred.",
						Status = StatusCodes.Status500InternalServerError,
					},
					cancellationToken);
				break;
		}

		return true;
	}
}