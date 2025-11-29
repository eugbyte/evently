using Evently.Server.Domains.Entities;
using System.Security.Claims;

namespace Evently.Server.Domains.Interfaces;

public interface IAccountsService {
	Task<Account> ExternalLogin(ClaimsPrincipal claimsPrincipal, string loginProvider);
	Task<Account?> FindByClaimsPrincipalAsync(ClaimsPrincipal claimsPrincipal);
}