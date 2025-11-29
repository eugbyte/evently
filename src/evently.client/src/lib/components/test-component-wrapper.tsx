import type { JSX, ReactNode } from "react";
import { Account } from "~/domains/entities";
import {
	type AnyRoute,
	createMemoryHistory,
	createRootRouteWithContext,
	createRoute,
	createRouter,
	Outlet,
	RouterProvider
} from "@tanstack/react-router";
import type { RouteContext } from "~/domains/interfaces";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";

interface TestComponentProps {
	children: ReactNode;
}

export const wrappedComponentId = "root-component-layout";

/**
 * A HOC to wire up the components with TanStack configurations so that it can be tested.
 *
 * Remember to wait for the wrapped Component to be rendered by calling: `await waitFor(() => screen.findByTestId(WrapperDataTestId))`
 * @param children The React Component to be tested
 * @constructor
 */
export function TestComponentWrapper({ children }: TestComponentProps): JSX.Element {
	const rootRoute = createRootRouteWithContext<RouteContext>()({
		beforeLoad: async () => {
			const account: Account | null = new Account();
			return { account };
		},
		component: () => (
			<div data-testid={wrappedComponentId}>
				<Outlet />
			</div>
		)
	});

	const indexRoute = createRoute({
		getParentRoute: () => rootRoute,
		path: "/",
		component: () => <>{children}</>
	});

	const router = createRouter({
		routeTree: rootRoute.addChildren([indexRoute]),
		defaultPendingMinMs: 0,
		history: createMemoryHistory({ initialEntries: ["/"] }),
		context: {
			account: undefined!
		}
	});
	const queryClient = new QueryClient();

	return (
		<QueryClientProvider client={queryClient}>
			<RouterProvider router={router} />
		</QueryClientProvider>
	);
}

export const wrappedRouteId = "root-route-layout";

interface TestRouteProps {
	route: AnyRoute;
}

/**
 * A HOC to wire up a TanStack route variable so that it can be tested
 * @param route A TanStack route variable of the type `@tanstack/react-router Route`.
 * @constructor
 */
export function TestRouteWrapper({ route }: TestRouteProps): JSX.Element {
	const router = createRouter({
		routeTree: route,
		defaultPendingMinMs: 0,
		history: createMemoryHistory({ initialEntries: ["/"] }),
		context: {
			account: undefined!
		}
	});

	const queryClient = new QueryClient();
	return (
		<QueryClientProvider client={queryClient}>
			<div data-testid={wrappedRouteId}>
				<RouterProvider router={router} />
			</div>
		</QueryClientProvider>
	);
}
