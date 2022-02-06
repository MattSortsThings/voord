namespace MJT.Voord.Results.ResultsService.Api;

public class PollResultsServiceException : Exception
{
    public PollResultsServiceException()
    {
    }

    public PollResultsServiceException(string message) : base(message)
    {
    }

    public PollResultsServiceException(string message, Exception inner) : base(message, inner)
    {
    }
}