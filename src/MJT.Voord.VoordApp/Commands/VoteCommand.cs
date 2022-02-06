using System.Diagnostics.CodeAnalysis;
using MJT.Voord.Data.DataGatewayService.Api;
using MJT.Voord.Voting.VotingService.Api;
using MJT.Voord.VotingDomain.Types;
using Spectre.Console;
using Spectre.Console.Cli;

namespace MJT.Voord.VoordApp.Commands;

public class VoteCommand : Command<VoteCommand.Settings>
{
    private readonly IDataGatewayService _dataGatewayService;
    private readonly IPollVotingService _pollVotingService;

    public VoteCommand(IDataGatewayService dataGatewayService, IPollVotingService pollVotingService)
    {
        _dataGatewayService = dataGatewayService;
        _pollVotingService = pollVotingService;
    }

    [SuppressMessage("ReSharper", "RedundantNullableFlowAttribute")]
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
        AnsiConsole.MarkupLine("[bold]Voting (from worst candidate to best!)\n\n[/]");

        Poll activePoll = LoadPoll(pollName);
        RegisterWithVotingService(jurorName, activePoll);

        while (!_pollVotingService.Finished)
        {
            AssignVote();
        }

        AnsiConsole.Write("Finished!\n\n");

        bool shouldCommit = PromptForCommit();
        if (shouldCommit) PersistChanges(pollName, activePoll);
    }

    private void PersistChanges(string pollName, Poll activePoll)
    {
        _dataGatewayService.Persist(pollName, activePoll);
        AnsiConsole.WriteLine("Changes committed.");
    }

    private static bool PromptForCommit()
    {
        AnsiConsole.WriteLine();
        string shouldCommit = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Commit your votes?")
                .AddChoices("Yes", "No")
        );

        return shouldCommit == "Yes";
    }


    private void AssignVote()
    {
        int currentRanking = _pollVotingService.CurrentRanking;
        string promptTitle = $"Select the option you assign rank {currentRanking}.";

        string selectedCandidate = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(promptTitle)
                .AddChoices(_pollVotingService.RemainingCandidateNames)
        );

        AnsiConsole.WriteLine($"Assigned rank {_pollVotingService.CurrentRanking} to option {selectedCandidate}.");

        _pollVotingService.AssignCurrentRankingToCandidate(selectedCandidate);
    }

    private void RegisterWithVotingService(string jurorName, Poll activePoll)
    {
        _pollVotingService.ActivePoll = activePoll;
        _pollVotingService.JurorName = jurorName;
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
        var panel = new Panel($"[bold]Voting in Poll[/]\n[bold]Poll Name: [/]{pollName}\n[bold]Juror Name: [/]{jurorName}");
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