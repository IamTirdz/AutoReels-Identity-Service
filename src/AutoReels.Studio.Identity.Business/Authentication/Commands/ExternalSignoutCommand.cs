using MediatR;

namespace AutoReels.Studio.Identity.Business.Authentication.Commands
{
    public record ExternalSignoutCommand(string LogoutId) : IRequest<string>
    {
    }
}
