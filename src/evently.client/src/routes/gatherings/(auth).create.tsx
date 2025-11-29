import { createFileRoute } from "@tanstack/react-router";
import { Category, Gathering } from "~/domains/entities";
import { type JSX, useState } from "react";
import { authenticateRoute, createGathering, getCategories, sleep } from "~/lib/services";
import { type GatheringForm as IGatheringForm, useGatheringForm } from "./-services";
import { GatheringReqDto, ToastContent } from "~/domains/models";
import { GatheringForm } from "~/routes/gatherings/-components";

export const Route = createFileRoute("/gatherings/(auth)/create")({
	beforeLoad: ({ context }) => authenticateRoute(context.account, window.location.href),
	loader: async () => {
		const categories: Category[] = await getCategories();
		return { categories };
	},
	component: CreateGatheringPage
});

function CreateGatheringPage(): JSX.Element {
	const { categories } = Route.useLoaderData();
	const { account } = Route.useRouteContext();
	const accountId: string | undefined = account?.id;

	const navigate = Route.useNavigate();

	const gathering = new Gathering();
	gathering.organiserId = accountId ?? "";
	const defaultGathering: GatheringReqDto = {
		...gathering
	};
	// need to separate file field as Tanstack Form does not support file upload
	const [file, setFile] = useState<File | null>(null);
	const [toastMsg, setToastMsg] = useState(new ToastContent(false));

	const onSubmit = async (values: GatheringReqDto): Promise<void> => {
		setToastMsg(new ToastContent(true, "Creating..."));
		const { gatheringId } = await createGathering(values, file);
		setToastMsg(new ToastContent(true, "Successfully created. Redirecting..."));
		await sleep(1500);

		navigate({ to: `/gatherings/${gatheringId}` });
	};
	const form: IGatheringForm = useGatheringForm(defaultGathering, onSubmit);
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
