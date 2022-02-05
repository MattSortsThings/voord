using System.IO;
using System.Text.Json;
using Xunit;
using FluentAssertions;
using FluentAssertions.Execution;

namespace MJT.Voord.VotingDomain.Types.Tests;

public class PollTests
{
    [Fact]
    public void CanDeserialize()
    {
        // Arrange
        const string jsonFilePath = "test-poll.json";
        string contents = File.ReadAllText(jsonFilePath);
        
        // Act
        var result = JsonSerializer.Deserialize<Poll>(contents);

        // Assert
        using (new AssertionScope())
        {
            result!.Candidates.Should().HaveCount(3);
            result.Candidates[0].Name.Should().Be("UK");
            result.Candidates[1].Name.Should().Be("France");
            result.Candidates[2].Name.Should().Be("Ireland");
        }
    }
}