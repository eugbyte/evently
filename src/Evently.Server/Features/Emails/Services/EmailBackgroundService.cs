using System.Threading.Channels;
using Evently.Server.Common.Extensions;
using Evently.Server.Domains.Entities;
using Evently.Server.Domains.Interfaces;

namespace Evently.Server.Features.Emails.Services;

public sealed class EmailBackgroundService(
	IServiceScopeFactory scopeFactory,
	ChannelReader<string> reader,
	IEmailerAdapter emailerAdapter,
	ILogger<EmailBackgroundService> logger
) : BackgroundService
{
	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		while (await reader.WaitToReadAsync(stoppingToken))
		{
			try
			{
				string bookingId = await reader.ReadAsync(stoppingToken);

				using IServiceScope scope = scopeFactory.CreateScope();
				IBookingService bookingService =
					scope.ServiceProvider.GetRequiredService<IBookingService>();

				Booking? booking = await bookingService.GetBooking(bookingId);
				if (booking?.Account?.Email is null)
				{
					continue;
				}

				Account account = booking.Account;

				string html = await bookingService.RenderTicket(bookingId);
				await emailerAdapter.SendEmailAsync(
					"noreply@evently",
					account.Email,
					"Test QR ticket",
					html
				);
				logger.LogSuccessEmail(account.Email);
			}
			catch (Exception ex)
			{
				logger.LogError("email error: {}", ex.Message);
			}
		}
	}
}
