using AutoReels.Studio.Identity.Common.Models;

namespace AutoReels.Studio.Identity.Common.Exceptions
{
    public class NotFoundException : BaseException
    {
        public NotFoundException()
        {
        }

        public NotFoundException(ErrorResponse errorResponse) : base(errorResponse)
        {
        }
    }
}
