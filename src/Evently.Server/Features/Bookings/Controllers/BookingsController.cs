using System.Globalization;
using System.Threading.Channels;
using Evently.Server.Common.Extensions;
using Evently.Server.Domains.Entities;
using Evently.Server.Domains.Interfaces;
using Evently.Server.Domains.Models;
using Evently.Server.Features.Accounts.Services;
using Microsoft.AspNetCore.Mvc;

namespace Evently.Server.Features.Bookings.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public sealed class BookingsController(
    IBookingService bookingService,
    ChannelWriter<string> emailQueue,
    ILogger<BookingsController> logger
) : ControllerBase
{
    [HttpGet("{bookingId}", Name = "GetBooking")]
    public async Task<ActionResult<Booking>> GetBooking(string bookingId)
    {
        Booking? booking = await bookingService.GetBooking(bookingId);
        if (booking is null)
        {
            return NotFound();
        }

        return Ok(booking);
    }

    [HttpGet("{bookingId}/preview", Name = "PreviewBooking")]
    public async Task<ActionResult<Booking>> PreviewBooking(string bookingId)
    {
        string html = await bookingService.RenderTicket(bookingId);
        return Content(html, "text/html");
    }

    [HttpGet("", Name = "GetBookings")]
    public async Task<ActionResult<Booking>> GetBookings(
        string? attendeeId,
        long? gatheringId,
        DateTimeOffset? checkInStart,
        DateTimeOffset? checkInEnd,
        DateTimeOffset? gatheringStartBefore,
        DateTimeOffset? gatheringStartAfter,
        DateTimeOffset? gatheringEndBefore,
        DateTimeOffset? gatheringEndAfter,
        bool isCancelled,
        int? offset,
        int? limit
    )
    {
        PageResult<Booking> result = await bookingService.GetBookings(
            attendeeId,
            gatheringId,
            checkInStart,
            checkInEnd,
            gatheringStartBefore,
            gatheringStartAfter,
            gatheringEndBefore,
            gatheringEndAfter,
            isCancelled,
            offset,
            limit
        );
        List<Booking> bookingEvents = result.Items;
        int total = result.TotalCount;
        HttpContext.Response.Headers.Append("Access-Control-Expose-Headers", "X-Total-Count");
        HttpContext.Response.Headers.Append(
            "X-Total-Count",
            value: total.ToString(CultureInfo.InvariantCulture)
        );
        return Ok(bookingEvents);
    }

    [HttpPost("", Name = "CreateBooking")]
    public async Task<ActionResult<Booking>> CreateBooking([FromBody] BookingReqDto bookingReqDto)
    {
        Booking booking = await bookingService.CreateBooking(bookingReqDto);
        await emailQueue.WriteAsync(booking.BookingId);
        return Ok(booking);
    }

    [HttpPatch("{bookingId}/cancel", Name = "CancelBooking")]
    public async Task<ActionResult> CancelBooking(string bookingId)
    {
        Booking? booking = await bookingService.GetBooking(bookingId);
        if (booking?.Gathering is null)
        {
            return NotFound();
        }

        bool isAuth = await this.IsResourceOwner(booking.AttendeeId);
        logger.LogInformation("isAuth: {}", isAuth);
        if (!isAuth)
        {
            return Forbid();
        }

        booking.CancellationDateTime = DateTimeOffset.UtcNow;
        booking = await bookingService.UpdateBooking(
            bookingId,
            bookingReqDto: booking.ToBookingDto()
        );
        return Ok(booking);
    }

    [HttpPatch("{bookingId}/checkIn", Name = "CheckInBooking")]
    public async Task<ActionResult> CheckInBooking(string bookingId)
    {
        Booking? booking = await bookingService.GetBooking(bookingId);
        if (booking?.Gathering is null)
        {
            return NotFound();
        }

        Gathering gathering = booking.Gathering;
        bool isAuth = await this.IsResourceOwner(gathering.OrganiserId);
        logger.LogInformation("isAuth: {}", isAuth);
        if (!isAuth)
        {
            return Forbid();
        }

        booking.CheckInDateTime = DateTimeOffset.UtcNow;
        booking = await bookingService.UpdateBooking(
            bookingId,
            bookingReqDto: booking.ToBookingDto()
        );
        return Ok(booking);
    }
}
