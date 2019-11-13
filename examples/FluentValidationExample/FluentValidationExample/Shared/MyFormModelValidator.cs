using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace FluentValidationExample.Shared
{
    public class MyFormModelValidator : AbstractValidator<MyFormModel>
    {
        public MyFormModelValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("Please specify a first name");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Please specify a last name");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Please specify a valid email");
            RuleFor(x => x.Address).NotEmpty().Length(5,50).WithMessage("Please specify a valid address"); ;
            RuleFor(x => x.Age).NotEqual(0).WithMessage("Age should be more than zero");
        }
    }
}
