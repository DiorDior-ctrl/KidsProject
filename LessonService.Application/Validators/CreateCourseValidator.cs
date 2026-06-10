
using FluentValidation;
using LessonService.Application.DTOs.Requests;

namespace LessonService.Application.Validators;

public class CreateCourseValidator : AbstractValidator<CreateCourseRequest>
{
    public CreateCourseValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Titulli nuk mund të jetë bosh.")
            .MaximumLength(200).WithMessage("Titulli nuk mund të jetë më i gjatë se 200 karaktere.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Përshkrimi nuk mund të jetë bosh.")
            .MaximumLength(1000).WithMessage("Përshkrimi nuk mund të jetë më i gjatë se 1000 karaktere.");

        RuleFor(x => x.TargetAgeMin)
            .InclusiveBetween(4, 12).WithMessage("Mosha minimale duhet të jetë ndërmjet 4 dhe 12.");

        RuleFor(x => x.TargetAgeMax)
            .InclusiveBetween(4, 12).WithMessage("Mosha maksimale duhet të jetë ndërmjet 4 dhe 12.")
            .GreaterThan(x => x.TargetAgeMin).WithMessage("Mosha maksimale duhet të jetë më e madhe se minimale.");
    }
}