using FluentValidation;
using IrisExLibStd.Model;

namespace IrisExLibStd.Validator
{
    public class IrisModelValidator : AbstractValidator<IrisModel>
    {
        public IrisModelValidator()
        {
            RuleFor(x => x.PetalLength)
                .NotNull();

            RuleFor(x => x.PetalWidth)
                .NotNull();

            RuleFor(x => x.SepalLength)
                .NotNull();

            RuleFor(x => x.SepalWidth)
                .NotNull();

            RuleFor(y => y.Label)
                .Empty();

        }
    }
}
