import type { JSX } from "react";
import { Booking } from "~/domains/entities";
import { toIsoDateTimeString } from "~/lib/services";
import { Icon } from "@iconify/react";

interface BookingsTableProps {
	bookings: Booking[];
}

export function BookingsTable({ bookings }: BookingsTableProps): JSX.Element {
	return (
		<div className="card bg-base-200 mt-6 shadow-xl">
			<div className="card-body">
				<h2 className="card-title flex items-center gap-2">
					<Icon icon="material-symbols:history" className="text-accent" />
					Recent Registrations
				</h2>

				<div className="overflow-x-auto">
					<table className="table-sm table">
						<thead>
							<tr>
								<th>Name</th>
								<th>Email</th>
								<th>Registration Date</th>
								<th>Status</th>
							</tr>
						</thead>
						<tbody>
							{bookings.slice(0, 5).map((booking, index) => (
								<tr key={index} className="hover">
									<td>{booking.accountDto.name}</td>
									<td className="text-base-content/70 text-sm">{booking.accountDto.email}</td>
									<td className="text-sm">{toIsoDateTimeString(booking.creationDateTime)}</td>
									<td>
										{booking.checkInDateTime ? (
											<div className="badge badge-success badge-sm">Checked In</div>
										) : (
											<div className="badge badge-warning badge-sm">Registered</div>
										)}
									</td>
								</tr>
							))}
						</tbody>
					</table>
				</div>

				{bookings.length > 5 && (
					<div className="mt-4 text-center">
						<span className="text-base-content/70 text-sm">
							Showing 5 of {bookings.length} registrations
						</span>
					</div>
				)}
			</div>
		</div>
	);
}
