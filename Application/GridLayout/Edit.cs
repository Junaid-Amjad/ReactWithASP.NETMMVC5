using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Persistence;

namespace Application.GridLayout
{
    public class Edit
    {
        public class Command : IRequest<Result<Unit>>{
            public GridLayoutMaster Master { get; set; }
            public List<GridLayoutDetail> Detail { get; set; }
        }
        public class CommandValidator:AbstractValidator<Command>{
            public CommandValidator(){
//                RuleFor(m => m.Master).SetValidator(new GridLayoutValidator());
            }
        }
        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context,IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                using(IDbContextTransaction transaction = _context.Database.BeginTransaction()){
                    try{
                        var Master = await _context.GridLayoutMasters.FindAsync(request.Master.GridLayoutMasterID);
                        if(Master == null){await transaction.RollbackAsync();return Result<Unit>.Failure("GridLayoutMaster not found");} 
                        _mapper.Map(request.Master,Master);
                        var result = await _context.SaveChangesAsync() >0;
                        //if(!result) return  Result<Unit>.Failure("Failure to update Master");
                        var gridLayoutDetail = await _context.GridLayoutDetails.Where(m => m.GridLayoutMasterID == request.Master.GridLayoutMasterID).ToListAsync();
                        bool isUpdated = false;
                        foreach (var item in gridLayoutDetail)
                        {
                            _context.Remove(item);           
                            isUpdated = await _context.SaveChangesAsync() >0;          
                        }
                        if(!isUpdated){ await transaction.RollbackAsync();return Result<Unit>.Failure("Failure to update Detail");}
                        foreach (var item in request.Detail)
                        {
                            item.GridLayoutMasterID = request.Master.GridLayoutMasterID;
                            _context.GridLayoutDetails.Add(item);
                            await _context.SaveChangesAsync();
                        }
                        await transaction.CommitAsync();
                        return Result<Unit>.Success(Unit.Value);

                    }
                    catch(Exception){
                        await transaction.RollbackAsync();
                        return Result<Unit>.Failure("Failure In Transaction");
                    }
                }
            }
        }
    }
}