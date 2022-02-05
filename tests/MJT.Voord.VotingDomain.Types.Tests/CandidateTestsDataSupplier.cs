using System;
using Xunit;

namespace MJT.Voord.VotingDomain.Types.Tests;

public class CandidateTestsDataSupplier
{
    public class DeserializationTestCases : TheoryData<string, Candidate>
    {
        public DeserializationTestCases()
        {
            Add("{\"Id\": 10, \"Name\": \"UK\"}",
                new Candidate(10, "UK")
                {
                    Votes = Array.Empty<Vote>()
                }
            );

            Add("{\"Id\": 10, \"Name\": \"UK\", \"Votes\": [" +
                "{\"JurorName\":\"A\", \"Ranking\":1}," +
                "{\"JurorName\":\"B\", \"Ranking\":2}," +
                "{\"JurorName\":\"C\", \"Ranking\":3}" +
                "]}",
                new Candidate(10, "UK")
                {
                    Votes = new[] { new Vote("A", 1), new Vote("B", 2), new Vote("C", 3) }
                });

            Add("{\"Id\": 10, \"Name\": \"UK\"}",
                new Candidate(10, "UK")
                {
                    Votes = Array.Empty<Vote>()
                });

            Add("{\"Id\": 10, \"Name\": \"UK\", \"Votes\": [" +
                "{\"JurorName\":\"A\", \"Ranking\":1}," +
                "{\"JurorName\":\"B\", \"Ranking\":2}," +
                "{\"JurorName\":\"C\", \"Ranking\":3}" +
                "]}",
                new Candidate(10, "UK")
                {
                    Votes = new[] { new Vote("A", 1), new Vote("B", 2), new Vote("C", 3) }
                });
        }
    }
}