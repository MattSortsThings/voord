using System.IO.Abstractions;
using Microsoft.Extensions.Options;
using MJT.Voord.Loading.LoadingService.Api;
using MJT.Voord.Loading.LoadingService.Types;
using MJT.Voord.VoordApp.Options;

namespace MJT.Voord.VoordApp.ServiceFactories;

public class PollLoadingServiceFactory : IPollLoadingServiceFactory
{
    private readonly IFileSystem _fileSystem;

    public PollLoadingServiceFactory(IFileSystem fileSystem)
    {
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
    }

    public IPollLoadingService CreateInstance()
    {
        return new PollLoader(_fileSystem);
    }
}