export class BookingReqDto {
	bookingId = "";
	attendeeId = "";
	gatheringId = 0;
	creationDateTime = new Date();
	checkInDateTime: Date | null = null;
	checkoutDateTime: Date | null = null;
	cancellationDateTime: Date | null = null;

	constructor(partial: Partial<BookingReqDto> = {}) {
		Object.assign(this, partial);
	}
}

export class GatheringReqDto {
	gatheringId = 0;
	name = "";
	description = "";
	start = new Date();
	end = new Date();
	location = "";
	organiserId = "";
	coverSrc?: string | null = null;
	cancellationDateTime: Date | null = null;
	gatheringCategoryDetails: GatheringCategoryDetailReqDto[] = [];

	constructor(partial: Partial<GatheringReqDto> = {}) {
		Object.assign(this, partial);
	}
}

export class GatheringCategoryDetailReqDto {
	gatheringId = 0;
	categoryId = 0;
}
