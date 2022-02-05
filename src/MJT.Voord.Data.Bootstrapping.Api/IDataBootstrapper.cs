using MJT.Voord.VotingDomain.Types;

namespace MJT.Voord.Data.Bootstrapping.Api;

public interface IDataBootstrapper
{
    IEnumerable<Poll> GetSeedData();
}