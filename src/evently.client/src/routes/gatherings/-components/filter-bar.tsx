import React, { type JSX } from "react";
import { type GetGatheringsParams, toIsoDateString } from "~/lib/services";
import { Category } from "~/domains/entities";
import { DateTime } from "luxon";
import { Icon } from "@iconify/react";

interface FilterBarProps {
	categories: Category[];
	queryParams: GetGatheringsParams;
	handleParamsChange: (queryParams: GetGatheringsParams) => void;
}

export function FilterBar({
	categories,
	queryParams,
	handleParamsChange
}: FilterBarProps): JSX.Element {
	const dropdownButtonStyle = { anchorName: "--anchor-1" } as React.CSSProperties;
	const popoverStyle = { positionAnchor: "--anchor-1" } as React.CSSProperties;

	return (
		<div className="mt-1 flex flex-wrap justify-center gap-2" data-testid="filter-bar">
			<div>
				<button
					className="btn btn-outline btn-primary"
					popoverTarget="popover-1"
					style={dropdownButtonStyle}
				>
					Categories
					<Icon icon="material-symbols:arrow-drop-down" width="24" height="24" />
				</button>
				<ul
					className="dropdown menu rounded-box bg-base-100 shadow-sm"
					popover="auto"
					id="popover-1"
					style={popoverStyle}
				>
					{categories.map((category) => {
						const oldCategoryIds: number[] = queryParams.categoryIds ?? [];
						const wasChecked = oldCategoryIds.some((id) => id === category.categoryId);
						return (
							<li key={category.categoryId}>
								<label className="label">
									<input
										type="checkbox"
										checked={wasChecked}
										className="checkbox"
										onChange={(e) => {
											const checked = e.target.checked;
											let newValue: number[] = [];
											if (checked) {
												newValue = [...oldCategoryIds, category.categoryId];
											} else {
												newValue = oldCategoryIds.filter((id) => id !== category.categoryId);
											}
											handleParamsChange({
												...queryParams,
												categoryIds: newValue
											});
										}}
									/>
									{category.categoryName}
								</label>
							</li>
						);
					})}
				</ul>
				<p className="block text-left text-xs">
					{(queryParams.categoryIds?.length ?? 0) > 0
						? `${queryParams.categoryIds?.length} Selected`
						: ""}
				</p>
			</div>

			<label className="floating-label input input-bordered input-primary hover:input-primary-focus flex w-full items-center gap-2 transition-colors sm:w-auto">
				<span>Search Gatherings</span>
				<input
					type="search"
					className="grow"
					placeholder="Search gatherings..."
					value={queryParams.name ?? ""}
					onChange={(e) => {
						const text: string = e.target.value;
						handleParamsChange({
							...queryParams,
							name: text
						});
					}}
				/>
			</label>

			<label className="input input-bordered input-primary hover:input-primary-focus w-full transition-colors sm:w-auto">
				<span className="label">Start date</span>
				<input
					type="date"
					value={
						queryParams.startDateAfter == null ? "" : toIsoDateString(queryParams.startDateAfter)
					}
					onChange={(e) => {
						const value: DateTime<boolean> = DateTime.fromISO(e.target.value);
						const date: Date | null = !value.isValid ? null : value.startOf("day").toJSDate();
						handleParamsChange({
							...queryParams,
							startDateAfter: date as any
						});
					}}
				/>
			</label>
			<label className="input input-bordered input-primary hover:input-primary-focus w-full transition-colors sm:w-auto">
				<span className="label">End date</span>
				<input
					type="date"
					value={
						queryParams.endDateBefore == null ? "" : toIsoDateString(queryParams.endDateBefore)
					}
					onChange={(e) => {
						const value: DateTime<boolean> = DateTime.fromISO(e.target.value);
						const date: Date | null = !value.isValid ? null : value.endOf("day").toJSDate();
						handleParamsChange({
							...queryParams,
							endDateBefore: date as any
						});
					}}
				/>
			</label>

			<button
				className="btn btn-outline btn-primary hover:btn-primary flex items-center transition-colors sm:ml-10"
				onClick={() => {
					handleParamsChange({
						endDateAfter: new Date()
					});
				}}
			>
				<Icon icon="material-symbols:refresh" width="20" height="20" />
				Reset Filters
			</button>
		</div>
	);
}
