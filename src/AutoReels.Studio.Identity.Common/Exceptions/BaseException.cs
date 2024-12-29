using AutoReels.Studio.Identity.Common.Models;

namespace AutoReels.Studio.Identity.Common.Exceptions
{
    [Serializable]
    public abstract class BaseException : Exception
    {
        public ErrorResponse ErrorResponse { get; set; } = null!;

        protected BaseException() : base(string.Empty)
        {
        }

        protected BaseException(ErrorResponse errorResponse)
        {
            ErrorResponse = errorResponse;
        }
    }
}
