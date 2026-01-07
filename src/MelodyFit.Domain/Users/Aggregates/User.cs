using MelodyFit.Domain.Common;
using MelodyFit.Domain.Users.Entities;
using MelodyFit.Domain.Users.Events;
using MelodyFit.Domain.Users.ValueObjects;
using MelodyFit.Domain.Workouts.Events;


namespace MelodyFit.Domain.Users.Aggregates
{
    public sealed class User : AggregateRoot
    {
        private User() { }
        public Email Email { get; private set; } = null!;
        public string PasswordHash { get; private set; } = null!;
        public UserProfile Profile { get; private set; }= null!;
        public UserRole Role { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private PersonalRecords _personalRecords = null!;
        public PersonalRecords PersonalRecords => _personalRecords;

        //Refresh tokens
        private readonly List<RefreshToken> _refreshTokens = new();
        public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();

        private User(
            Email email,
            string passwordHash,
            UserProfile profile
            )
        {
            Email = email;
            PasswordHash = passwordHash;
            Profile = profile;
            Role = UserRole.User;
            CreatedAt = DateTime.UtcNow;

            _personalRecords = PersonalRecords.Create();

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

        public Result UpdatePersonalRecords(WorkoutSummary summary)
        {
            if (summary is null)
                return Result.Failure("Workout summary is required");

            var isNewRecord = _personalRecords.TryUpdate(summary);

            if (isNewRecord)
            {
                AddDomainEvent(new PersonalRecordUpdatedEvent(
                    Id,
                    summary.Date
                ));
            }

            return Result.Success();
        }

        public Result PromoteAdmin()
        {
            if (Role == UserRole.Admin)
                return Result.Failure("User is already an admin");
            Role = UserRole.Admin;
            return Result.Success();
        }


        public Result UpdateProfile(UserProfile newProfile)
        {
            if (newProfile is null)
                return Result.Failure("Profile can not be null");
            Profile = newProfile;
            return Result.Success();
        }

        // Methods related to refresh token
        public Result AddRefreshToken(RefreshToken token)
        {
            if (token is null)
                return Result.Failure("Refresh token can not be null");
            _refreshTokens.Add(token);
            return Result.Success();
        }

        public RefreshToken? GetRefreshToken(string tokenHash)
        {
            return _refreshTokens.SingleOrDefault(t => t.TokenHash == tokenHash);
        }

        public Result RotateRefreshToken(
            string currentTokenHash,
            RefreshToken newToken,
            string? ipAddress)
        {
            var existing = GetRefreshToken(currentTokenHash);
            if (existing is null)
                return Result.Failure("Refresh token not found");
            if (!existing.IsActive)
                return Result.Failure("Refresh token is not active");
            existing.RevokeAndRotation(newToken.Id, ipAddress);
            _refreshTokens.Add(newToken);
            return Result.Success();
        }

        // Logout -- revoke single token
        public Result RevokeRefreshToken(string tokenHash, string? ipAddress = null)
        {
            var token = GetRefreshToken(tokenHash);
            if (token is null)
                return Result.Failure("Refresh token not found");
            token.Revoke(ipAddress);
            return Result.Success();
        }

        public Result RevokeAllRefreshTokens(string? ipAddress = null)
        {
            foreach(var token in _refreshTokens.Where(t => t.IsActive))
            {
                token.Revoke(ipAddress);
            }
            return Result.Success();
        }

    }
}
