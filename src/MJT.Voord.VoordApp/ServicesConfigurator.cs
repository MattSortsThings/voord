using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MJT.Voord.Loading.LoadingService.Api;
using MJT.Voord.VoordApp.ServiceFactories;

namespace MJT.Voord.VoordApp;

public static class ServicesConfigurator
{
    public static IServiceCollection ConfigureServices(IConfigurationRoot configurationRoot)
    {
        var services = new ServiceCollection();

        services.AddSingleton<IPollLoadingServiceFactory, PollLoadingServiceFactory>();
        services.AddScoped(serviceProvider => serviceProvider.GetRequiredService<IPollLoadingServiceFactory>().CreateInstance());

        return services;
    }
}