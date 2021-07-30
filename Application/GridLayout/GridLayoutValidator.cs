using Domain;
using FluentValidation;

namespace Application.GridLayout
{
    public class GridLayoutValidator : AbstractValidator<GridLayoutMaster>
    {
        public GridLayoutValidator()
        {
            RuleFor(z => z.LayoutName).NotEmpty();
            RuleFor(z => z.NoofColumns).NotEmpty();
        }
    }

    public class GridLayoutValidatorForDetail : AbstractValidator<GridLayoutDetail>
    {
        public GridLayoutValidatorForDetail()
        {
            RuleFor(z => z.CameraIP).NotEmpty();
            RuleFor(z => z.GridLayoutMasterID).NotEmpty();
        }
    }
}