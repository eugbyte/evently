import { createFileRoute } from "@tanstack/react-router";
import { type JSX, useState } from "react";
import { Booking, Gathering } from "~/domains/entities";
import { getBookings, type GetBookingsParams } from "~/lib/services";
import { Card, Tabs, TabState } from "~/lib/components";
import cloneDeep from "lodash.clonedeep";
import { useQuery } from "@tanstack/react-query";

export const Route = createFileRoute("/bookings/(auth)/attending/")({
	loader: ({ context }) => ({ account: context.account }),
	component: GetBookingsPage,
	pendingComponent: () => (
		<div className="h-full">
			<progress className="progress w-full"></progress>
		</div>
	)
});

export function GetBookingsPage(): JSX.Element {
	const { account } = Route.useRouteContext();
	const accountId: string | undefined = account?.id;

	const [tab, setTab] = useState(0);

	const [bkQueryParams, setBkQueryParams] = useState<GetBookingsParams>({
		attendeeId: accountId,
		gatheringEndAfter: new Date(),
		isCancelled: false
	});
	const { data: _bookings, isLoading } = useQuery({
		queryKey: ["getBookings", bkQueryParams, tab],
		queryFn: async (): Promise<Booking[]> => {
			const { data: bookings } = await getBookings(bkQueryParams);
			return bookings;
		}
	});

	let gatherings: Gathering[] = cloneDeep(_bookings ?? []).map((booking) => booking.gathering);
	gatherings = gatherings.sort((a, b) => b.end.getTime() - a.end.getTime());

	const handleTabChange = (_tab: number) => {
		setTab(_tab);

		switch (_tab) {
			case TabState.Upcoming: {
				setBkQueryParams({
					attendeeId: accountId,
					gatheringEndAfter: new Date(),
					isCancelled: false
				});
				break;
			}
			case TabState.Past: {
				setBkQueryParams({ attendeeId: accountId, gatheringEndBefore: new Date() });
				break;
			}
		}
	};

	return (
		<div className="h-full p-1">
			<Tabs tab={tab} handleTabChange={handleTabChange} />
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
