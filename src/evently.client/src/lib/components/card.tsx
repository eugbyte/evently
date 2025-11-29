import Placeholder1 from "~/lib/assets/event_placeholder_1.webp";
import Placeholder2 from "~/lib/assets/event_placeholder_2.png";
import { type JSX } from "react";
import { Link } from "@tanstack/react-router";
import { Category, Gathering } from "~/domains/entities";
import { Icon } from "@iconify/react";
import { DateTime } from "luxon";
import { hashString } from "~/lib/services";

export interface CardProps {
	gathering: Gathering;
	accountId?: string | null;
}

export function Card({ gathering, accountId }: CardProps): JSX.Element {
	let { name: title, description, coverSrc: imgSrc } = gathering;
	const categories: Category[] = gathering.gatheringCategoryDetails.map(
		(detail) => detail.category
	);

	if (imgSrc == null || imgSrc.length === 0) {
		const hash: number = hashString(gathering.name);
		imgSrc = hash % 2 === 0 ? Placeholder1 : Placeholder2;
	}

	if (title.length > 30) {
		title = title.substring(0, 30) + "...";
	}
	if (description.length > 190) {
		description = description.substring(0, 100) + "...";
	}
	const isOrganiser: boolean = gathering.organiserId === accountId;
	const start: DateTime = DateTime.fromJSDate(gathering.start);
	const end: DateTime = DateTime.fromJSDate(gathering.end);
	const isCancelled: boolean = gathering.cancellationDateTime != null;
	return (
		<div className="card bg-base-200 w-96 text-white shadow-sm">
			<figure>
				<img src={imgSrc} alt="Event Image" className="h-48 w-96" />
			</figure>
			<div className="card-body">
				<div className="flex flex-row flex-wrap justify-between">
					<h2 className="card-title">{title}</h2>
					{isCancelled && <div className="badge badge-outline">Cancelled</div>}
					{isOrganiser && <div className="badge badge-outline badge-secondary">I'm the host</div>}
				</div>
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
				<div className="flex flex-row flex-wrap gap-x-2 gap-y-1">
					{categories.map((category) => (
						<div className="badge badge-soft badge-info" key={category.categoryId}>
							{category.categoryName}
						</div>
					))}
				</div>
				<div className="card-actions justify-end">
					<Link
						className="btn btn-primary"
						to={`/gatherings/$gatheringId`}
						params={{ gatheringId: gathering.gatheringId.toString() }}
					>
						View
					</Link>
				</div>
			</div>
		</div>
	);
}
