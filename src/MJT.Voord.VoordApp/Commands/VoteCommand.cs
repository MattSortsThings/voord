using System.Diagnostics.CodeAnalysis;
using Spectre.Console;
using Spectre.Console.Cli;

namespace MJT.Voord.VoordApp.Commands;

public class VoteCommand : Command<VoteCommand.Settings>
{
    public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        AnsiConsole.WriteLine("CreateCommand says hello world.");

        AnsiConsole.WriteLine("The poll name is: " + settings.PollName);
        AnsiConsole.WriteLine("The juror name is: " + settings.JurorName);

        return 0;
    }

    public sealed class Settings : CommandSettings
    {
        public Settings(string pollName, string jurorName)
        {
            PollName = pollName ?? throw new ArgumentNullException(nameof(pollName));
            JurorName = jurorName ?? throw new ArgumentNullException(nameof(jurorName));
        }

        [CommandArgument(0, "<PollName>")] public string PollName { get; set; }

        [CommandArgument(0, "<JurorName>")] public string JurorName { get; }
    }
}