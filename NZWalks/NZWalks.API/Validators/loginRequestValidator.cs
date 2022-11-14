using FluentValidation;

namespace NZWalks.API.Validators
{
    public class loginRequestValidator : AbstractValidator<Models.DTO.LoginRequest>
    {
        public loginRequestValidator()
        {
            RuleFor(x => x.UserName).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
