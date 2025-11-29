import { Icon } from "@iconify/react/dist/iconify.js";
import { DateTime } from "luxon";
import type { JSX } from "react";
import { Gathering } from "~/domains/entities";

interface JumbotronProps {
	gathering: Gathering;
}

export function Jumbotron({ gathering }: JumbotronProps): JSX.Element {
	const start: DateTime = DateTime.fromJSDate(gathering.start);
	const end: DateTime = DateTime.fromJSDate(gathering.end);

	return (
		<div className="hero from-primary/10 to-secondary/10 mt-4 rounded-xl bg-gradient-to-r">
			<div className="hero-content py-8 text-center">
				<div className="max-w-md">
					<h1 className="text-primary mb-2 text-3xl font-bold">{gathering.name}</h1>
					<div className="text-base-content/80 flex flex-col gap-2">
						<div className="flex items-center justify-center gap-2">
							<Icon icon="material-symbols:location-on" className="text-accent" />
							<span>{gathering.location}</span>
						</div>
						<div className="flex items-center justify-center gap-2">
							<Icon icon="material-symbols:calendar-today" className="text-accent" />
							<span>{`${start.toLocaleString(DateTime.DATETIME_MED)} — ${end.toLocaleString(DateTime.DATETIME_MED)}`}</span>
						</div>
					</div>
				</div>
			</div>
		</div>
	);
}
