using System.Diagnostics.CodeAnalysis;
using System.Text;
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
        RenderResultsTable(results);
    }

    private Poll LoadPoll(string pollName)
    {
        return _dataGatewayService.LoadPoll(pollName);
    }

    private IReadOnlyList<Result> ComputeResults(Poll poll)
    {
        return _pollResultsService.ComputeResults(poll);
    }

    private static void RenderResultsTable(IReadOnlyList<Result> results)
    {
        var table = new Table();

        table.AddColumn("[bold]Total Points[/]");
        table.AddColumn("[bold]Avg Ranking[/]");
        table.AddRow(CreateBarChart(results), CreateAvgRankings(results));
        table.Border(TableBorder.Rounded);

        table.Columns[1].RightAligned();

        AnsiConsole.Write(table);
    }

    private static BarChart CreateBarChart(IReadOnlyList<Result> results)
    {
        BarChart barChart = new BarChart().Width(50);

        (string n1, int tp1, _) = results[0];
        barChart.AddItem(n1, tp1, Color.Gold1);

        var counter = 1;

        if (results.Count > 3)
        {
            (string n2, int tp2, _) = results[1];
            barChart.AddItem(n2, tp2, Color.LightSlateBlue);

            (string n3, int tp3, _) = results[2];
            barChart.AddItem(n3, tp3, Color.RosyBrown);

            counter = 3;
        }

        for (; counter < results.Count - 1; counter++)
        {
            barChart.AddItem(results[counter].Name, results[counter].TotalPoints, Color.White);
        }

        barChart.AddItem(results[counter].Name, results[counter].TotalPoints, Color.Red);

        return barChart;
    }

    private static Markup CreateAvgRankings(IEnumerable<Result> results)
    {
        var sb = new StringBuilder();
        foreach (Result r in results)
        {
            sb.Append($"{r.AvgRanking:0.00}\n");
        }

        return new Markup(sb.ToString());
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