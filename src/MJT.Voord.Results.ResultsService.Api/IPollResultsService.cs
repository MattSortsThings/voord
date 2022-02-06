using MJT.Voord.Results.Models;
using MJT.Voord.VotingDomain.Types;

namespace MJT.Voord.Results.ResultsService.Api;

public interface IPollResultsService
{
    IReadOnlyList<Result> ComputeResults(Poll poll);
}