using AutoReels.Studio.Identity.Common.Models;

namespace AutoReels.Studio.Identity.Common.Exceptions
{
    public class UnauthorizedException : BaseException
    {
        public UnauthorizedException()
        {
        }

        public UnauthorizedException(ErrorResponse errorResponse) : base(errorResponse)
        {
        }
    }
}
