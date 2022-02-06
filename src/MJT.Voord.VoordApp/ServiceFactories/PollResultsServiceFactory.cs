using Microsoft.Extensions.Options;
using MJT.Voord.Results.ResultsService.Api;
using MJT.Voord.Results.ResultsService.Types;
using MJT.Voord.VoordApp.Options;

namespace MJT.Voord.VoordApp.ServiceFactories;

public class PollResultsServiceFactory : IPollResultsServiceFactory
{
    private readonly IOptions<ResultsOptions> _options;

    public PollResultsServiceFactory(IOptions<ResultsOptions> options)
    {
        _options = options;
    }

    public IPollResultsService CreateInstance()
    {
        return new LogarithmicPollCalculator(_options.Value.RankOneScore);
    }
}