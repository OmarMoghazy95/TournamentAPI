using Microsoft.EntityFrameworkCore;
using Tournament.Api.Core;
using Tournament.Api.Core.Entities;
using Tournament.Api.Infrastructure.Context;

namespace Tournament.Api.Infrastructure.Repositories
{

    public interface ITournamentRepository
    {
        Task<Response<Core.Entities.Tournament>> AddNewTournament(Core.Entities.Tournament tournament, CancellationToken cancellationToken = default);
        Task<Response<Core.Entities.Tournament>> GetById(Guid Id, CancellationToken cancellationToken = default);
    }
    public class TournamentRepository : ITournamentRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly DbSet<Core.Entities.Tournament> _dbset;

        public TournamentRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbset = _dbContext.Set<Core.Entities.Tournament>();
        }

        public async Task<Response<Core.Entities.Tournament>> AddNewTournament(Core.Entities.Tournament tournament, CancellationToken cancellationToken = default)
        {

            try
            {
                await _dbset.AddAsync(tournament, cancellationToken);

                var saved = await _dbContext.SaveChangesAsync(cancellationToken);

                if (saved < 1)
                    return Result.Validation<Core.Entities.Tournament>("failed to create tournament", default);

                return Result.Success("tournament data created succesfully", tournament);
            }
            catch (Exception ex)
            {

                return Result.Error<Core.Entities.Tournament>("un expected error happend while saving tournament to database", [ex.Message, ex.InnerException?.Message, ex.StackTrace]);

                throw;
            }

        }

        public async Task<Response<Core.Entities.Tournament>> GetById(Guid Id, CancellationToken cancellationToken = default)
        {
            var tournamentData = await _dbset
                .Include(a => a.Matches)
                .ThenInclude(a => a.TeamA)
                .Include(a => a.Matches)
                .ThenInclude(a => a.TeamB)
                .Include(a => a.Matches)
                .ThenInclude(a => a.Winner)
                .AsNoTracking().FirstOrDefaultAsync(a => a.Id == Id, cancellationToken);

            if (tournamentData is null)
                return Result.Validation<Core.Entities.Tournament>("tournament data not found", []);

            return Result.Success("data found!", tournamentData);
        }
    }
}
