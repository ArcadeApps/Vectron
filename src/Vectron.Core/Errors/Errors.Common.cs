namespace Vectron.Core;

public static partial class Errors
{
    public static class Common
    {
        public static Error Unknown => new("M_UNKNOWN", "");

        public static Error Forbidden => new("M_FORBIDDEN", "");
        public static Error InvalidToken => new("M_UNKNOWN_TOKEN", "");
        public static Error MissingToken => new("M_MISSING_TOKEN", "");
        public static Error LockedUser => new("M_USER_LOCKED", "");

        public static Error BadJson => new("M_BAD_JSON", "");
        public static Error NotJson => new("M_NOT_JSON", "");

        public static Error NotFound => new("M_NOT_FOUND", "");
        public static Error Unrecognized => new("M_UNRECOGNIZED", "");
        public static Error RateLimited => new("M_LIMIT_EXCEEDED", "");
    }
}