import { Booking, Gathering } from "~/domains/entities";
import {
	downloadFile,
	getBookings,
	type GetBookingsParams,
	getGathering,
	toIsoDateTimeString
} from "~/lib/services";
import { createFileRoute, Link } from "@tanstack/react-router";
import { type JSX, useState } from "react";
import uniqby from "lodash.uniqby";
import cloneDeep from "lodash.clonedeep";
import { Icon } from "@iconify/react";
import { json2csv } from "json-2-csv";
import { useQuery } from "@tanstack/react-query";
import { BookingsTable, Jumbotron, StatsCard } from "./-components";
import { useInterval } from "usehooks-ts";
import type { PageResult } from "~/domains/interfaces";

export const Route = createFileRoute("/bookings/(auth)/hosting/$gatheringId/dashboard/")({
	loader: async ({ params }) => {
		const gatheringId: number = parseInt(params.gatheringId);
		const gathering: Gathering = (await getGathering(gatheringId)) ?? new Gathering();
		return { gathering };
	},
	component: DashboardPage
});

export function DashboardPage(): JSX.Element {
	const { gathering } = Route.useLoaderData();
	const oneMinute = 60 * 1000;

	const [queryParams] = useState<GetBookingsParams>({ gatheringId: gathering.gatheringId });
	const { data, refetch, isLoading } = useQuery({
		queryKey: ["getBookings", queryParams],
		queryFn: (): Promise<PageResult<Booking[]>> => getBookings(queryParams)
	});

	let bookings: Booking[] = cloneDeep(data?.data ?? []);
	bookings = bookings.sort((a, b) => b.creationDateTime.getTime() - a.creationDateTime.getTime());
	uniqby(bookings, (b) => b.bookingId);
	const registrationCount = data?.totalCount ?? 0;
	const checkInCount = bookings.filter((b) => b.checkInDateTime !== null).length;

	useInterval(async () => {
		await refetch();
	}, oneMinute);

	const downloadCsv = () => {
		const rows = bookings.map((booking) => ({
			Name: booking.accountDto.name,
			Email: booking.accountDto.email,
			RegistrationDate: toIsoDateTimeString(booking.creationDateTime),
			CheckInDate: toIsoDateTimeString(booking.checkInDateTime)
		}));

		const csv: string = json2csv(rows, {});
		const blob = new Blob([csv], { type: "text/csv;charset=utf-8," });
		downloadFile(blob, "registrations");
	};

	return (
		<div className="bg-base-100 h-full p-4 md:p-6">
			<div className="mx-auto max-w-6xl">
				{isLoading && <progress className="progress w-full"></progress>}
				{/* Header Section */}
				<div className="mb-8">
					<Jumbotron gathering={gathering} />
				</div>

				{/* Main Dashboard Card */}
				<div className="grid grid-cols-1 gap-6 lg:grid-cols-2">
					{/* Registration Progress Card */}
					<StatsCard checkInCount={checkInCount} registrationCount={registrationCount} />

					{/* Actions Card */}
					<div className="card bg-base-200 shadow-xl">
						<div className="card-body">
							<h2 className="card-title flex items-center gap-2">
								<Icon icon="material-symbols:settings" className="text-secondary" />
								Quick Actions
							</h2>

							<div className="mt-4 space-y-4">
								<Link
									className="btn btn-accent btn-outline btn-block justify-start gap-3"
									to="/bookings/hosting/$gatheringId/dashboard/scan"
									params={{ gatheringId: gathering.gatheringId.toString() }}
								>
									<Icon icon="material-symbols:qr-code-scanner" width="20" height="20" />
									QR Scanner
								</Link>

								<button
									className="btn btn-primary btn-block justify-start gap-3"
									onClick={downloadCsv}
								>
									<Icon icon="material-symbols:download-2" width="20" height="20" />
									Download Registration Data
									<div className="badge badge-neutral ml-auto">{registrationCount}</div>
								</button>

								<button
									className="btn btn-secondary btn-outline btn-block justify-start gap-3"
									onClick={() => refetch()}
								>
									<Icon icon="material-symbols:refresh" width="20" height="20" />
									Refresh Data
								</button>
							</div>

							<div className="divider"></div>

							<div className="alert alert-info">
								<Icon icon="material-symbols:info" />
								<span className="text-sm">Data refreshes automatically every minute</span>
							</div>
						</div>
					</div>
				</div>

				{/* Recent Activity Card (Optional - if you want to show recent registrations) */}
				{bookings.length > 0 && <BookingsTable bookings={bookings} />}
			</div>
		</div>
	);
}
