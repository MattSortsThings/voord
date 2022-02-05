using System;
using System.Collections.Generic;
using MJT.Voord.VotingDomain.Types;
using Xunit;

namespace MJT.Voord.Loading.LoadingService.Types.Tests;

public class TestDataSupplier
{
    public class LoadFromCsvMethodHappyPath : TheoryData<string, IEnumerable<string>, Poll>
    {
        public LoadFromCsvMethodHappyPath()
        {
            Add(
                "C:\\temp\\file1.csv",
                new[]
                {
                    "Name"
                },
                new Poll(Array.Empty<Candidate>())
            );


            Add("C:\\temp\\file2.csv",
                new[]
                {
                    "Name",
                    "UK",
                    "France",
                    "Ireland"
                },
                new Poll(new[]
                {
                    new Candidate(1, "UK"),
                    new Candidate(2, "France"),
                    new Candidate(3, "Ireland")
                })
            );

            Add("C:\\temp\\file3.csv",
                new[]
                {
                    "Name",
                    "UK",
                    "France",
                    "Ireland"
                },
                new Poll(new[]
                {
                    new Candidate(1, "UK"),
                    new Candidate(2, "France"),
                    new Candidate(3, "Ireland")
                })
            );
        }
    }
}