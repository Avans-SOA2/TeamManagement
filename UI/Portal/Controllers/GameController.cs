using Core.Domain;
using Core.DomainServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Portal.Models;
using Microsoft.EntityFrameworkCore;
using CommunityToolkit.Diagnostics;


namespace Portal.Controllers;

public class GameController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IGameRepository _gameRepository;
    private readonly ICoachRepository _coachRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly IOpponentRepository _opponentRepository;
    private readonly ITeamRepository _teamRepository;

    public GameController(ILogger<HomeController> logger,
        IGameRepository gameRepository,
        ICoachRepository coachRepository,
        IPlayerRepository playerRepository,
        IOpponentRepository opponentRepository,
        ITeamRepository teamRepository)
    {
        ArgumentException.ThrowIfNullOrEmpty(nameof(gameRepository));
        Guard.IsNotNull(coachRepository); // <-- gebruikt CallerArgumentExpression (https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/proposals/csharp-10.0/caller-argument-expression)
        //Guard.IsNotNull(coachRepository, nameof(coachRepository));

        _logger = logger;
        _gameRepository = gameRepository;
        _coachRepository = coachRepository;
        _playerRepository = playerRepository;
        _opponentRepository = opponentRepository ?? throw new ArgumentNullException(nameof(opponentRepository));
        _teamRepository = teamRepository;
    }

    public IActionResult Index()
    {
        return View(_gameRepository.GetAll().ToViewModel());
    }

    [Authorize]
    public IActionResult List(string team)
    {
        var model = _gameRepository.GetAll().Include(game => game.Team)
            .Where(game => team == null || game.Team.Name == team).OrderByDescending(game => game.PlayTime).ToList().ToViewModel();

        return View(model);
    }

    [Authorize(Policy = "TeamManagerOnly")]
    [HttpGet]
    public IActionResult NewGame()
    {
        var model = new NewGameViewModel();

        PrefillSelectOptions();

        return View(model);
    }

    private void PrefillSelectOptions()
    {
        var coaches = _coachRepository.GetCoaches().Prepend(new Coach() { Id = -1, Name = "Select a coach" });
        ViewBag.Coaches = new SelectList(coaches, "Id", "Name");

        var careTakers = _playerRepository.GetPlayers().SelectMany(p => p.CareTakers)
            .Prepend(new CareTaker { Id = -1, Name = "Select a caretaker" });
        ViewBag.CareTakers = new SelectList(careTakers, "Id", "Name");
    }

    [Authorize(Policy = "TeamManagerOnly")]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> NewGame(NewGameViewModel newGame)
    {
        if (newGame.IsHomeGame && newGame.DepartureTime.HasValue)
        {
            ModelState.AddModelError(nameof(newGame.DepartureTime),
                "Vertrektijd mag niet op worden gegeven bij een thuiswedstrijd");
        }

        if (ModelState.IsValid)
        {
            var gameToCreate = new Game(newGame.PlayTime, newGame.IsHomeGame);

            if (newGame.CoachId != -1)
            {
                var selectedCoach = _coachRepository.GetById(newGame.CoachId);
                gameToCreate.Coach = selectedCoach;
            }
            else
            {
                var teams = _teamRepository.GetTeams();
                var team = teams.FirstOrDefault(t => t.Name == "");
                var selectedCoach = team.TeamHeadCoach ?? throw new Exception("Coach niet aanwezig");
                gameToCreate.Coach = selectedCoach;
                var players = _playerRepository.GetPlayers()
                    .OrderBy(p => p.Games.Count)
                    .Take(12)
                    .ToList();
                gameToCreate.Drivers = players
                    .SelectMany(p => p.CareTakers.Where(c => c.HasCar))
                    .ToList();

            }

            if (newGame.LaundryDutyId != -1)
            {
                var careTakers = _playerRepository.GetPlayers().SelectMany(p => p.CareTakers);
                var selectedCareTakerForLaundryDuty =
                    careTakers.SingleOrDefault(careTaker => careTaker.Id == newGame.LaundryDutyId);
                gameToCreate.LaundryDuty = selectedCareTakerForLaundryDuty;
            }

            await _gameRepository.AddGame(gameToCreate);

            return RedirectToAction("Index");
        }

        PrefillSelectOptions();

        return View(newGame);

    }
}
