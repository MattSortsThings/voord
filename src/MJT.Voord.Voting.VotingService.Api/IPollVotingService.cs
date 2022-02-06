using MJT.Voord.VotingDomain.Types;

namespace MJT.Voord.Voting.VotingService.Api;

public interface IPollVotingService
{
    IReadOnlyList<string> RemainingCandidateNames { get; }

    Poll ActivePoll { get; set; }

    int CurrentRanking { get; }

    string JurorName { get; set; }

    bool Finished { get; }

    void AssignCurrentRankingToCandidate(string candidateName);
}