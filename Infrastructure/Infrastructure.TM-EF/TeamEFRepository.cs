using System;
using System.Collections.Generic;
using Core.Domain;
using Core.DomainServices;

namespace Infrastructure;

public class TeamEFRepository : ITeamRepository
{
    private readonly GameDbContext _context;

    public TeamEFRepository(GameDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    
    public IEnumerable<Team> GetTeams()
    {
        return _context.Teams;
    }
}