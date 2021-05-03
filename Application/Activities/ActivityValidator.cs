using Domain;
using FluentValidation;

namespace Application.Activities
{
    public class ActivityValidator : AbstractValidator<Activity>
    {
        public ActivityValidator()
        {
            RuleFor(z=>z.Title).NotEmpty();
            RuleFor(z=>z.Description).NotEmpty();
            RuleFor(z=>z.Date).NotEmpty();
            RuleFor(z=>z.Category).NotEmpty();
            RuleFor(z=>z.City).NotEmpty();
            RuleFor(z=>z.Venue).NotEmpty();
        }
    }
}