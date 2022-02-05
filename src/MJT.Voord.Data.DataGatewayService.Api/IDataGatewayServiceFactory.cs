namespace MJT.Voord.Data.DataGatewayService.Api;

public interface IDataGatewayServiceFactory
{
    IDataGatewayService CreateInstance();
}