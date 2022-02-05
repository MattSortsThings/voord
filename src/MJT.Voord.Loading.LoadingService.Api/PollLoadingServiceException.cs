namespace MJT.Voord.Loading.LoadingService.Api;

public class PollLoadingServiceException : Exception
{
    public PollLoadingServiceException()
    {
    }

    public PollLoadingServiceException(string message) : base(message)
    {
    }

    public PollLoadingServiceException(string message, Exception inner) : base(message, inner)
    {
    }
}