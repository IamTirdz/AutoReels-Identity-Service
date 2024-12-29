using FluentValidation;
using MediatR;

namespace AutoReels.Studio.Identity.Business.Password.Commands
{
    public record ResetPasswordCommand(ResetPasswordRequest Request) : IRequest<bool>
    {
    }

    public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidator()
        {
            RuleFor(v => v.Request.Email)
                .Cascade(CascadeMode.Continue)
                .EmailAddress();

            RuleFor(v => v.Request.Code)
                .Cascade(CascadeMode.Continue)
                .NotEmpty()
                    .WithMessage("{PropertyName} should not be empty");

            RuleFor(v => v.Request.Password)
                .Cascade(CascadeMode.Continue)
                .NotEmpty()
                    .WithMessage("{PropertyName} should not be empty")
                .Length(5, 15)
                .Matches(@"^(?=.*\d)(?=.*[a-zA-Z])(?=.*\W)(?=.*[a-zA-Z]).{0,}$")
                    .WithMessage("{PropertyName} must have 1 uppercase, 1 lowercase and 1 number");
        }
    }
}
