using MJT.Voord.VotingDomain.Types;

namespace MJT.Voord.Data.DataGatewayService.Api;

public interface IDataGatewayService
{
    bool AppDataExists { get; }

    void WipeAllAppData();

    void LoadSeedData();

    void Persist(string pollName, Poll poll);

    Poll LoadPoll(string pollName);

    IReadOnlyList<string> LoadAllPollNames();
}