using Tournament.Api.Application.Dtos;
using Tournament.Api.Core.Entities;
using Tournament.Api.Core.Enums;
using Tournament.Api.Infrastructure.Context;

namespace Tournament.Api.Application.Services
{

    public interface ITournamentService
    {

        Task<Core.Entities.Tournament> CreateTournamentAsync(CreateTournamentDto tournament, CancellationToken cancellationToken);
    }
    internal class TournamentService : ITournamentService
    {
        private readonly AppDbContext _dbContext;
        public TournamentService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Core.Entities.Tournament> CreateTournamentAsync(CreateTournamentDto tournament, CancellationToken cancellationToken)
        {

            var tournamentDb = new Core.Entities.Tournament
            {
                Name = tournament.Name.Trim(),
            };
            var teams = new List<TournamentTeam>();
            for (int i = 1; i <= tournament.Count; i++)
            {
                teams.Add(new TournamentTeam { TeamName = $"Team {i}" });
            }

            tournamentDb.Teams = teams;



            int totalSlots = (int)Math.Pow(2, Math.Ceiling(Math.Log2(teams.Count))); // nearest power of 2
            int totalRounds = (int)Math.Log2(totalSlots);

            var firstRoundMatches = new List<TournamentMatch>();

            for (int i = 0; i < totalSlots / 2; i++)
            {
                var match = new TournamentMatch
                {

                    RoundNumber = 1,
                    Status = MatchState.Pending,
                    TeamA = i * 2 < teams.Count ? teams[i * 2] : null,
                    TeamB = i * 2 + 1 < teams.Count ? teams[i * 2 + 1] : null,
                };
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

                    nextRoundMatches.Add(match);
                }

                allMatches.AddRange(nextRoundMatches);
                currentRoundMatches = nextRoundMatches;
            }

            tournamentDb.Matches = allMatches;

            await _dbContext.Set<Core.Entities.Tournament>().AddAsync(tournamentDb, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return tournamentDb;

        }



    }
}
