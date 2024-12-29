using FluentValidation;
using MediatR;

namespace AutoReels.Studio.Identity.Business.Email.Commands
{
    public record VerifyEmailCommand(VerifyEmailRequest Request) : IRequest<bool>
    {
    }

    public class VerifyEmailQueryValidator : AbstractValidator<VerifyEmailCommand>
    {
        public VerifyEmailQueryValidator()
        {
            RuleFor(v => v.Request.Email)
                .Cascade(CascadeMode.Continue)
                .EmailAddress();

            RuleFor(v => v.Request.Token)
                .Cascade(CascadeMode.Continue)
                .NotEmpty()
                    .WithMessage("{PropertyName} should not be empty");
        }
    }
}
