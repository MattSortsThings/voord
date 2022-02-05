using System.Text.Json.Serialization;

namespace MJT.Voord.VotingDomain.Types;

[Serializable]
public class Candidate
{
    [JsonConstructor]
    public Candidate(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public int Id { get; }

    public string Name { get; }

    public IList<Vote> Votes { get; init; } = new List<Vote>();
}