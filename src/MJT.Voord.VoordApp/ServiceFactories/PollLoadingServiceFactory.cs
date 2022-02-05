using System.IO.Abstractions;
using MJT.Voord.Loading.LoadingService.Api;
using MJT.Voord.Loading.LoadingService.Types;

namespace MJT.Voord.VoordApp.ServiceFactories;

public class PollLoadingServiceFactory : IPollLoadingServiceFactory
{
    public IPollLoadingService CreateInstance()
    {
        IFileSystem fileSystem = new FileSystem();

        return new CsvPollLoader(fileSystem);
    }
}