using Microsoft.EntityFrameworkCore;
using Tournament.Api.Application.Dtos.Matches;
using Tournament.Api.Core;
using Tournament.Api.Core.Entities;
using Tournament.Api.Infrastructure.Context;
using Tournament.Api.Infrastructure.Repositories;

namespace Tournament.Api.Application.Services;


public interface IMatchService
{
    Task<Response<TournamentMatch>> FinishMatch(Guid matchId, UpdateMatchResultDto result, CancellationToken cancellationToken = default);
}
public class MatchService : IMatchService
{
    private readonly IMatchRepository _repository;
    public MatchService(IMatchRepository repository)
    {
        _repository = repository;
    }

    public async Task<Response<TournamentMatch>> FinishMatch(Guid matchId, UpdateMatchResultDto result, CancellationToken cancellationToken = default)
    {

        var matchResponse = await _repository.GetMatchById(matchId, cancellationToken);

        if (matchResponse.Status is not ResultEnum.Success)
            return matchResponse;

        var match = matchResponse.Data;

        if (match.Status is Core.Enums.MatchState.Finished)
            return Result.Validation<TournamentMatch>("can't finish or close finished match", []);


        switch (result.Result)
        {
            case Enums.MatchResult.Winner:
            case Enums.MatchResult.WithdrawOne:
                match.WinnerId = result.WinningTeamId;
                match.Status = Core.Enums.MatchState.Finished;

                if (match.ChildMatchA is not null)
                {
                    match.ChildMatchA.TeamAId = match.ChildMatchA.TeamAId.HasValue ? match.ChildMatchA?.TeamAId : result.WinningTeamId;
                    match.ChildMatchA.TeamBId = match.ChildMatchA.TeamBId.HasValue ? match.ChildMatchA?.TeamBId : result.WinningTeamId;

                    if (match.ChildMatchA.ParentMatchB.Status is Core.Enums.MatchState.Finished && match.ChildMatchA.ParentMatchB.WinnerId is null)
                    {
                        match.ChildMatchA.WinnerId = match.ChildMatchA.TeamAId = match.WinnerId;
                        match.ChildMatchA.Status = Core.Enums.MatchState.Finished;
                    }
                }

                else if (match.ChildMatchB is not null)
                {
                    match.ChildMatchB.TeamAId = match.ChildMatchB.TeamAId.HasValue ? match.ChildMatchB?.TeamAId : result.WinningTeamId;
                    match.ChildMatchB.TeamBId = match.ChildMatchB.TeamBId.HasValue ? match.ChildMatchB?.TeamBId : result.WinningTeamId;

                    if (match.ChildMatchB.ParentMatchA.Status is Core.Enums.MatchState.Finished && match.ChildMatchB.ParentMatchA.WinnerId is null)
                    {
                        match.ChildMatchB.WinnerId = match.ChildMatchB.TeamAId = match.WinnerId;
                        match.ChildMatchA.Status = Core.Enums.MatchState.Finished;
                    }
                }
                break;
            case Enums.MatchResult.WithdrawBoth:
                match.Status = Core.Enums.MatchState.Finished;
                if (match.ChildMatchA is not null)
                {
                    if (match.ChildMatchA.ParentMatchB.Status is Core.Enums.MatchState.Finished)
                        match.ChildMatchA.WinnerId = match.ChildMatchA.ParentMatchB.WinnerId;
                }
                else if (match.ChildMatchB is not null)
                {
                    if (match.ChildMatchB.ParentMatchA.Status is Core.Enums.MatchState.Finished)
                        match.ChildMatchB.WinnerId = match.ChildMatchB.ParentMatchA.WinnerId;
                }

                break;
        }

        var saveResponse = await _repository.SaveChangesAsync(cancellationToken);

        if (saveResponse.Status is not ResultEnum.Success)
            return Result.Validation<TournamentMatch>(saveResponse.Message, saveResponse.Errors);

        return Result.Success("match updated succesfully", match);

    }
}


