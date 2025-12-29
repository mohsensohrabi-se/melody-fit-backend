using MelodyFit.Domain.Common;


namespace MelodyFit.Domain.Users.ValueObjects
{
    public sealed class UserRole : ValueObject
    {
        public static readonly UserRole User = new("User");
        public static readonly UserRole Admin = new("Admin");
        public string Name { get; }

        private UserRole(string name)
        {
            Name = name;
        }
        public static Result<UserRole> From(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Result.Failure<UserRole>("Value is required");
            return value switch
            {
                "User" => Result.Success(User),
                "Admin" => Result.Success(Admin),
                _ => Result.Failure<UserRole>("Invalid role")
            };

        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Name;
        }
    }
}
