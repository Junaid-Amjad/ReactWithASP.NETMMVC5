using Application.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Right
{
    public class UserRightValue
    {
        public class Query : IRequest<Result<long>> {
            public string UserID { get; set; }
        }
        public class Handler : IRequestHandler<Query, Result<long>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                this._context = context;
            }

            public async Task<Result<long>> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await _context.RightsAllotmentMs.Where(z => z.UserID == request.UserID).SingleOrDefaultAsync();
                if (result == null) return Result<long>.Failure("No User Found");
                return Result<long>.Success(result.TotalRightValue);
            }
        }
    }
}