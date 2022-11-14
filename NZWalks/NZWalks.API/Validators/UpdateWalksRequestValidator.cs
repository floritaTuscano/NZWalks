using FluentValidation;

namespace NZWalks.API.Validators
{
    public class UpdateWalksRequestValidator : AbstractValidator<Models.DTO.UpdateWalksRequest>
    {
        public UpdateWalksRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Length).GreaterThan(0);
        }
    }
}
