using System.Text.Json.Serialization;

namespace MJT.Voord.VoordApp.Options;

public class DataOptions
{
    [JsonConstructor]
    public DataOptions(string appDataFileName)
    {
        AppDataFileName = appDataFileName;
    }

    public string AppDataFileName { get; init; }
}