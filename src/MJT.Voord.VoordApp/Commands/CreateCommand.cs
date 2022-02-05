using System.Diagnostics.CodeAnalysis;
using Spectre.Console;
using Spectre.Console.Cli;

namespace MJT.Voord.VoordApp.Commands;

public sealed class CreateCommand : Command<CreateCommand.Settings>
{
    public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        AnsiConsole.WriteLine("CreateCommand says hello world.");

        AnsiConsole.WriteLine("The poll name is: " + settings.PollName);
        AnsiConsole.WriteLine("The source file path is: " + settings.SrcFilePath);

        return 0;
    }

    public sealed class Settings : CommandSettings
    {
        public Settings(string pollName, string srcFilePath)
        {
            PollName = pollName ?? throw new ArgumentNullException(nameof(pollName));
            SrcFilePath = srcFilePath ?? throw new ArgumentNullException(nameof(srcFilePath));
        }

        [CommandArgument(0, "<PollName>")] public string PollName { get; set; }

        [CommandArgument(1, "<SrcFilePath>")] public string SrcFilePath { get; }
    }
}