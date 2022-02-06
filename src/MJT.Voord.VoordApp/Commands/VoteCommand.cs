using System.Diagnostics.CodeAnalysis;
using MJT.Voord.Data.DataGatewayService.Api;
using MJT.Voord.VotingDomain.Types;
using Spectre.Console;
using Spectre.Console.Cli;

namespace MJT.Voord.VoordApp.Commands;

public class VoteCommand : Command<VoteCommand.Settings>
{
    private readonly IDataGatewayService _dataGatewayService;

    public VoteCommand(IDataGatewayService dataGatewayService)
    {
        _dataGatewayService = dataGatewayService;
    }

    public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        RenderSessionHeader(settings.PollName, settings.JurorName);

        ValidationResult x = settings.Validate();
        if (!x.Successful)
        {
            AnsiConsole.WriteLine(x.Message!);

            return (int)ExitCodes.InvalidCommandArgsError;
        }

        try
        {
            RunExecutionPath(settings.PollName, settings.JurorName);

            return (int)ExitCodes.Success;
        }
        catch (DataGatewayServiceException e)
        {
            AnsiConsole.WriteException(e);

            return (int)ExitCodes.DataGatewayError;
        }
        catch (Exception e)
        {
            AnsiConsole.WriteException(e);

            return (int)ExitCodes.OtherError;
        }
    }

    private void RunExecutionPath(string pollName, string jurorName)
    {
        SetupAppData();
        Poll activePoll = LoadPoll(pollName);
    }

    private void SetupAppData()
    {
        if (_dataGatewayService.AppDataExists) return;
        _dataGatewayService.WipeAllAppData();
        _dataGatewayService.LoadSeedData();
    }

    private Poll LoadPoll(string pollName)
    {
        return _dataGatewayService.LoadPoll(pollName);
    }

    private static void RenderSessionHeader(string pollName, string jurorName)
    {
        var rule = new Rule("[cornflowerblue]Voord - Developed by Matt Tantony - February 2022[/]")
        {
            Alignment = Justify.Left
        };
        AnsiConsole.Write(rule);

        AnsiConsole.WriteLine();
        var panel = new Panel($"[bold]Poll Name: [/]{pollName}\n[bold]Juror Name: [/]{jurorName}");
        AnsiConsole.Write(panel);
        AnsiConsole.WriteLine();
    }

    public sealed class Settings : BaseSettings
    {
        public Settings(string pollName, string jurorName) : base(pollName)
        {
            JurorName = jurorName ?? throw new ArgumentNullException(nameof(jurorName));
        }

        [CommandArgument(1, "<JurorName>")] public string JurorName { get; }
    }
}