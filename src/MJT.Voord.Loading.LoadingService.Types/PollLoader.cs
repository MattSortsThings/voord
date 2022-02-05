using System.Globalization;
using System.IO.Abstractions;
using System.Text.Json;
using CsvHelper;
using MJT.Voord.Loading.LoadingService.Api;
using MJT.Voord.VotingDomain.Types;

namespace MJT.Voord.Loading.LoadingService.Types;

public class PollLoader : IPollLoadingService
{
    private const string LoadingFromCsvFailMessage = "Something went wrong while trying to load a poll from CSV.";
    private readonly List<Candidate> _candidates;
    private readonly IFileSystem _fileSystem;
    private int _counter;

    public PollLoader(IFileSystem fileSystem)
    {
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        _candidates = new List<Candidate>(20);
    }

    public Poll LoadNewPollFromCsv(string srcFilePath)
    {
        _ = srcFilePath ?? throw new ArgumentNullException(nameof(srcFilePath));

        try
        {
            return CreateNewPollFromCsvFileAt(srcFilePath);
        }
        catch (Exception e)
        {
            throw new PollLoadingServiceException(LoadingFromCsvFailMessage, e);
        }
    }


    private Poll CreateNewPollFromCsvFileAt(string path)
    {
        using StreamReader reader = _fileSystem.File.OpenText(path);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        foreach (PollItemModel record in csv.GetRecords<PollItemModel>())
        {
            _counter++;
            _candidates.Add(new Candidate(_counter, record.Name));
        }

        var poll = new Poll(_candidates.ToArray());
        Reset();

        return poll;
    }

    private void Reset()
    {
        _counter = 0;
        _candidates.Clear();
    }
}