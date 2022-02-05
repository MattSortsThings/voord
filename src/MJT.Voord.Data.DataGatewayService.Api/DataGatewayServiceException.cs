namespace MJT.Voord.Data.DataGatewayService.Api;

public class DataGatewayServiceException : Exception
{
    public DataGatewayServiceException()
    {
        
    }

    public DataGatewayServiceException(string message) : base(message)
    {
    }

    public DataGatewayServiceException(string message, Exception inner) : base(message, inner)
    {
    }
}