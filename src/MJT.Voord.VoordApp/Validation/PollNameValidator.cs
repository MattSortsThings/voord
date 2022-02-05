namespace MJT.Voord.VoordApp.Validation;

public static class PollNameValidator
{
    public static string ValidationMessage => "Poll name must be between 1 and 20 chars in length. " +
                                              "It must start with a letter. " +
                                              "It may contain only letters, digits, underscores and/or hyphens.";
    
    public static bool IsValid(string pollName)
    {
        return HasValidLength(pollName) && StartsWithLetter(pollName) && ContainsOnlyValidChars(pollName);
    }

    private static bool HasValidLength(string s)
    {
        return s.Length is > 0 and <= 20;
    }

    private static bool StartsWithLetter(string s)
    {
        return char.IsLetter(s[0]);
    }

    private static bool ContainsOnlyValidChars(string s)
    {
        return s.All(c => char.IsLetterOrDigit(c) || c is '_' or '-');
    }
}