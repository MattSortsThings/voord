using System.IO.Abstractions;
using System.Text.Json;
using MJT.Voord.Data.DataGatewayService.Api;
using MJT.Voord.VotingDomain.Types;

namespace MJT.Voord.Data.DataGatewayService.Types;

public class JsonDataGateway : IDataGatewayService
{
    private readonly string _appDataFilePath;
    private readonly IFileSystem _fileSystem;
    private readonly string _seedDataFilePath;

    public JsonDataGateway(IFileSystem fileSystem, string appDataFilePath, string seedDataFilePath)
    {
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        _appDataFilePath = appDataFilePath ?? throw new ArgumentNullException(nameof(appDataFilePath));
        _seedDataFilePath = seedDataFilePath ?? throw new ArgumentNullException(nameof(seedDataFilePath));
    }

    public bool AppDataExists => _fileSystem.File.Exists(_appDataFilePath);


    public void WipeAllAppData()
    {
        var persistence = new Persistence(Array.Empty<PollPersistenceModel>());
        string json = JsonSerializer.Serialize(persistence);
        _fileSystem.File.WriteAllText(_appDataFilePath, json);
    }

    public void LoadSeedData()
    {
        try
        {
            IList<PollPersistenceModel> seedData = DeserializeSeedData();
            OverwriteAllAppData(new Persistence(seedData));
        }
        catch (Exception e)
        {
            throw new DataGatewayServiceException("Something went wrong with the data gateway.", e);
        }
    }

    public void Persist(string pollName, Poll poll)
    {
        Persistence data = LoadAllAppData();

        var overwritten = false;
        for (var i = 0; i < data.Polls.Count && !overwritten; i++)
        {
            if (data.Polls[i].PollName != pollName) continue;
            data.Polls[i].Poll = poll;
            overwritten = true;
        }

        if (!overwritten) data.Polls.Add(new PollPersistenceModel(pollName, poll));

        SaveAllAppData(data);
    }

    public Poll LoadPoll(string pollName)
    {
        _ = pollName ?? throw new ArgumentNullException(nameof(pollName));

        try
        {
            return SelectByPollName(pollName);
        }
        catch (Exception e)
        {
            throw new DataGatewayServiceException("Something went wrong with the data gateway.", e);
        }
    }

    private void OverwriteAllAppData(Persistence data)
    {
        _fileSystem.File.WriteAllText(_appDataFilePath, JsonSerializer.Serialize(data));
    }

    private IList<PollPersistenceModel> DeserializeSeedData()
    {
        string text = _fileSystem.File.ReadAllText(_seedDataFilePath);
        var models = JsonSerializer.Deserialize<IList<PollPersistenceModel>>(text);

        return models ?? throw new InvalidOperationException("Json deserialization failed.");
    }

    private Poll SelectByPollName(string pollName)
    {
        PollPersistenceModel? selected = LoadAllAppData().Polls.FirstOrDefault(x => x.PollName == pollName);

        return selected != null
            ? selected.Poll
            : throw new KeyNotFoundException("No poll with that name.");
    }

    private Persistence LoadAllAppData()
    {
        string json = _fileSystem.File.ReadAllText(_appDataFilePath);

        return JsonSerializer.Deserialize<Persistence>(json) ??
               throw new InvalidOperationException("Unable to deserialize JSON.");
    }


    private void SaveAllAppData(Persistence data)
    {
        string json = JsonSerializer.Serialize(data);
        _fileSystem.File.WriteAllText(_appDataFilePath, json);
    }
}