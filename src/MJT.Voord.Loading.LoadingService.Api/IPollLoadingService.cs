using MJT.Voord.VotingDomain.Types;

namespace MJT.Voord.Loading.LoadingService.Api;

public interface IPollLoadingService
{
    Poll LoadFromCsv(string srcFilePath);
}