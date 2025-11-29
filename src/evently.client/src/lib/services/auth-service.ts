import { redirect } from "@tanstack/react-router";
import axios, { type AxiosResponse } from "axios";
import { Account } from "~/domains/entities";
import { sleep } from "~/lib/services/util-service";

export async function login(redirectUrl: string) {
	// must redirect instead of making a REST request (https://stackoverflow.com/a/48925986/6514532)
	// cannot use navigate();
	window.location.href = `/api/v1/Auth/external/google/login?originUrl=${redirectUrl}`;
}

export async function logout(redirectUrl: string): Promise<void> {
	await axios.post("/api/v1/Auth/external/logout", {}, { params: { redirectUrl } });

	window.location.href = redirectUrl;
}

export async function getAccount(): Promise<Account | null> {
	try {
		const result: void | AxiosResponse<Account> = await Promise.race([
			axios.get<Account>("/api/v1/Auth/external/account"),
			sleep(1500)
		]);
		if (result == null) {
			throw new Error("timeout fetching account - might be due to cold start.");
		}
		return result.data;
	} catch (e) {
		const error = e as Error;
		console.warn(error.message);
	}
	return null;
}

/**
 * Validate against the user's `Account` in the route context.
 * Redirect to login page if authentication fails.
 * @param account
 * @param currentHref
 */
export async function authenticateRoute(
	account: Account | null,
	currentHref: string
): Promise<void> {
	if (account == null) {
		throw redirect({
			to: "/login",
			replace: true,
			search: {
				redirect: currentHref
			}
		});
	}
}
