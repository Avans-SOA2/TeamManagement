using Core.Domain;
using Core.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Infrastructure.TMEF;
public class GameRestRepository : IGameRepository
{
    private readonly HttpClient _client;

    public GameRestRepository(HttpClient client)
    {
        _client = client;
    }
    public Task AddGame(Game newGame)
    {
        throw new NotImplementedException();
        // POST via _client
    }

    public IEnumerable<Game> Filter(Func<Game, bool> filterExpressie)
    {
        throw new NotImplementedException();

    }

    public IQueryable<Game> GetAll()
    {
        throw new NotImplementedException();
        // GET via client
    }

    public IEnumerable<Game> GetAllExternalGames()
    {
        throw new NotImplementedException();
        // GET via client
    }

    public IEnumerable<Game> GetAllHomeGames()
    {
        throw new NotImplementedException();
        // GET via client
    }
}
