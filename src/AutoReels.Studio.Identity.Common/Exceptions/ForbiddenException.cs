using AutoReels.Studio.Identity.Common.Models;

namespace AutoReels.Studio.Identity.Common.Exceptions
{
    public class ForbiddenException : BaseException
    {
        public ForbiddenException()
        {
        }

        public ForbiddenException(ErrorResponse errorResponse) : base(errorResponse)
        {
        }
    }
}
