using System.IO.Abstractions;
using MJT.Voord.Data.Bootstrapping.Api;
using MJT.Voord.VotingDomain.Types;

namespace MJT.Voord.Data.Bootstrapping.Types;

public class JsonDataBootstrapper : IDataBootstrapper
{
    private readonly IFileSystem _fileSystem;
    private readonly string _seedDataFilePath;


    public JsonDataBootstrapper(IFileSystem fileSystem, string seedDataFilePath)
    {
        _fileSystem = fileSystem;
        _seedDataFilePath = seedDataFilePath;
    }

    public IEnumerable<Poll> GetSeedData()
    {
        throw new NotImplementedException();
    }
}