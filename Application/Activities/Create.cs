using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class Create
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Activity Activity { get; set; }
        }
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(z=>z.Activity).SetValidator(new ActivityValidator());
            }
        }

        public class Handler : IRequestHandler<Command,Result<Unit>>
        {
            private readonly DataContext __datacontext;
            public Handler(DataContext _datacontext)
            {
                __datacontext = _datacontext;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                __datacontext.Activities.Add(request.Activity);
                var result = await __datacontext.SaveChangesAsync() > 0;
                if(!result) return Result<Unit>.Failure("Failed to Create Activity");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}