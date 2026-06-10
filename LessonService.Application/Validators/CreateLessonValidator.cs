
using FluentValidation;
using LessonService.Application.DTOs.Requests;

namespace LessonService.Application.Validators;

public class CreateLessonValidator : AbstractValidator<CreateLessonRequest>
{
    public CreateLessonValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Titulli nuk mund të jetë bosh.")
            .MaximumLength(200).WithMessage("Titulli nuk mund të jetë më i gjatë se 200 karaktere.");

        RuleFor(x => x.OrderIndex)
            .GreaterThan(0).WithMessage("Order index duhet të jetë më i madh se 0.");

        RuleFor(x => x.XpReward)
            .GreaterThan(0).WithMessage("XP reward duhet të jetë më i madh se 0.");
    }
}