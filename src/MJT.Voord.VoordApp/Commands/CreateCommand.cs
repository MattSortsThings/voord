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

    [SuppressMessage("ReSharper", "RedundantNullableFlowAttribute")]
    public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        RenderSessionHeader(settings.SrcFilePath);

        ValidationResult x = settings.Validate();
        if (!x.Successful)
        {
            AnsiConsole.WriteLine(x.Message!);

            return (int)ExitCodes.InvalidCommandArgsError;
        }

        try
        {
            RunExecutionPath(settings.PollName, settings.SrcFilePath);

            return (int)ExitCodes.Success;
        }
        catch (DataGatewayServiceException e)
        {
            AnsiConsole.WriteException(e);

            return (int)ExitCodes.DataGatewayError;
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
    }

    private void RunExecutionPath(string pollName, string srcFilePath)
    {
        SetupAppData();
        Poll newPoll = LoadNewPoll(srcFilePath);
        RenderPoll(pollName, newPoll);

        if (PromptForCommit()) PersistNewPoll(pollName, newPoll);
    }

    private void PersistNewPoll(string pollName, Poll newPoll)
    {
        _dataGatewayService.Persist(pollName, newPoll);
        AnsiConsole.WriteLine("Changes committed.");
    }

    private static bool PromptForCommit()
    {
        AnsiConsole.WriteLine();
        string shouldCommit = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Commit your new poll?")
                .AddChoices("Yes", "No")
        );

        return shouldCommit == "Yes";
    }

    private static void RenderPoll(string pollName, Poll poll)
    {
        Tree tree = GenerateTree(pollName, poll);

        AnsiConsole.WriteLine();
        AnsiConsole.Write(tree);
        AnsiConsole.WriteLine();
    }

    private static Tree GenerateTree(string pollName, Poll poll)
    {
        string treeTitle = "[bold]Poll Name: [/]" + pollName;
        string candidatesCount = $"{poll.Candidates.Count} candidates:";
        Tree tree = new Tree(treeTitle).Style("bold yellow");
        var node = new TreeNode(new Markup(candidatesCount));
        tree.AddNode(node);
        foreach (Candidate c in poll.Candidates)
        {
            node.AddNode(c.Name);
        }

        return tree;
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

    private static void RenderSessionHeader(string srcFilePath)
    {
        var rule = new Rule("[cornflowerblue]Voord - Developed by Matt Tantony - February 2022[/]")
        {
            Alignment = Justify.Left
        };
        AnsiConsole.Write(rule);

        AnsiConsole.WriteLine();
        var panel = new Panel($"[bold]Creating a Poll[/]\n[bold]Source: [/]{srcFilePath}");
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