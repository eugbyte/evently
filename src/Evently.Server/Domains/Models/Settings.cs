using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Evently.Server.Domains.Models;

public sealed class Settings {
	public StorageAccount StorageAccount { get; init; } = new();

	[NotMapped] public AuthSetting Authentication { get; init; } = new();

	[NotMapped] public EmailSettings EmailSettings { get; init; } = new();
	[NotMapped] public AzureAIFoundry AzureAiFoundry { get; init; } = new();
}

[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
public sealed class StorageAccount {
	public string AccountName { get; init; } = string.Empty;
	public string AzureStorageConnectionString { get; init; } = string.Empty;
}

[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
public sealed class AuthSetting {
	public OAuthSetting Google { get; init; } = new();
}

[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
public sealed class OAuthSetting {
	public string ClientId { get; init; } = string.Empty;
	public string ClientSecret { get; init; } = string.Empty;
}

[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
public sealed class EmailSettings {
	public string ActualFrom { get; init; } = string.Empty;
	public string SmtpPassword { get; init; } = string.Empty;
}

[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
// ReSharper disable once InconsistentNaming
public sealed class AzureAIFoundry {
	public string ContentSafetyKey { get; init; } = string.Empty;
	public string ContentSafetyEndpoint { get; init; } = string.Empty;
}