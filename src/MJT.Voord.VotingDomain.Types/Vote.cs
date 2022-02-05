namespace MJT.Voord.VotingDomain.Types;

public readonly record struct Vote(string JurorName, int Ranking);