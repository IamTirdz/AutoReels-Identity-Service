using FluentValidation;
using MediatR;

namespace AutoReels.Studio.Identity.Business.User.Commands
{
    public record DeleteUserCommand(string Email) : IRequest<bool>
    {
    }

    public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserCommandValidator()
        {
            RuleFor(v => v.Email)
                .Cascade(CascadeMode.Continue)
                .EmailAddress();
        }
    }
}
