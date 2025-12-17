using MelodyFit.Domain.Common;
using MelodyFit.Domain.Users.Events;
using MelodyFit.Domain.Users.ValueObjects;


namespace MelodyFit.Domain.Users.Aggregates
{
    public sealed class User : AggregateRoot
    {
        private User() { }
        public Email Email { get; private set; } = null!;
        public string PasswordHash { get; private set; } = null!;
        public UserProfile Profile { get; private set; }= null!;
        public DateTime CreatedAt { get; private set; }

        private User(
            Email email,
            string passwordHash,
            UserProfile profile
            )
        {
            Email = email;
            PasswordHash = passwordHash;
            Profile = profile;
            CreatedAt = DateTime.UtcNow;

            AddDomainEvent(new UserRegisteredEvent(Id));
        }

        public static Result<User> Register(
            string email,
            string passwordHash,
            UserProfile profile
            )
        {
            var EmailResult = Email.Create(email);
            if (!EmailResult.IsSuccess)
                return Result.Failure<User>(EmailResult.Error);

            if (profile is null)
                return Result.Failure<User>("Profile is required");

            if(string.IsNullOrWhiteSpace(passwordHash))
                return Result.Failure<User>("PasswordHash is required");

            var user = new User(EmailResult.Value, passwordHash, profile);
            return Result.Success<User>(user);
        }

        public Result UpdateProfile(UserProfile newProfile)
        {
            if (newProfile is null)
                return Result.Failure("Profile can not be null");
            Profile = newProfile;
            return Result.Success();
        }
    }
}
