using Application.Core;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.GridLayout
{
    public class Create
    {
        public class Command: IRequest<Result<Unit>>
        {
            public GridLayoutMaster Master { get; set; }
            public List<GridLayoutDetail> Detail { get; set; }
        }
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
//                RuleFor(z => z.Master).SetValidator(new GridLayoutValidator());
//                RuleFor(z => z.Detail).SetValidator(new GridLayoutValidatorForDetail());
            }
        }
        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _dataContext;

            public Handler(DataContext dataContext)
            {
                _dataContext = dataContext;
            }
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                using (IDbContextTransaction transaction = _dataContext.Database.BeginTransaction())
                {
                    try{
                        _dataContext.GridLayoutMasters.Add(request.Master);
                        var result = await _dataContext.SaveChangesAsync() > 0;
                        if (!result) return Result<Unit>.Failure("Issue with DB");
                        foreach (var item in request.Detail)
                        {
                            item.GridLayoutMasterID = request.Master.GridLayoutMasterID;
                            _dataContext.GridLayoutDetails.Add(item);
                            await _dataContext.SaveChangesAsync();
                        }
                        await transaction.CommitAsync();
                        return Result<Unit>.Success(Unit.Value);

                    }
                    catch(Exception){
                        transaction.RollbackAsync();
                        return Result<Unit>.Failure(Unit.Value.ToString());

                    }
                    
                }
            }
        }
    }
}
