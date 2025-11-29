import { createFileRoute, Link, useNavigate } from "@tanstack/react-router";
import { type JSX, useRef } from "react";
import { Booking, Gathering } from "~/domains/entities";
import {
	cancelBooking,
	createBooking,
	getBookings,
	getGathering,
	hashString,
	updateGathering
} from "~/lib/services";
import { useMutation } from "@tanstack/react-query";
import { BookingReqDto, GatheringReqDto } from "~/domains/models";
import { CancellationDialog, Jumbotron, QrDialog } from "./-components";
import Placeholder1 from "~/lib/assets/event_placeholder_1.webp";
import Placeholder2 from "~/lib/assets/event_placeholder_2.png";

export const Route = createFileRoute("/gatherings/$gatheringId/")({
	loader: async ({ params, context }) => {
		const accountId: string | undefined = context.account?.id;
		const gatheringId: number = parseInt(params.gatheringId);
		const gathering: Gathering | null = await getGathering(gatheringId);
		let booking: Booking | null = null;
		console.log({ accountId });
		if (accountId != null && accountId.trim().length > 0) {
			const { data: bookings } = await getBookings({
				attendeeId: accountId,
				gatheringId,
				isCancelled: false
			});
			booking = bookings.length > 0 ? bookings[0] : null;
		}

		return {
			gathering,
			booking
		};
	},
	component: GatheringPage,
	pendingComponent: () => (
		<div className="h-full px-2">
			<progress className="progress w-full"></progress>
		</div>
	)
});

export function GatheringPage(): JSX.Element {
	const { account } = Route.useRouteContext();
	const accountId: string | undefined = account?.id;

	const { gathering, booking: _booking } = Route.useLoaderData();
	console.log({ accountId });
	const navigate = useNavigate();

	let { coverSrc: imgSrc } = gathering;
	if (imgSrc == null || imgSrc.trim().length === 0) {
		const hash: number = hashString(gathering.name);
		imgSrc = hash % 2 === 0 ? Placeholder1 : Placeholder2;
	}

	const { mutate: handleRegister, data } = useMutation({
		mutationFn: () => {
			const dto = new BookingReqDto();
			dto.attendeeId = accountId ?? "";
			dto.gatheringId = gathering.gatheringId;
			return createBooking(dto);
		}
	});

	const booking: Booking | null = data ?? _booking;
	const isOrganiser: boolean = accountId != null && gathering.organiserId === accountId;
	const isAttendee: boolean = !isOrganiser;

	const qrDialogRef = useRef<HTMLDialogElement>(null);
	const cancellationDialogRef = useRef<HTMLDialogElement>(null);
	const cancelRegistration = async () => {
		if (booking == null) {
			return;
		}
		await cancelBooking(booking?.bookingId ?? "");
		navigate({
			to: `/gatherings/${gathering.gatheringId}`,
			reloadDocument: true
		});
	};
	const cancelGathering = async () => {
		if (!isOrganiser) {
			return;
		}

		const gatheringDto = new GatheringReqDto(gathering);
		gatheringDto.cancellationDateTime = new Date();
		await updateGathering(gathering.gatheringId, gatheringDto);
		await navigate({
			to: `/gatherings/${gathering.gatheringId}`,
			reloadDocument: true
		});
	};

	return (
		<div className="p-2">
			<div className="card bg-base-200 card-lg container mx-auto w-full text-white shadow-sm md:w-2/3">
				<figure className="mt-0 sm:mt-5">
					<img src={imgSrc} alt="Event Image" className="h-60 w-120 rounded-lg" />
				</figure>
				<div className="card-body">
					<Jumbotron gathering={gathering} accountId={accountId} booking={booking} />
					<div className="divider"></div>
					{/*registered attendee*/}
					{isAttendee && booking != null && (
						<div className="card-actions justify-between">
							<button className="btn btn-info" onClick={() => qrDialogRef.current?.showModal()}>
								View QR Ticket
							</button>
							<button
								className="btn btn-error"
								onClick={() => cancellationDialogRef.current?.showModal()}
							>
								Cancel Registration
							</button>
						</div>
					)}
					{/*unregistered attendee*/}
					{isAttendee && booking == null && (
						<div className="card-actions justify-between">
							<button className="btn btn-primary" onClick={() => handleRegister()}>
								Register
							</button>
						</div>
					)}
					{isOrganiser && gathering.cancellationDateTime == null && (
						<>
							<div className="card-actions justify-between">
								<Link
									className="btn btn-primary"
									to={`/gatherings/$gatheringId/update`}
									params={{ gatheringId: gathering.gatheringId.toString() }}
								>
									Edit
								</Link>
								<button
									className="btn btn-error"
									onClick={() => cancellationDialogRef.current?.showModal()}
								>
									Cancel Event
								</button>
							</div>
							<div className="card-actions my-2 justify-between">
								<Link
									to="/bookings/hosting/$gatheringId/dashboard"
									params={{ gatheringId: gathering.gatheringId.toString() }}
									className="btn btn-outline btn-accent"
								>
									Manage Registrations and Check Ins
								</Link>
							</div>
						</>
					)}
					<QrDialog qrDialogRef={qrDialogRef} booking={booking} />
					<CancellationDialog
						cancellationDialogRef={cancellationDialogRef}
						handleCancel={async () => {
							if (isOrganiser) {
								await cancelGathering();
							} else if (isAttendee) {
								await cancelRegistration();
							}
						}}
					/>
				</div>
			</div>
		</div>
	);
}
