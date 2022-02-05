using System.Diagnostics.CodeAnalysis;
using MJT.Voord.Data.DataGatewayService.Api;
using MJT.Voord.Loading.LoadingService.Api;
using MJT.Voord.VotingDomain.Types;
using Spectre.Console;
using Spectre.Console.Cli;

namespace MJT.Voord.VoordApp.Commands;

public sealed class CreateCommand : Command<CreateCommand.Settings>
{
    private readonly IDataGatewayService _dataGatewayService;
    private readonly IPollLoadingService _pollLoadingService;

    public CreateCommand(IDataGatewayService dataGatewayService, IPollLoadingService pollLoadingService)
    {
        _dataGatewayService = dataGatewayService ?? throw new ArgumentNullException(nameof(dataGatewayService));
        _pollLoadingService = pollLoadingService ?? throw new ArgumentNullException(nameof(pollLoadingService));
    }

    public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        RenderSessionHeader(settings.PollName, settings.SrcFilePath);
        
        ValidationResult x = settings.Validate();
        if (!x.Successful)
        {
            AnsiConsole.WriteLine(x.Message!);

            return (int)ExitCodes.InvalidCommandArgsError;
        }

        try
        {
            RunExecutionPath(settings.SrcFilePath);
        }
        catch (PollLoadingServiceException e)
        {
            AnsiConsole.WriteException(e);

            return (int)ExitCodes.LoadingError;
        }
        catch (Exception e)
        {
            AnsiConsole.WriteException(e);

            return (int)ExitCodes.OtherError;
        }

        return (int)ExitCodes.Success;
    }

    private void RunExecutionPath(string srcFilePath)
    {
        SetupAppData();
        Poll newPoll = LoadNewPoll(srcFilePath);

        foreach (Candidate candidate in newPoll.Candidates)
        {
            AnsiConsole.WriteLine(candidate.Id + " " + candidate.Name);
        }
    }

    private void SetupAppData()
    {
        if (_dataGatewayService.AppDataExists) return;
        _dataGatewayService.WipeAllAppData();
        _dataGatewayService.LoadSeedData();
    }

    private Poll LoadNewPoll(string srcFilePath)
    {
        return _pollLoadingService.LoadNewPollFromCsv(srcFilePath);
    }

    private static void RenderSessionHeader(string pollName, string srcFilePath)
    {
        var rule = new Rule("[cornflowerblue]Voord - Developed by Matt Tantony - February 2022[/]")
        {
            Alignment = Justify.Left
        };
        AnsiConsole.Write(rule);

        AnsiConsole.WriteLine();
        var panel = new Panel($"[bold]Poll Name: [/]{pollName}\n[bold]Source: [/]{srcFilePath}");
        AnsiConsole.Write(panel);
        AnsiConsole.WriteLine();
    }

    public sealed class Settings : BaseSettings
    {
        public Settings(string pollName, string srcFilePath) : base(pollName)
        {
            SrcFilePath = srcFilePath ?? throw new ArgumentNullException(nameof(srcFilePath));
        }

        [CommandArgument(1, "<SrcFilePath>")] public string SrcFilePath { get; }
    }
}