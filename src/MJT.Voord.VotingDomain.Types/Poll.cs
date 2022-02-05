namespace MJT.Voord.VotingDomain.Types;

public class Poll
{
    public Poll(IReadOnlyList<Candidate> candidates)
    {
        Candidates = candidates ?? throw new ArgumentNullException(nameof(candidates));
    }


    public IReadOnlyList<Candidate> Candidates { get; }
}