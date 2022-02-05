using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MJT.Voord.VoordApp;

public static class ServicesConfigurator
{
    public static IServiceCollection ConfigureServices(IConfigurationRoot configurationRoot)
    {
        var services = new ServiceCollection();

        return services;
    }
}