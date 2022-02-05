using Microsoft.Extensions.DependencyInjection;
using MJT.Voord.VoordApp.Commands;
using MJT.Voord.VoordApp.DependencyInjection;
using Spectre.Console.Cli;

namespace MJT.Voord.VoordApp;

public static class SpectreAppBuilder
{
    public static ICommandApp ConfigureApp(IServiceCollection serviceCollection)
    {
        var registrar = new TypeRegistrar(serviceCollection);
        var app = new CommandApp(registrar);

        app.Configure(config =>
        {
            config.AddCommand<CreateCommand>("create")
                .WithDescription("Creates a new poll.")
                .WithExample(new[] { "create", "ExamplePoll", "C:\\Users\\example\\example_poll_data.csv" });

            config.AddCommand<VoteCommand>("vote")
                .WithDescription("Registers a single juror's votes in a single poll.")
                .WithExample(new[] { "vote", "ExamplePoll", "Matt" });

            config.AddCommand<ViewCommand>("view")
                .WithDescription("Displays the results of a single poll.")
                .WithExample(new[] { "view", "ExamplePoll" });
        });

        return app;
    }
}