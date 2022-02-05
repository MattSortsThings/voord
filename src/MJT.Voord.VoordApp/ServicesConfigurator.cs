﻿using System.IO.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MJT.Voord.Data.DataGatewayService.Api;
using MJT.Voord.Loading.LoadingService.Api;
using MJT.Voord.VoordApp.Options;
using MJT.Voord.VoordApp.ServiceFactories;

namespace MJT.Voord.VoordApp;

public static class ServicesConfigurator
{
    public static IServiceCollection ConfigureServices(IConfigurationRoot configurationRoot)
    {
        var services = new ServiceCollection();

        services.Configure<DataOptions>(options => configurationRoot.GetSection("Data").Bind(options));

        services.AddSingleton<IFileSystem, FileSystem>();

        services.AddSingleton<IPollLoadingServiceFactory, PollLoadingServiceFactory>();
        services.AddScoped(serviceProvider => serviceProvider.GetRequiredService<IPollLoadingServiceFactory>().CreateInstance());

        services.AddSingleton<IDataGatewayServiceFactory, DataGatewayServiceFactory>();
        services.AddScoped(serviceProvider => serviceProvider.GetRequiredService<IDataGatewayServiceFactory>().CreateInstance());

        return services;
    }
}