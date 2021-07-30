using Application.Core;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.GridLayout
{
    public class List
    {
        public class Query: IRequest<Result<List<GridLayoutMaster>>>{}

        public class Handler : IRequestHandler<Query, Result<List<GridLayoutMaster>>>
        {
            private readonly DataContext _context;
            private readonly ILogger<List> _logger;

            public Handler(DataContext context,ILogger<List> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<Result<List<GridLayoutMaster>>> Handle(Query request, CancellationToken cancellationToken)
            {
                return Result<List<GridLayoutMaster>>.Success(await _context.GridLayoutMasters.ToListAsync(cancellationToken));
            }
        }
    }
}