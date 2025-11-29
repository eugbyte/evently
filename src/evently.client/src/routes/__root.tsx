import { createRootRouteWithContext, Outlet } from "@tanstack/react-router";
import { TanStackRouterDevtools } from "@tanstack/react-router-devtools";
import { Navbar, NotFound } from "~/lib/components";
import { type JSX, useEffect } from "react";
import { getAccount } from "~/lib/services";
import { Account } from "~/domains/entities";
import type { RouteContext } from "~/domains/interfaces/route-context.ts";
import polyfill from "@oddbird/css-anchor-positioning/fn";

export const Route = createRootRouteWithContext<RouteContext>()({
	beforeLoad: async () => {
		const account: Account | null = await getAccount();
		return { account };
	},
	component: App,
	notFoundComponent: () => <NotFound />
});

export function App(): JSX.Element {
	useEffect(() => {
		polyfill({
			elements: undefined,
			excludeInlineStyles: false,
			useAnimationFrame: false
		}).catch((err) => console.error(err));
	}, []);
	return (
		<div className="h-screen">
			<Navbar />
			<div className="h-full py-18">
				<Outlet />
				<div className="h-10"></div>
			</div>
			<TanStackRouterDevtools />
		</div>
	);
}
