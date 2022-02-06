using MJT.Voord.Voting.VotingService.Api;
using MJT.Voord.VotingDomain.Types;

namespace MJT.Voord.Voting.VotingService.Types;

public class VoteRecorder : IPollVotingService
{
    private readonly List<string> _remainingCandidateNames = new();
    private Poll? _activePoll;

    public IReadOnlyList<string> RemainingCandidateNames => _remainingCandidateNames;

    public Poll ActivePoll
    {
        get => _activePoll ?? throw new InvalidOperationException("No active poll.");
        set
        {
            _activePoll = value;
            foreach (Candidate c in _activePoll.Candidates)
            {
                _remainingCandidateNames.Add(c.Name);
            }

            CurrentRanking = _activePoll.Candidates.Count;
        }
    }

    public int CurrentRanking { get; private set; }

    public string JurorName { get; set; } = "Anonymous juror";

    public bool Finished => CurrentRanking == 0;

    public void AssignCurrentRankingToCandidate(string candidateName)
    {
        if (_activePoll == null) throw new InvalidOperationException("Active poll is null.");

        Candidate? c = _activePoll.Candidates.SingleOrDefault(x => x.Name == candidateName);

        if (c == null) throw new InvalidOperationException("No candidate with that name.");
        c.Votes.Add(new Vote(JurorName, CurrentRanking));

        _remainingCandidateNames.Remove(candidateName);

        CurrentRanking--;
    }
}