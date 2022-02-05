using System.Text.Json;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace MJT.Voord.VotingDomain.Types.Tests;

public class CandidateTests
{
    [Theory]
    [ClassData(typeof(CandidateTestsDataSupplier.DeserializationTestCases))]
    public void CanDeserialize(string jsonString, Candidate expected)
    {
        // Act
        var result = JsonSerializer.Deserialize<Candidate>(jsonString);

        // Assert
        using (new AssertionScope())
        {
            result!.Id.Should().Be(expected.Id);
            result.Name.Should().Be(expected.Name);
            result.Votes.Should().BeEquivalentTo(expected.Votes);
        }
    }
}