using Application.Core;
using Domain.Users;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using Persistence;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Right
{
    public class CreateRights
    {
        public class Command: IRequest<Result<Unit>>
        {
            public RightsAllotmentM UserRightsMaster { get; set; }
            public List<RightsAllotmentD> UserRightsDetail { get; set; }
        }
        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                this._context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        _context.RightsAllotmentMs.Add(request.UserRightsMaster);
                        var result = await _context.SaveChangesAsync() > 0;
                        if (!result) { await transaction.RollbackAsync(); return Result<Unit>.Failure("Saving Data Error"); }
                        foreach (var item in request.UserRightsDetail)
                        {
                            item.RightsAllotmentMID = request.UserRightsMaster.RightsAllotmentID;
                            _context.RightsAllotmentDs.Add(item);
                            await _context.SaveChangesAsync();
                            _context.RightsAllotmentHistories.Add(new RightsAllotmentHistory()
                            {
                                RightsAllotmentMID = item.RightsAllotmentMID,
                                RightID = item.RightID,
                                EntryDate = request.UserRightsMaster.EntryDateTime,
                                UserID = request.UserRightsMaster.UserID,
                                UserIP = request.UserRightsMaster.AssignByIP,
                                UserSystem = request.UserRightsMaster.AssignBySystem,
                                EnteredUserID=request.UserRightsMaster.AssignByUserID

                            });
                            await _context.SaveChangesAsync();
                        }
                        await transaction.CommitAsync();
                        return Result<Unit>.Success(Unit.Value);
                    }
                    catch(Exception ex)
                    {
                        await transaction.RollbackAsync();
                        return Result<Unit>.Failure(ex.Message);
                    }
                }
            }
        }

    }
}