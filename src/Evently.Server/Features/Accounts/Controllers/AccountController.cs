using Evently.Server.Common.Extensions;
using Evently.Server.Domains.Entities;
using Evently.Server.Domains.Exceptions;
using Evently.Server.Domains.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Evently.Server.Features.Accounts.Controllers;

// Based on https://tinyurl.com/26arz8vk
[ApiController]
[Route("api/v1/Auth/external")]
public sealed class AccountController(
	IAccountsService accountService,
	ILogger<AccountController> logger) : ControllerBase {
	private readonly Dictionary<string, string> _authSchemes = new() {
		{ "google", GoogleDefaults.AuthenticationScheme },
		{ "microsoft", MicrosoftAccountDefaults.AuthenticationScheme },
	};

	/**
	 * Overall auth flow:
	 * 1. Login from FE browser. FE specified callback URL.
	 * 2. Lands on "{provider}/login".
	 * 3. One callback url is appended to the challenge URL in the form of {provider}/callback by the Login method.
	 * 4. Another callback url is appended to the challenge URL in the form of /signin-google/ by the OAuth middleware in Program.cs.
	 * 5. User is redirected to the Google Login page.
	 * 6. On success, redirected to /signin-google/ specified by middleware.
	 * 7. Middleware redirects to {provider}/callback.
	 * 8. Callback() method redirects to FE specified callback URL.
	 */
	[HttpGet("{provider}/login")]
	public IActionResult Login(string provider, string? originUrl = "") {
		Uri rootUri = Request.RootUri();
		string uri = Url.Action("Callback", "Account", values: new { provider }) ?? "";
		UriBuilder combined = new(rootUri) {
			Path = uri,
			Query = $"originUrl={originUrl}",
		};

		logger.LogCallbackUrl(combined.Uri.AbsoluteUri);
		AuthenticationProperties properties = new() {
			RedirectUri = combined.Uri.AbsoluteUri,
			IsPersistent = true,
		};
		return Challenge(properties, _authSchemes.GetValueOrDefault(provider) ?? "");
	}

	[HttpGet("{provider}/callback")]
	public async Task<ActionResult<int>> Callback(string provider, [FromQuery] string originUrl = "") {
		AuthenticateResult result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

		if (!result.Succeeded || result.Principal is null) {
			return Unauthorized();
		}

		ClaimsPrincipal claimsPrincipal = result.Principal;
		if (claimsPrincipal == null) {
			throw new ExternalLoginProviderException(provider, "ClaimsPrincipal is null");
		}

		await accountService.ExternalLogin(claimsPrincipal,
			loginProvider: _authSchemes.GetValueOrDefault(provider) ?? "");
		return Redirect(originUrl);
	}

	[HttpGet("account", Name = "GetAccount from Cookie")]
	public async Task<ActionResult> GetAccount() {
		AuthenticateResult result = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);
		if (!result.Succeeded) {
			return Unauthorized();
		}

		ClaimsPrincipal principal = result.Principal ?? new ClaimsPrincipal();
		Account? user = await accountService.FindByClaimsPrincipalAsync(principal);
		if (user is null) {
			return NotFound(new { message = "User not found" });
		}

		return Ok(user.ToAccountDto());
	}

	[HttpPost("logout")]
	public async Task<ActionResult> Logout(string? redirectUrl = "") {
		// Sign out of an external identity provider (if used)
		AuthenticationProperties authProps = new() {
			RedirectUri = redirectUrl,
			IsPersistent = true,
		};
		await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme, authProps);
		// Manually remove the authentication cookie
		// Delete each cookie
		foreach (string cookieName in Request.Cookies.Keys) {
			Response.Cookies.Delete(cookieName);
		}

		return Ok(new { redirectUrl });
	}
}