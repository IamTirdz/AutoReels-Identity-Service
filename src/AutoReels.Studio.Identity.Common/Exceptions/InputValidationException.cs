using AutoReels.Studio.Identity.Common.Models;

namespace AutoReels.Studio.Identity.Common.Exceptions
{
    public class InputValidationException : BaseException
    {
        public InputValidationException()
        {
        }

        public InputValidationException(ErrorResponse errorResponse) : base(errorResponse)
        {
        }
    }
}
