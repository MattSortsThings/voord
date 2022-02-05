using System.Diagnostics.CodeAnalysis;
using MJT.Voord.Data.DataGatewayService.Api;
using Spectre.Console;
using Spectre.Console.Cli;

namespace MJT.Voord.VoordApp.Commands;

public class ViewCommand : Command<ViewCommand.Settings>
{
    private readonly IDataGatewayService _dataGatewayService;

    public ViewCommand(IDataGatewayService dataGatewayService)
    {
        _dataGatewayService = dataGatewayService ?? throw new ArgumentNullException(nameof(dataGatewayService));
    }

    public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        RenderSessionHeader(settings.PollName);
        
        ValidationResult x = settings.Validate();
        if (!x.Successful)
        {
            AnsiConsole.WriteLine(x.Message!);

            return (int)ExitCodes.InvalidCommandArgsError;
        }

        return 0;
    }

    private static void RenderSessionHeader(string pollName)
    {
        var rule = new Rule("[cornflowerblue]Voord - Developed by Matt Tantony - February 2022[/]")
        {
            Alignment = Justify.Left
        };
        AnsiConsole.Write(rule);

        AnsiConsole.WriteLine();
        var panel = new Panel($"[bold]Poll Name: [/]{pollName}");
        AnsiConsole.Write(panel);
        AnsiConsole.WriteLine();
    }

    public sealed class Settings : BaseSettings
    {
        public Settings(string pollName) : base(pollName)
        {
        }
    }
}