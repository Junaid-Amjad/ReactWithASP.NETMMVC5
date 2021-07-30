using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.GridLayout
{
    public class Master
    {
        public class Query:IRequest<Result<List<GridLayoutMaster>>>{

        }
        public class Handler : IRequestHandler<Query, Result<List<GridLayoutMaster>>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<List<GridLayoutMaster>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var gridLayoutMaster = await _context.GridLayoutMasters.ToListAsync();                
                return Result<List<GridLayoutMaster>>.Success(gridLayoutMaster);
            }
        }
    }
}