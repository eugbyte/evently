import { useForm } from "@tanstack/react-form";
import { GatheringReqDto } from "~/domains/models";

export function useGatheringForm(
	defaultGathering: GatheringReqDto,
	onSubmit: (value: GatheringReqDto) => Promise<void>
) {
	return useForm({
		defaultValues: defaultGathering,
		onSubmit: async ({ value }) => {
			// Do something with form data
			try {
				await onSubmit(value);
			} catch (e) {
				console.error(e);
			}
		}
	});
}

export type GatheringForm = ReturnType<typeof useGatheringForm>;
