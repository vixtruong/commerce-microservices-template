namespace Commerce.BuildingBlocks.Domain.Results
{
    public sealed record Error(string Code, string Message, ErrorType Type)
    {
        public static readonly Error None = new(string.Empty, string.Empty, ErrorType.None);

        public static readonly Error NullValue = new Error(
            "General.NullValue",
            "A required value was null.",
            ErrorType.Validation);

        public static Error Validation(string code, string description) => new Error(code, description, ErrorType.Validation);

        public static Error NotFound(string code, string description) => new Error(code, description, ErrorType.NotFound);

        public static Error Conflict(string code, string description) => new Error(code, description, ErrorType.Conflict);

        public static Error Unauthorized(string code, string description) => new Error(code, description, ErrorType.Unauthorized);

        public static Error Forbidden(string code, string description) => new Error(code, description, ErrorType.Forbidden);

        public static Error Failure(string code, string description) => new Error(code, description, ErrorType.Failure);
    }
}
