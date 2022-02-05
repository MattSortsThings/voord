using System.IO.Abstractions;
using MJT.Voord.Data.DataGatewayService.Api;

namespace MJT.Voord.Data.DataGatewayService.Types;

public class JsonDataGateway : IDataGatewayService
{
    private readonly IFileSystem _fileSystem;
    private readonly string _appDataFilePath;

    public JsonDataGateway(IFileSystem fileSystem, string appDataFilePath)
    {
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        _appDataFilePath = appDataFilePath ?? throw new ArgumentNullException(nameof(appDataFilePath));
    }
}