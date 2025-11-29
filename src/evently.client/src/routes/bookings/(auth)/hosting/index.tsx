import { createFileRoute, Link } from "@tanstack/react-router";
import { type JSX, useState } from "react";
import { Gathering } from "~/domains/entities";
import { getGatherings, type GetGatheringsParams } from "~/lib/services";
import { Card, Tabs, TabState } from "~/lib/components";
import { useQuery } from "@tanstack/react-query";
import cloneDeep from "lodash.clonedeep";

export const Route = createFileRoute("/bookings/(auth)/hosting/")({
	component: GetHostedGatheringsPage,
	pendingComponent: () => (
		<div className="h-full">
			<progress className="progress w-full"></progress>
		</div>
	)
});

export function GetHostedGatheringsPage(): JSX.Element {
	const { account } = Route.useRouteContext();
	const accountId: string | undefined = account?.id;

	const [tab, setTab] = useState(0);

	const [queryParams, setQueryParams] = useState<GetGatheringsParams>({
		organiserId: accountId ?? "",
		endDateAfter: new Date(),
		isCancelled: false
	});
	const { data: _hostedGatherings, isLoading: isHostedGatheringLoading } = useQuery({
		queryKey: ["getHostedGatherings", queryParams],
		queryFn: async (): Promise<Gathering[]> => {
			const result = await getGatherings(queryParams);
			return result.data;
		}
	});
	const isLoading: boolean = isHostedGatheringLoading;

	let gatherings: Gathering[] = cloneDeep(_hostedGatherings ?? []);
	gatherings = gatherings.sort((a, b) => b.end.getTime() - a.end.getTime());

	const handleTabChange = (_tab: number) => {
		setTab(_tab);

		switch (_tab) {
			case TabState.Upcoming: {
				setQueryParams({
					organiserId: accountId ?? "",
					endDateAfter: new Date(),
					isCancelled: false
				});
				break;
			}
			case TabState.Past: {
				setQueryParams({
					organiserId: accountId ?? "",
					endDateBefore: new Date(),
					isCancelled: false
				});
				break;
			}
		}
	};

	return (
		<div className="h-full p-1">
			<Tabs tab={tab} handleTabChange={handleTabChange} />
			<div className="flex w-full flex-row justify-end px-4">
				<Link to="/gatherings/create" className="btn btn-success">
					Host Event
				</Link>
			</div>
			{gatherings.length === 0 && !isLoading && <div className="text-center">None Found</div>}
			{isLoading ? (
				<progress className="progress mx-2 w-full"></progress>
			) : (
				<div className="my-4 grid grid-cols-1 content-evenly justify-items-center gap-4 lg:grid-cols-2 xl:grid-cols-3">
					{gatherings.map((gathering) => (
						<Card key={gathering.gatheringId} gathering={gathering} accountId={accountId} />
					))}
				</div>
			)}
		</div>
	);
}
