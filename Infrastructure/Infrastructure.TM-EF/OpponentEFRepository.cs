using Core.DomainServices;

namespace Infrastructure;

public class OpponentEFRepository : IOpponentRepository
{
    private readonly GameDbContext _context;

}