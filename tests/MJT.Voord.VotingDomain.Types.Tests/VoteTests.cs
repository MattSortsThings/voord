using System.Text.Json;
using FluentAssertions;
using Xunit;

namespace MJT.Voord.VotingDomain.Types.Tests;

public class VoteTests
{
    [Fact]
    public void CanDeserialize()
    {
        // Arrange
        const string jsonString = "{\"JurorName\": \"Matt\", \"Ranking\": 10}";

        // act
        var result = JsonSerializer.Deserialize<Vote>(jsonString);

        // Assert
        var expected = new Vote("Matt", 10);

        result.Should().BeEquivalentTo(expected);
    }
}