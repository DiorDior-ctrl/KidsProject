
using FluentValidation;
using LessonService.Application.DTOs.Requests;

namespace LessonService.Application.Validators;

public class CreateExerciseValidator : AbstractValidator<CreateExerciseRequest>
{
    public CreateExerciseValidator()
    {
        RuleFor(x => x.ContentJson)
            .NotEmpty().WithMessage("Content nuk mund të jetë bosh.");

        RuleFor(x => x.CorrectAnswer)
            .NotEmpty().WithMessage("Përgjigja e saktë nuk mund të jetë bosh.");

        RuleFor(x => x.XpReward)
            .GreaterThan(0).WithMessage("XP reward duhet të jetë më i madh se 0.");

        RuleFor(x => x.OrderIndex)
            .GreaterThan(0).WithMessage("Order index duhet të jetë më i madh se 0.");
    }
}