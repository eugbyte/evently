import { createFileRoute } from "@tanstack/react-router";
import { type JSX, useState } from "react";
import { useQuery } from "@tanstack/react-query";
import { Category, Gathering } from "~/domains/entities";
import { getCategories, getGatherings, type GetGatheringsParams } from "~/lib/services";
import { Card } from "~/lib/components";
import type { PageResult } from "~/domains/interfaces";
import { FilterBar } from "~/routes/gatherings/-components";
import { Icon } from "@iconify/react/dist/offline";

export const Route = createFileRoute("/gatherings/")({
	component: GatheringsPage,
	loader: async () => {
		let categories: Category[] = [];
        try {
            categories =  await getCategories();
        } catch (error) {
            console.error(error);
        }
		return { categories };
	},
	pendingComponent: () => (
		<div className="h-full">
			<progress className="progress w-full"></progress>
		</div>
	)
});

export function GatheringsPage(): JSX.Element {
	const { categories } = Route.useLoaderData();
	const { account } = Route.useRouteContext();
	const accountId: string | undefined = account?.id;

	const pageSize = 6;
	const [page, setPage] = useState(1);
	const [queryParams, setQueryParams] = useState<GetGatheringsParams>({
		endDateAfter: new Date(),
		offset: 0,
		limit: pageSize
	});
	const { data, isLoading } = useQuery({
		queryKey: ["getGatherings", queryParams],
		queryFn: (): Promise<PageResult<Gathering[]>> => {
			return getGatherings(queryParams);
		}
	});
	const gatherings: Gathering[] = data == null ? [] : data.data;
	const totalCount: number = data == null ? 0 : data.totalCount;

	let maxPage = Math.ceil(totalCount / pageSize);
	maxPage = Math.max(1, maxPage);
	const onPrevPage = () => {
		let prevPage = page - 1;
		prevPage = Math.max(1, prevPage);
		setPage(prevPage);

		setQueryParams({
			...queryParams,
			offset: (prevPage - 1) * pageSize,
			limit: pageSize
		});
	};

	const onNextPage = () => {
		let nextPage = page + 1;
		nextPage = Math.min(maxPage, nextPage);
		setPage(nextPage);

		setQueryParams({
			...queryParams,
			offset: (nextPage - 1) * pageSize,
			limit: pageSize
		});
	};

	const handleParamsChange = (queryParams: GetGatheringsParams) => {
		setQueryParams({
			...queryParams,
			offset: 0,
			limit: pageSize
		});
		setPage(1);
	};

	let filterCount: number = queryParams.categoryIds?.length ?? 0;
	filterCount += queryParams.name ? 1 : 0;
	filterCount += queryParams.startDateAfter ? 1 : 0;
	filterCount += queryParams.endDateBefore ? 1 : 0;

	return (
		<div className="h-full">
			<div
				tabIndex={0}
				className="collapse-arrow from-base-100 to-base-200 border-base-300 collapse mx-auto mb-4 w-11/12 rounded-lg border bg-gradient-to-r shadow-sm"
			>
				<input type="checkbox" defaultChecked={window.innerWidth > 768} />
				<div className="collapse-title text-primary hover:bg-base-200/50 flex items-center gap-2 text-lg font-bold transition-colors duration-200">
					<Icon
						icon="material-symbols:filter-list"
						width="24"
						height="24"
						className="text-primary"
					/>
					<span>Filters</span>
					<div className="badge badge-secondary badge-sm ml-auto">{filterCount}</div>
				</div>
				<div className="collapse-content">
					<div className="pt-2">
						<FilterBar
							categories={categories}
							queryParams={queryParams}
							handleParamsChange={handleParamsChange}
						/>
					</div>
				</div>
			</div>

			{isLoading ? (
				<progress className="progress w-full"></progress>
			) : (
				<div className="my-4 grid grid-cols-1 content-evenly justify-items-center gap-4 lg:grid-cols-2 xl:grid-cols-3">
					{gatherings.map((gathering, index) => (
						<Card
							key={gathering.gatheringId + "-" + index}
							gathering={gathering}
							accountId={accountId}
						/>
					))}
				</div>
			)}
			<div id="pagination" className="fixed bottom-20 left-1/2 -translate-x-1/2 sm:bottom-10">
				<div className="join border-primary rounded-sm border">
					<button className="join-item btn bg-base-100" onClick={onPrevPage} disabled={page === 1}>
						«
					</button>
					<button className="join-item btn bg-base-100">Page {page}</button>
					<button
						className="join-item btn bg-base-100"
						onClick={onNextPage}
						disabled={page === maxPage}
					>
						»
					</button>
				</div>
			</div>
			<div className="h-10"></div>
		</div>
	);
}
