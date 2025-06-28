using Tournament.Api.Application.Dtos.Tournaments;
using Tournament.Api.Core;
using Tournament.Api.Core.Entities;
using Tournament.Api.Core.Enums;
using Tournament.Api.Infrastructure.Context;
using Tournament.Api.Infrastructure.Repositories;

namespace Tournament.Api.Application.Services
{

    public interface ITournamentService
    {

        Task<Response<Core.Entities.Tournament>> CreateTournamentAsync(CreateTournamentDto tournament, CancellationToken cancellationToken);
        Task<Response<GetTournamentDto>> GetTournamentById(Guid id, CancellationToken cancellationToken);
    }
    internal class TournamentService : ITournamentService
    {
        private readonly ITournamentRepository _repository;
        public TournamentService(AppDbContext dbContext, ITournamentRepository repository)
        {
            _repository = repository;
        }
        public async Task<Response<Core.Entities.Tournament>> CreateTournamentAsync(CreateTournamentDto tournament, CancellationToken cancellationToken)
        {

            var tournamentDb = new Core.Entities.Tournament
            {
                Name = tournament.Name.Trim(),
            };
            var teams = new List<TournamentTeam>();
            for (int i = 1; i <= tournament.Count; i++)
            {
                teams.Add(new TournamentTeam { TeamName = $"team-{i}" });
            }
            int availabile = (int)Math.Pow(2, Math.Ceiling(Math.Log2(teams.Count)));

            if (availabile < 2 || availabile > 128)
                return Result.Validation<Core.Entities.Tournament>("please enter valid teams count", []);

            tournamentDb.Teams = teams;



            int totalRounds = (int)Math.Log2(availabile);

            var firstRoundMatches = new List<TournamentMatch>();

            for (int i = 0; i < availabile / 2; i++)
            {
                var match = new TournamentMatch
                {

                    RoundNumber = 1,
                    Status = MatchState.Pending,
                    TeamA = i * 2 < teams.Count ? teams[i * 2] : null,
                    TeamB = i * 2 + 1 < teams.Count ? teams[i * 2 + 1] : null,
                };

                if (match.TeamA is null && match.TeamB is not null)
                {
                    match.Status = MatchState.Finished;
                    match.Winner = match.TeamB;
                }

                else if (match.TeamA is not null && match.TeamB is null)
                {
                    match.Status = MatchState.Finished;
                    match.Winner = match.TeamA;
                }

                else if (match.TeamA is null && match.TeamB is null)
                    continue;

                firstRoundMatches.Add(match);
            }

            var allMatches = new List<TournamentMatch>(firstRoundMatches);
            var currentRoundMatches = firstRoundMatches;

            for (uint round = 2; round <= totalRounds; round++)
            {
                var nextRoundMatches = new List<TournamentMatch>();

                for (int i = 0; i < currentRoundMatches.Count; i += 2)
                {
                    var parentA = currentRoundMatches[i];
                    var parentB = i + 1 < currentRoundMatches.Count ? currentRoundMatches[i + 1] : null;

                    var match = new TournamentMatch
                    {
                        RoundNumber = round,
                        Status = MatchState.Pending,
                        ParentMatchA = parentA,
                        ParentMatchB = parentB,
                    };

                    parentA.ChildMatchA = match;
                    if (parentB != null)
                        parentB.ChildMatchB = match;


                    if(parentA.Winner is not null)
                        match.TeamA=parentA.Winner;
                    else if(parentB.Winner is not null)
                        match.TeamA=parentB.Winner;
                    nextRoundMatches.Add(match);
                }

                allMatches.AddRange(nextRoundMatches);
                currentRoundMatches = nextRoundMatches;
            }

            tournamentDb.Matches = allMatches;

            var result = await _repository.AddNewTournament(tournamentDb, cancellationToken);

            return result;

        }

        public async Task<Response<GetTournamentDto>> GetTournamentById(Guid id, CancellationToken cancellationToken)
        {
            var response = await _repository.GetById(id, cancellationToken);

            if (response.Status is not ResultEnum.Success)
                return Result.Validation<GetTournamentDto>(response.Message, response.Errors);

            var mappedResult = new GetTournamentDto
            {
                Id = response.Data.Id,
                Name = response.Data.Name,
                Matches = response.Data.Matches.OrderBy(a=>a.RoundNumber).Select(a => new GetTournamentMatchDto
                {
                    Id = a.Id,
                    Round = a.RoundNumber,
                    Status = a.Status,
                    TeamA = a.TeamA?.TeamName ?? "N/A",
                    TeamB = a.TeamB?.TeamName ?? "N/A",
                    Winner = a.Winner?.TeamName ?? "N/A",
                })
            };

            return Result.Success("tournament data found", mappedResult);
        }
    }
}
