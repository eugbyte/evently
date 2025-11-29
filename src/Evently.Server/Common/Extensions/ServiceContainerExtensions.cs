using Evently.Server.Domains.Models;
using Microsoft.Extensions.Options;

namespace Evently.Server.Common.Extensions;

public static class ServiceContainerExtensions
{
    public static IOptions<Settings> LoadAppConfiguration(
        this IServiceCollection services,
        ConfigurationManager configuration
    )
    {
        // load .env variables, in addition to appsettings.json that is loaded by default
        configuration.AddEnvironmentVariables();

        // Inject IOptions<Settings> into the App
        services.Configure<Settings>(configuration);

        // Bind all key value pairs to the Settings Object and return it, as it is used in Program.cs
        Settings settings = new();
        configuration.Bind(settings);

        IOptions<Settings> options = Options.Create(settings);
        return options;
    }
}
