namespace AutoReels.Studio.Identity.Common.Constants
{
    public static class ApiScope
    {
        public const string Read = $"{ApiResource.Module}:read";
        public const string Write = $"{ApiResource.Module}:write";
        public const string Update = $"{ApiResource.Module}:update";
        public const string Delete = $"{ApiResource.Module}:delete";
        public const string UpdatePassword = $"{ApiResource.Module}:update_password";
    }
}
