import axios from "axios";
import { Booking } from "~/domains/entities";
import { BookingReqDto } from "~/domains/models";
import type { PageResult } from "~/domains/interfaces";

export interface GetBookingsParams {
	attendeeId?: string;
	gatheringId?: number;
	checkInStart?: Date;
	checkInEnd?: Date;
	gatheringStartBefore?: Date;
	gatheringStartAfter?: Date;
	gatheringEndBefore?: Date;
	gatheringEndAfter?: Date;
	isCancelled?: boolean;
	offset?: number;
	limit?: number;
}

export async function getBookings(params: GetBookingsParams): Promise<PageResult<Booking[]>> {
	const response = await axios.get<Booking[]>("/api/v1/Bookings", { params });
	const bookings: Booking[] = response.data;
	for (const booking of bookings) {
		booking.creationDateTime = new Date(booking.creationDateTime);
		booking.checkInDateTime = booking.checkInDateTime ? new Date(booking.checkInDateTime) : null;
		booking.checkoutDateTime = booking.checkoutDateTime ? new Date(booking.checkoutDateTime) : null;
		booking.cancellationDateTime = booking.cancellationDateTime
			? new Date(booking.cancellationDateTime)
			: null;

		if (booking.gathering != null) {
			booking.gathering.start = new Date(booking.gathering.start);
			booking.gathering.end = new Date(booking.gathering.end);
		}
	}

	const totalCount: number = parseInt(response.headers["x-total-count"]);
	return {
		totalCount,
		data: bookings
	};
}

export async function createBooking(bookingReqDto: BookingReqDto): Promise<Booking> {
	const response = await axios.post<Booking>("/api/v1/Bookings", bookingReqDto);
	return response.data;
}

export async function getBooking(bookingId: string): Promise<Booking> {
	const response = await axios.get<Booking>(`/api/v1/Bookings/${bookingId}`);
	const booking: Booking = response.data;
	booking.creationDateTime = new Date(booking.creationDateTime);
	booking.checkInDateTime = booking.checkInDateTime ? new Date(booking.checkInDateTime) : null;
	booking.checkoutDateTime = booking.checkoutDateTime ? new Date(booking.checkoutDateTime) : null;
	booking.cancellationDateTime = booking.cancellationDateTime
		? new Date(booking.cancellationDateTime)
		: null;
	return booking;
}

export async function checkInBooking(bookingId: string): Promise<Booking> {
	const response = await axios.patch<Booking>(`/api/v1/Bookings/${bookingId}/checkIn`);
	return response.data;
}

export async function cancelBooking(bookingId: string): Promise<Booking> {
	const response = await axios.patch<Booking>(`/api/v1/Bookings/${bookingId}/cancel`);
	return response.data;
}
