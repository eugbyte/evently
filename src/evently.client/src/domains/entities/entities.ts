export class Gathering {
	gatheringId = 0;
	name = "";
	description = "";
	start = new Date();
	end = new Date();
	location = "";
	coverSrc?: string | null = null;
	organiserId = "";
	cancellationDateTime: Date | null = null;
	bookings: Booking[] = []; // You may want to replace 'any[]' with a proper Booking[] type
	gatheringCategoryDetails: GatheringCategoryDetail[] = []; // You may want to replace 'any[]' with a proper GatheringCategoryDetail[] type

	constructor(data: Partial<Gathering> = {}) {
		Object.assign(this, data);
	}
}

export class Account {
	id = "";
	name = "";
	userName = "";
	email = "";
	bookings: Booking[] = []; // You may want to replace 'any[]' with a proper Booking[] type

	constructor(data: Partial<Account> = {}) {
		Object.assign(this, data);
	}
}

export class Booking {
	bookingId = "";
	attendeeId = "";
	accountDto = new Account();
	gatheringId = 0;
	gathering = new Gathering();
	creationDateTime = new Date();
	checkInDateTime: Date | null = null;
	checkoutDateTime: Date | null = null;
	cancellationDateTime: Date | null = null;

	constructor(partial: Partial<Booking> = {}) {
		Object.assign(this, partial);
	}
}

export class Category {
	categoryId = 0;
	categoryName = "";

	constructor(partial: Partial<Category> = {}) {
		// Apply partial properties using Object.assign
		Object.assign(this, partial);
	}
}

export class GatheringCategoryDetail {
	gatheringId = 0;
	categoryId = 0;
	gathering = new Gathering();
	category = new Category();

	constructor(partial: Partial<GatheringCategoryDetail> = {}) {
		// Apply partial properties using Object.assign
		Object.assign(this, partial);
	}
}
