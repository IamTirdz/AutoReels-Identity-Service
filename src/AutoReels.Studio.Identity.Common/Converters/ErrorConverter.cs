using Microsoft.AspNetCore.Identity;

namespace AutoReels.Studio.Identity.Common.Converters
{
    public static class ErrorConverter
    {
        public static IDictionary<string, string[]> ToErrors(this IdentityResult result) =>
            result.Errors
                .GroupBy(e => e.Code)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.Description).ToArray()
                );
    }
}
