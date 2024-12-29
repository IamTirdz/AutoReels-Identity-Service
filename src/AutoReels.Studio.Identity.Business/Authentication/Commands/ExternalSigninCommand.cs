using MediatR;
using Microsoft.AspNetCore.Http;

namespace AutoReels.Studio.Identity.Business.Authentication.Commands
{
    public record ExternalSigninCommand(HttpContext HttpContext) : IRequest<string>
    {
    }
}
