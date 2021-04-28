using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class Delete
    {
        public class Command : IRequest
        {
            public Guid id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext __Context;
            public Handler(DataContext _Context)
            {
                __Context = _Context;

            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await __Context.Activities.FindAsync(request.id);
                __Context.Remove(activity);

                await __Context.SaveChangesAsync();
                return Unit.Value;
            }
        }
    }
}