import type { JSX } from "react";
import { Icon } from "@iconify/react";
import { Booking, Category, Gathering } from "~/domains/entities";
import { DateTime } from "luxon";

interface JumbotronProps {
	booking: Booking | null;
	gathering: Gathering;
	accountId?: string | null;
}

export function Jumbotron({ gathering, accountId, booking }: JumbotronProps): JSX.Element {
	const { name: title, description, gatheringCategoryDetails } = gathering;
	const start: DateTime = DateTime.fromJSDate(gathering.start);
	const end: DateTime = DateTime.fromJSDate(gathering.end);
	const categories: Category[] = gatheringCategoryDetails.map((detail) => detail.category);
	const isOrganiser: boolean = booking != null && booking.gathering.organiserId === accountId;

	return (
		<>
			<h2 className="card-title flex-wrap justify-between">
				<span>{title}</span>
				<div>
					{isOrganiser && <div className="badge badge-outline badge-secondary">I'm the host</div>}
					{isOrganiser && booking?.cancellationDateTime != null && (
						<div className="badge badge-outline badge-error">Cancelled</div>
					)}
				</div>
			</h2>
			<p>{description}</p>
			<div className="flex flex-row items-center space-x-2 text-xs">
				<Icon icon="material-symbols:location-on" width="24" height="24" className="inline">
					Location
				</Icon>
				<span>{gathering.location}</span>
			</div>
			<div className="flex flex-row items-center space-x-2 text-xs">
				<Icon icon="mingcute:time-fill" width="24" height="24" />
				<span>{`${start.toLocaleString(DateTime.DATETIME_MED)} — ${end.toLocaleString(DateTime.DATETIME_MED)}`}</span>
			</div>
			<div className="flex flex-row flex-wrap space-x-2">
				{categories.map((category) => (
					<div className="badge badge-soft badge-info" key={category.categoryId}>
						{category.categoryName}
					</div>
				))}
			</div>
		</>
	);
}
