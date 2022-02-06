using System.Collections.Immutable;
using MJT.Voord.Results.Models;
using MJT.Voord.Results.ResultsService.Api;
using MJT.Voord.VotingDomain.Types;

namespace MJT.Voord.Results.ResultsService.Types;

public class LogarithmicPollCalculator : IPollResultsService
{
    private static readonly IComparer<Result> _comparer = new DescendingTotalPointsComparer();
    private readonly int _rankOneScore;

    public LogarithmicPollCalculator(int rankOneScore)
    {
        _rankOneScore = rankOneScore;
    }

    public IReadOnlyList<Result> ComputeResults(Poll poll)
    {
        _ = poll ?? throw new ArgumentNullException(nameof(poll));

        try
        {
            return GenerateResultsInDescendingTotalPointsOrder(poll);
        }
        catch (Exception e)
        {
            throw new PollResultsServiceException("Something went wrong while calculating the results.", e);
        }
    }


    private IReadOnlyList<Result> GenerateResultsInDescendingTotalPointsOrder(Poll p)
    {
        Func<int, int> logFunction = GenerateLogFunction(p);

        List<Result> results = p.Candidates.Select(c => ComputeIndividualResult(c, logFunction)).ToList();

        results.Sort(_comparer);
        return results;
    }

    private Result ComputeIndividualResult(Candidate c, Func<int, int> f)
    {
        int totalPoints = ComputeTotalPoints(c, f);
        double avg = ComputeAvgRanking(c);

        return new Result(c.Name, totalPoints, avg);
    }

    private static int ComputeTotalPoints(Candidate c, Func<int, int> f)
    {
        return c.Votes.Sum(v => f(v.Ranking));
    }

    private static double ComputeAvgRanking(Candidate c)
    {
        return c.Votes.Count == 0 ? 0 : c.Votes.Select(v => v.Ranking).Average();
    }

    private Func<int, int> GenerateLogFunction(Poll p)
    {
        int n = p.Candidates.Count;

        return ranking => ranking == 1 ? _rankOneScore :
            ranking == n ? 0 : (int)Math.Ceiling(_rankOneScore - Math.Log(ranking, n) * _rankOneScore);
    }
}