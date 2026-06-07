
using FluentValidation;
using UserService.Application.DTOs.Requests;

namespace UserService.Application.Validators;

public class RegisterChildValidator : AbstractValidator<RegisterChildRequest>
{
    public RegisterChildValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email nuk mund të jetë bosh.")
            .EmailAddress().WithMessage("Email nuk është i vlefshëm.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Fjalëkalimi nuk mund të jetë bosh.")
            .MinimumLength(6).WithMessage("Fjalëkalimi duhet të ketë të paktën 6 karaktere.");

        RuleFor(x => x.DisplayName)
            .NotEmpty().WithMessage("Emri nuk mund të jetë bosh.")
            .MaximumLength(100).WithMessage("Emri nuk mund të jetë më i gjatë se 100 karaktere.");

        RuleFor(x => x.Age)
            .InclusiveBetween(4, 12).WithMessage("Mosha duhet të jetë ndërmjet 4 dhe 12 vjeç.");

        RuleFor(x => x.AvatarId)
            .NotEmpty().WithMessage("Avatar duhet të zgjidhet.");
    }
}