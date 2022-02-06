using MJT.Voord.VoordApp.Validation;
using Spectre.Console;
using Spectre.Console.Cli;

namespace MJT.Voord.VoordApp.Commands;

public abstract class BaseSettings : CommandSettings
{
    protected BaseSettings(string pollName)
    {
        PollName = pollName;
    }

    [CommandArgument(0, "<PollName>")] public string PollName { get; }

    public override ValidationResult Validate()
    {
        return PollNameValidator.IsValid(PollName)
            ? ValidationResult.Success()
            : ValidationResult.Error(PollNameValidator.ValidationMessage);
    }
}