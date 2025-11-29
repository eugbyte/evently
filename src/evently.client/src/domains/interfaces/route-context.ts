import { Account } from "~/domains/entities";

export interface RouteContext {
	// The ReturnType of your useAuth hook or the value of your AuthContext
	account: Account;
}
