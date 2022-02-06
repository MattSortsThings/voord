using System.Diagnostics.CodeAnalysis;
using MJT.Voord.Data.DataGatewayService.Api;
using MJT.Voord.Results.Models;
using MJT.Voord.Results.ResultsService.Api;
using MJT.Voord.VotingDomain.Types;
using Spectre.Console;
using Spectre.Console.Cli;

namespace MJT.Voord.VoordApp.Commands;

public class ViewCommand : Command<ViewCommand.Settings>
{
    private readonly IDataGatewayService _dataGatewayService;
    private readonly IPollResultsService _pollResultsService;

    public ViewCommand(IDataGatewayService dataGatewayService, IPollResultsService pollResultsService)
    {
        _dataGatewayService = dataGatewayService ?? throw new ArgumentNullException(nameof(dataGatewayService));
        _pollResultsService = pollResultsService ?? throw new ArgumentNullException(nameof(pollResultsService));
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

        try
        {
            RunExecutionPath(settings.PollName);


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

    private void RunExecutionPath(string pollName)
    {
        SetupAppData();
        Poll activePoll = LoadPoll(pollName);
        IReadOnlyList<Result> results = ComputeResults(activePoll);
        foreach (var r in results)
        {
            AnsiConsole.Write("1");
        }
    }

    private Poll LoadPoll(string pollName)
    {
        return _dataGatewayService.LoadPoll(pollName);
    }

    private IReadOnlyList<Result> ComputeResults(Poll poll)
    {
        return _pollResultsService.ComputeResults(poll);
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

    private void SetupAppData()
    {
        if (_dataGatewayService.AppDataExists) return;
        _dataGatewayService.WipeAllAppData();
        _dataGatewayService.LoadSeedData();
    }

    public sealed class Settings : BaseSettings
    {
        public Settings(string pollName) : base(pollName)
        {
        }
    }
}