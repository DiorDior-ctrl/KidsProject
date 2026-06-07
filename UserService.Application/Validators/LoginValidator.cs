
using FluentValidation;
using UserService.Application.DTOs.Requests;

namespace UserService.Application.Validators;

public class LoginValidator : AbstractValidator<LoginRequest>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email nuk mund të jetë bosh.")
            .EmailAddress().WithMessage("Email nuk është i vlefshëm.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Fjalëkalimi nuk mund të jetë bosh.");
    }
}