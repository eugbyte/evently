import type { Gathering } from "~/domains/entities";
import axios from "axios";
import { GatheringCategoryDetailReqDto, GatheringReqDto } from "~/domains/models";
import type { PageResult } from "~/domains/interfaces";
import cloneDeep from "lodash.clonedeep";

export interface GetGatheringsParams {
	attendeeId?: string;
	organiserId?: string;
	name?: string;
	startDateBefore?: Date;
	startDateAfter?: Date;
	endDateBefore?: Date;
	endDateAfter?: Date;
	isCancelled?: boolean;
	categoryIds?: number[];
	offset?: number;
	limit?: number;
}

export async function getGatherings(params: GetGatheringsParams): Promise<PageResult<Gathering[]>> {
	const queryParams: Record<string, any> = cloneDeep(params);

	const response = await axios.get<Gathering[]>("/api/v1/Gatherings", { params: queryParams });
	const gatherings: Gathering[] = response.data;
	for (const gathering of gatherings) {
		gathering.start = new Date(gathering.start);
		gathering.end = new Date(gathering.end);
	}

	const totalCount: number = parseInt(response.headers["x-total-count"]);
	return {
		totalCount,
		data: gatherings
	};
}

export async function getGathering(id: number): Promise<Gathering> {
	const response = await axios.get<Gathering>(`/api/v1/Gatherings/${id}`);
	const gathering: Gathering = response.data;
	gathering.start = new Date(gathering.start);
	gathering.end = new Date(gathering.end);
	return gathering;
}

export async function createGathering(
	gatheringDto: GatheringReqDto,
	coverImg?: File | null
): Promise<Gathering> {
	const formData: FormData = toFormData(gatheringDto, coverImg);

	const response = await axios.post<Gathering>(`/api/v1/Gatherings`, formData, {
		headers: { "Content-Type": "multipart/form-data" }
	});
	return response.data;
}

export async function updateGathering(
	gatheringId: number,
	gatheringDto: GatheringReqDto,
	coverImg?: File | null
): Promise<Gathering> {
	const formData: FormData = toFormData(gatheringDto, coverImg);

	const response = await axios.put<Gathering>(`/api/v1/Gatherings/${gatheringId}`, formData, {
		headers: { "Content-Type": "multipart/form-data" }
	});
	return response.data;
}

function toFormData(gatheringDto: GatheringReqDto, coverImg?: File | null): FormData {
	const formData = new FormData();
	for (const [key, value] of Object.entries(gatheringDto)) {
		if (value != null) {
			formData.set(key, value);
		}
	}

	formData.delete("gatheringCategoryDetails");
	if (gatheringDto.gatheringCategoryDetails.length === 0) {
		formData.set("gatheringCategoryDetails", "[]");
	}

	for (let i = 0; i < gatheringDto.gatheringCategoryDetails.length; i++) {
		const detail: GatheringCategoryDetailReqDto = gatheringDto.gatheringCategoryDetails[i];
		for (const [key, value] of Object.entries(detail)) {
			formData.set(`gatheringCategoryDetails[${i}].${key}`, value);
		}
	}

	if (coverImg != null) {
		formData.set("coverImg", coverImg, coverImg.name);
	}
	formData.set("start", gatheringDto.start.toISOString());
	formData.set("end", gatheringDto.end.toISOString());
	if (gatheringDto.cancellationDateTime != null) {
		formData.set("cancellationDateTime", gatheringDto.cancellationDateTime.toISOString());
	}
	return formData;
}
