using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Domain.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Right
{
    public class List
    {
        public class Query : IRequest<Result<List<Rights>>>
        {

        }
        public class Handler : IRequestHandler<Query, Result<List<Rights>>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<List<Rights>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var resultoftherights = await _context.Rights.ToListAsync();
                return Result<List<Rights>>.Success(resultoftherights);
            }
        }

    }
}