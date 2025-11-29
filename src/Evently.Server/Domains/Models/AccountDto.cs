using JetBrains.Annotations;

namespace Evently.Server.Domains.Models;

[UsedImplicitly]
public sealed record AccountDto(string Id, string Email, string Username, string Name);