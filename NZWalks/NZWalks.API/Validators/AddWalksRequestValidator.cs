using FluentValidation;

namespace NZWalks.API.Validators
{
    public class AddWalksRequestValidator : AbstractValidator<Models.DTO.AddWalksRequest>
    {
        public AddWalksRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Length).GreaterThan(0);
        }
    }
}
