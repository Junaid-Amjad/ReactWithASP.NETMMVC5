using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Persistence;

namespace Application.GridLayout
{
    public class Delete
    {
        public class Command: IRequest<Result<Unit>>{
            public long GridLayoutMasterID {get;set;}
        }
        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
                {
                    try {
                        var gridLayoutDetails = await _context.GridLayoutDetails.Where(x => x.GridLayoutMasterID == request.GridLayoutMasterID).ToListAsync();
                        foreach (var gridLayoutDetail in gridLayoutDetails)
                        {
                            _context.Remove(gridLayoutDetail);
                            await _context.SaveChangesAsync();
                        }
                        var gridLayoutMaster = await _context.GridLayoutMasters.FindAsync(request.GridLayoutMasterID);
                        if (gridLayoutMaster == null) return Result<Unit>.Failure("No Record Found in Model");
                        _context.Remove(gridLayoutMaster);
                        var result = await _context.SaveChangesAsync() > 0;
                        if (!result) return Result<Unit>.Failure("No Record Found in Database");
                        await transaction.CommitAsync();
                        return Result<Unit>.Success(Unit.Value);
                    }
                    catch(Exception)
                    {
                        await transaction.RollbackAsync();
                        return Result<Unit>.Failure("Issue in the Transaction");
                    }
                }

            }
        }
    }
}