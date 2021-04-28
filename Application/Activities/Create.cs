using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class Create
    {
        public class Command : IRequest
        {
            public Activity Activity { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext __datacontext;
            public Handler(DataContext _datacontext)
            {
                __datacontext = _datacontext;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                __datacontext.Activities.Add(request.Activity);
                await __datacontext.SaveChangesAsync();
                return Unit.Value;
            }
        }
    }
}