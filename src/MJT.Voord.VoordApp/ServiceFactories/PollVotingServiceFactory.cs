using MJT.Voord.Voting.VotingService.Api;
using MJT.Voord.Voting.VotingService.Types;

namespace MJT.Voord.VoordApp.ServiceFactories;

public class PollVotingServiceFactory : IPollVotingServiceFactory
{
    public IPollVotingService CreateInstance()
    {
        return new VoteRecorder();
    }
}