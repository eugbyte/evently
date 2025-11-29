import { createFileRoute } from "@tanstack/react-router";
import { Category, Gathering } from "~/domains/entities";
import { type JSX, useEffect, useState } from "react";
import {
	authenticateRoute,
	fetchFile,
	getCategories,
	getGathering,
	sleep,
	updateGathering
} from "~/lib/services";
import {
	type GatheringForm as IGatheringForm,
	useGatheringForm
} from "~/routes/gatherings/-services";
import { GatheringReqDto, ToastContent } from "~/domains/models";
import { GatheringForm } from "~/routes/gatherings/-components";

export const Route = createFileRoute("/gatherings/$gatheringId/(auth)/update")({
	beforeLoad: ({ context }) => authenticateRoute(context.account, window.location.href),
	loader: async ({ params }) => {
		const gatheringId: number = parseInt(params.gatheringId);
		let gathering: Gathering | null = await getGathering(gatheringId);
		gathering = gathering ?? new Gathering();
		const categories: Category[] = await getCategories();
		return { gathering, categories };
	},
	component: UpdateGatheringPage
});

function UpdateGatheringPage(): JSX.Element {
	const { gathering, categories } = Route.useLoaderData();
	const navigate = Route.useNavigate();
	const defaultGathering: GatheringReqDto = {
		...gathering
	};
	// need to separate file field as Tanstack Form does not support file upload
	const [file, setFile] = useState<File | null>(null);
	const [toastMsg, setToastMsg] = useState(new ToastContent(false));

	const onSubmit = async (values: GatheringReqDto): Promise<void> => {
		setToastMsg(new ToastContent(true, "Updating..."));
		await updateGathering(values.gatheringId, values, file);
		setToastMsg(new ToastContent(true, "Successfully updated. Redirecting..."));
		await sleep(1500);
		navigate({ to: `/gatherings/${gathering.gatheringId}` });
	};
	const form: IGatheringForm = useGatheringForm(defaultGathering, onSubmit);

	useEffect(() => {
		// set the initial file
		(async () => {
			const coverSrc: string | null = gathering.coverSrc ?? null;
			if (coverSrc == null) {
				return;
			}
			const file: File = await fetchFile(coverSrc);
			setFile(file);
		})();
	}, [gathering]);
	return (
		<>
			<GatheringForm file={file} setFile={setFile} form={form} categories={categories} />
			{toastMsg.show && (
				<div className="toast toast-center">
					<div className="alert alert-success">
						<span>{toastMsg.message}</span>
					</div>
				</div>
			)}
		</>
	);
}
