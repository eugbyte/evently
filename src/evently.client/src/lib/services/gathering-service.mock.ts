import { Booking, Category, Gathering, GatheringCategoryDetail } from "~/domains/entities";
import { GatheringReqDto } from "~/domains/models";
import type { GetGatheringsParams } from "./gathering-service";
import type { PageResult } from "~/domains/interfaces";

// Mock data for categories
const mockCategories: Category[] = [
	new Category({
		categoryId: 1,
		categoryName: "Technology"
	}),
	new Category({
		categoryId: 2,
		categoryName: "Networking"
	})
];

// Mock data for gatherings
const mockGatherings: Gathering[] = [
	{
		gatheringId: 1,
		name: "Tech Conference 2024",
		description: "Annual technology conference with industry leaders",
		start: new Date("2024-12-15T09:00:00Z"),
		end: new Date("2024-12-15T17:00:00Z"),
		location: "Convention Center",
		organiserId: "org-123",
		cancellationDateTime: null,
		coverSrc: "/images/tech-conference.jpg",
		bookings: [{ ...new Booking(), attendeeId: "abc" }],
		gatheringCategoryDetails: [
			{
				gatheringId: 1,
				categoryId: 1,
				category: mockCategories[0],
				gathering: new Gathering()
			},
			{
				gatheringId: 2,
				categoryId: 2,
				category: mockCategories[1],
				gathering: new Gathering()
			}
		]
	},
	{
		gatheringId: 2,
		name: "Design Workshop",
		description: "Interactive design thinking workshop",
		start: new Date("2024-12-20T14:00:00Z"),
		end: new Date("2024-12-20T18:00:00Z"),
		location: "Creative Studio",
		organiserId: "org-456",
		cancellationDateTime: null,
		coverSrc: "/images/design-workshop.jpg",
		bookings: [],
		gatheringCategoryDetails: []
	},
	{
		gatheringId: 3,
		name: "Networking Event",
		description: "Professional networking event for developers",
		start: new Date("2025-01-10T18:00:00Z"),
		end: new Date("2025-01-10T21:00:00Z"),
		location: "Downtown Hub",
		organiserId: "org-789",
		cancellationDateTime: null,
		coverSrc: "/images/networking.jpg",
		bookings: [],
		gatheringCategoryDetails: [
			{
				gatheringId: 2,
				categoryId: 3,
				category: mockCategories[1],
				gathering: new Gathering()
			}
		]
	}
];

export async function getMockGathering(id: number): Promise<Gathering> {
	return mockGatherings.find((g) => g.gatheringId === id)!;
}

export async function getMockGatherings(
	// eslint-disable-next-line @typescript-eslint/no-unused-vars
	_params: GetGatheringsParams = {}
): Promise<PageResult<Gathering[]>> {
	// Simulate network delay
	await new Promise((resolve) => setTimeout(resolve, 300));
	return {
		totalCount: 10,
		data: [...mockGatherings]
	};
}

export async function createMockGathering(
	gatheringDto: GatheringReqDto,
	// eslint-disable-next-line @typescript-eslint/no-unused-vars
	_coverImg?: File | null
): Promise<Gathering> {
	return {
		...new Gathering(),
		...gatheringDto,
		gatheringCategoryDetails: gatheringDto.gatheringCategoryDetails.map((detail) => ({
			...detail,
			...new GatheringCategoryDetail()
		}))
	};
}

export async function updateMockGathering(
	gatheringId: number,
	gatheringDto: GatheringReqDto,
	// eslint-disable-next-line @typescript-eslint/no-unused-vars
	_coverImg?: File | null
): Promise<Gathering> {
	return {
		...new Gathering(),
		...gatheringDto,
		gatheringId: gatheringId,
		gatheringCategoryDetails: gatheringDto.gatheringCategoryDetails.map((detail) => ({
			...detail,
			...new GatheringCategoryDetail()
		}))
	};
}
