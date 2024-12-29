using AutoReels.Studio.Identity.Common.Models;

namespace AutoReels.Studio.Identity.Common.Exceptions
{
    public class BadRequestException : BaseException
    {
        public BadRequestException()
        {
        }

        public BadRequestException(ErrorResponse errorResponse) : base(errorResponse)
        {
        }
    }
}
