using System.Security.Claims;
using Evently.Server.Domains.Entities;

namespace Evently.Server.Domains.Interfaces;

public interface IAccountsService
{
	Task<Account> ExternalLogin(ClaimsPrincipal claimsPrincipal, string loginProvider);
	Task<Account?> FindByClaimsPrincipalAsync(ClaimsPrincipal claimsPrincipal);
}
