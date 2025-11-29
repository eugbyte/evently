using System.Security.Claims;
using Evently.Server.Domains.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Evently.Server.Features.Accounts.Services;

// Based on https://tinyurl.com/5cxw9vmu
public sealed class AccountAuthorizationHandler(UserManager<Account> userManager)
	: AuthorizationHandler<SameAccountRequirement, string>
{
	protected override async Task HandleRequirementAsync(
		AuthorizationHandlerContext context,
		SameAccountRequirement requirement,
		string? identityUserId
	)
	{
		ClaimsPrincipal principal = context.User;
		Account? user = await FindByClaimsPrincipalAsync(userManager, principal);

		bool userMatch = user is not null && user.Id == identityUserId;
		if (userMatch)
		{
			context.Succeed(requirement);
		}
	}

	private static async Task<Account?> FindByClaimsPrincipalAsync(
		UserManager<Account> userManager,
		ClaimsPrincipal claimsPrincipal
	)
	{
		string loginProvider = claimsPrincipal.Identity?.AuthenticationType ?? string.Empty;
		string providerKey =
			claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

		Account? user = await userManager.GetUserAsync(claimsPrincipal);
		return user ?? await userManager.FindByLoginAsync(loginProvider, providerKey);
	}
}

public class SameAccountRequirement : IAuthorizationRequirement
{
	public const string PolicyName = "SameAccountPolicy";
}
