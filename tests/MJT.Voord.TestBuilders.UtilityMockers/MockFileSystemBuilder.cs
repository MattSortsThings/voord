using System.IO.Abstractions.TestingHelpers;

namespace MJT.Voord.TestBuilders.UtilityMockers;

public class MockFileSystemBuilder
{
    private readonly MockFileSystem _mock;

    private MockFileSystemBuilder()
    {
        _mock = new MockFileSystem();
    }

    public static MockFileSystemBuilder New()
    {
        return new MockFileSystemBuilder();
    }

    public MockFileSystemBuilder WithTextFile(string path, IEnumerable<string> lines)
    {
        string contents = string.Join('\n', lines);
        _mock.AddFile(path, contents);

        return this;
    }

    public MockFileSystemBuilder WithEmptyDirectory(string path)
    {
        _mock.AddDirectory(path);

        return this;
    }

    public MockFileSystem Build()
    {
        return _mock;
    }
}