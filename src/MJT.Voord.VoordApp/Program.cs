using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MJT.Voord.VoordApp;
using Spectre.Console.Cli;

IConfigurationRoot configurationRoot = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", false, false)
    .Build();

IServiceCollection services = ServicesConfigurator.ConfigureServices(configurationRoot);
ICommandApp app = SpectreAppBuilder.ConfigureApp(services);

return app.Run(args);