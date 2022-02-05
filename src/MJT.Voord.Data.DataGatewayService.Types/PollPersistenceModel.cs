using System.Text.Json.Serialization;
using MJT.Voord.VotingDomain.Types;

namespace MJT.Voord.Data.DataGatewayService.Types;

public class PollPersistenceModel
{
    [JsonConstructor]
    public PollPersistenceModel(string pollName, Poll poll)
    {
        PollName = pollName;
        Poll = poll;
    }

    public string PollName { get; set; }

    public Poll Poll { get; set; }
}