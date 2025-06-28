using Microsoft.EntityFrameworkCore;
using Tournament.Api.Core;
using Tournament.Api.Core.Entities;
using Tournament.Api.Infrastructure.Context;

namespace Tournament.Api.Infrastructure.Repositories
{
    public interface IMatchRepository
    {
        Task<Response<TournamentMatch>> GetMatchById(Guid id, CancellationToken cancellationToken);
        Task<Response<bool>> SaveChangesAsync(CancellationToken cancellationToken);

    }
    public class MatchRepository : IMatchRepository
    {
        private readonly AppDbContext _dbContext;

        public MatchRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbset = _dbContext.Set<TournamentMatch>();
        }

        private readonly DbSet<TournamentMatch> _dbset;
        public async Task<Response<TournamentMatch>> GetMatchById(Guid id, CancellationToken cancellationToken)
        {

            var match = await _dbset
            .Include(a => a.ChildMatchA).ThenInclude(a => a.ParentMatchA)
            .Include(a => a.ChildMatchA).ThenInclude(a => a.ParentMatchB)
            .Include(a => a.ChildMatchB).ThenInclude(a => a.ParentMatchA)
            .Include(a => a.ChildMatchB).ThenInclude(a => a.ParentMatchB)
            .Include(a => a.Tournament)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

            if (match is null)
                return Result.Validation<TournamentMatch>("match not found", []);

            return Result.Success("match found !", match);
        }

        public async Task<Response<bool>> SaveChangesAsync(CancellationToken cancellationToken)
        {
            try
            {
                var saved = await _dbContext.SaveChangesAsync(cancellationToken);

                if (saved < 1)
                    return Result.Validation<bool>("an error occured while saving data", []);

                return Result.Success("data saved succesfuly", true);

            }
            catch (Exception ex)
            {
                return Result.Error<bool>("un expected error happend while saving tournament to database", [ex.Message, ex.InnerException?.Message, ex.StackTrace]);

                throw;
            }


        }
    }
}
