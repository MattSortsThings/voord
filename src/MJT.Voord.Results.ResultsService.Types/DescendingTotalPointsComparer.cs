using MJT.Voord.Results.Models;

namespace MJT.Voord.Results.ResultsService.Types;

public class DescendingTotalPointsComparer : IComparer<Result>
{
    public int Compare(Result? x, Result? y)
    {
        if (ReferenceEquals(x, y)) return 0;
        if (ReferenceEquals(null, y)) return 1;
        if (ReferenceEquals(null, x)) return -1;
        int totalPointsComparison = -x.TotalPoints.CompareTo(y.TotalPoints);

        return totalPointsComparison != 0 ? totalPointsComparison : x.AvgRanking.CompareTo(y.AvgRanking);
    }
}