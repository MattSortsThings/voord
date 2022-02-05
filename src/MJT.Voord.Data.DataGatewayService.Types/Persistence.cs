using System.Text.Json.Serialization;

namespace MJT.Voord.Data.DataGatewayService.Types;

public class Persistence
{
    [JsonConstructor]
    public Persistence(IList<PollPersistenceModel> polls)
    {
        Polls = polls ?? throw new ArgumentNullException(nameof(polls));
    }

    public IList<PollPersistenceModel> Polls { get; }
}