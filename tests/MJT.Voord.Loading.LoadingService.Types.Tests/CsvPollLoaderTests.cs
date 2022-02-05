using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using FluentAssertions;
using MJT.Voord.TestBuilders.UtilityMockers;
using MJT.Voord.VotingDomain.Types;
using Xunit;

namespace MJT.Voord.Loading.LoadingService.Types.Tests;

public class CsvPollLoaderTests
{
    [Theory]
    [ClassData(typeof(TestDataSupplier.LoadFromCsvMethodHappyPath))]
    public void CanLoadFromCsvFile(string srcFilePath, IEnumerable<string> srcFileLines, Poll expected)
    {
        // Arrange
        MockFileSystem fileSystem = MockFileSystemBuilder.New()
            .WithTextFile(srcFilePath, srcFileLines)
            .Build();

        var sut = new CsvPollLoader(fileSystem);

        // Act
        Poll result = sut.LoadFromCsv(srcFilePath);

        // Assert
        result.Candidates.Should().BeEquivalentTo(expected.Candidates);
    }
}