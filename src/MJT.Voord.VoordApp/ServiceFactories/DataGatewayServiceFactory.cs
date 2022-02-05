using System.IO.Abstractions;
using Microsoft.Extensions.Options;
using MJT.Voord.Data.DataGatewayService.Api;
using MJT.Voord.Data.DataGatewayService.Types;
using MJT.Voord.VoordApp.Options;

namespace MJT.Voord.VoordApp.ServiceFactories;

public class DataGatewayServiceFactory : IDataGatewayServiceFactory
{
    private readonly IFileSystem _fileSystem;
    private readonly IOptions<DataOptions> _options;

    public DataGatewayServiceFactory(IOptions<DataOptions> options, IFileSystem fileSystem)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
    }

    public IDataGatewayService CreateInstance()
    {
        string appDataFilePath = Path.Join(_fileSystem.Directory.GetCurrentDirectory(), _options.Value.AppDataFileName);
        string seedDataFilePath = Path.Join(_fileSystem.Directory.GetCurrentDirectory(), _options.Value.SeedDataFileName);

        return new JsonDataGateway(_fileSystem, appDataFilePath, seedDataFilePath);
    }
}