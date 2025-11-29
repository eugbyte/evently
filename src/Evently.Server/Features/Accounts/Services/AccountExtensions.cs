using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Evently.Server.Features.Accounts.Services;

public static class AccountExtensions
{
	public static async Task<bool> IsResourceOwner(
		this ControllerBase controller,
		object? resourceIdentityUserId
	)
	{
		IAuthorizationService authorizationService =
			controller.HttpContext.RequestServices.GetRequiredService<IAuthorizationService>();

		AuthenticateResult authenticationResult = await controller.HttpContext.AuthenticateAsync(
			IdentityConstants.ExternalScheme
		);
		if (!authenticationResult.Succeeded || resourceIdentityUserId is null)
		{
			return false;
		}

		ClaimsPrincipal principal = authenticationResult.Principal ?? new ClaimsPrincipal();
		AuthorizationResult authorizationResult = await authorizationService.AuthorizeAsync(
			principal,
			resourceIdentityUserId,
			SameAccountRequirement.PolicyName
		);
		return authorizationResult.Succeeded;
	}
}
