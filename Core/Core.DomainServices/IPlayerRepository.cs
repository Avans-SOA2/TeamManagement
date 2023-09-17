using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Domain;

namespace Core.DomainServices;

public interface IPlayerRepository
{
    IEnumerable<Player> GetPlayers();

    Task AddPlayer(Player newPlayer);
}