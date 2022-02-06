using System.Diagnostics.CodeAnalysis;
using MJT.Voord.Data.DataGatewayService.Api;
using Spectre.Console;
using Spectre.Console.Cli;

namespace MJT.Voord.VoordApp.Commands;

public sealed class NamesCommand : Command<NamesCommand.Settings>
{
    private readonly IDataGatewayService _dataGatewayService;

    public NamesCommand(IDataGatewayService dataGatewayService)
    {
        _dataGatewayService = dataGatewayService ?? throw new ArgumentNullException(nameof(dataGatewayService));
    }

    [SuppressMessage("ReSharper", "RedundantNullableFlowAttribute")]
    public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        RenderSessionHeader();

        try
        {
            RunExecutionPath();

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

    private void RunExecutionPath()
    {
        SetupAppData();
        AnsiConsole.WriteLine();
        IEnumerable<string> pollNames = LoadAllPollNames();
        AnsiConsole.Write(CreatePollNamesTree(pollNames));
        AnsiConsole.WriteLine();
    }

    private void SetupAppData()
    {
        if (_dataGatewayService.AppDataExists) return;
        _dataGatewayService.WipeAllAppData();
        _dataGatewayService.LoadSeedData();
    }

    private IReadOnlyList<string> LoadAllPollNames()
    {
        return _dataGatewayService.LoadAllPollNames();
    }

    private Tree CreatePollNamesTree(IEnumerable<string> pollNames)
    {
        const string treeTitle = "[bold]Poll Names: [/]";
        Tree tree = new Tree(treeTitle).Style("bold yellow");
        foreach (string name in pollNames)
        {
            tree.AddNode(name);
        }

        return tree;
    }

    private static void RenderSessionHeader()
    {
        var rule = new Rule("[cornflowerblue]Voord - Developed by Matt Tantony - February 2022[/]")
        {
            Alignment = Justify.Left
        };
        AnsiConsole.Write(rule);
        AnsiConsole.WriteLine();
    }

    public class Settings : CommandSettings
    {
    }
}