
using FluentValidation;
using UserService.Application.DTOs.Requests;

namespace UserService.Application.Validators;

public class RegisterParentValidator : AbstractValidator<RegisterParentRequest>
{
    public RegisterParentValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email nuk mund të jetë bosh.")
            .EmailAddress().WithMessage("Email nuk është i vlefshëm.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Fjalëkalimi nuk mund të jetë bosh.")
            .MinimumLength(8).WithMessage("Fjalëkalimi duhet të ketë të paktën 8 karaktere.")
            .Matches("[A-Z]").WithMessage("Fjalëkalimi duhet të ketë të paktën një shkronjë të madhe.")
            .Matches("[0-9]").WithMessage("Fjalëkalimi duhet të ketë të paktën një numër.");

        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password).WithMessage("Fjalëkalimet nuk përputhen.");
    }
}