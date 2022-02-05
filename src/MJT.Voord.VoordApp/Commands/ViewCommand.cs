using System.Diagnostics.CodeAnalysis;
using Spectre.Console;
using Spectre.Console.Cli;

namespace MJT.Voord.VoordApp.Commands;

public class ViewCommand : Command<ViewCommand.Settings>
{
    public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        AnsiConsole.WriteLine("CreateCommand says hello world.");

        AnsiConsole.WriteLine("The poll name is: " + settings.PollName);

        return 0;
    }

    public sealed class Settings : CommandSettings
    {
        public Settings(string pollName)
        {
            PollName = pollName ?? throw new ArgumentNullException(nameof(pollName));
        }

        [CommandArgument(0, "<PollName>")] public string PollName { get; }
    }
}