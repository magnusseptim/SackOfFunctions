using FluentlyCheckingFunction.Model;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentlyCheckingFunction.Validator
{
    public class OrderModelValidator : AbstractValidator<OrderModel>
    {
        public OrderModelValidator()
        {
            RuleFor(x => x.ID)
                .NotEmpty();

            RuleFor(x => x.Warename)
                .NotEmpty()
                .MinimumLength(3)
                .Must(x => x.Contains("WRE"));

            RuleFor(x => x.Quantity)
                .NotNull()
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Unit)
                .NotEmpty()
                .Must(x => x.Length >= 2);
        }
    }
}
