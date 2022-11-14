using FluentValidation;

namespace NZWalks.API.Validators
{
    public class updateWalkDifficultyRequestValidator : AbstractValidator<Models.DTO.UpdateWalkDifficultyRequest>
    {
        public updateWalkDifficultyRequestValidator()
        {
            RuleFor(x => x.Code).NotEmpty();
        }
    }
}
