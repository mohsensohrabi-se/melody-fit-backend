using MelodyFit.Domain.Common;


namespace MelodyFit.Domain.Users.Entities
{
    public sealed class RefreshToken : BaseEntity
    {
        public string TokenHash { get; private set; } = null!;
        public DateTime ExpiresAt { get; private set; }
        public DateTime CreatedAt { get; private set; }
        //Revoke
        public bool IsRevoked { get; private set; }
        public DateTime? RevokedAt { get; private set; }
        //IP
        public string? CreatedByIP { get; private set; }
        public string? RevokedByIP { get; private set; }

        public Guid? ReplacedByTokenId { get; private set; }

        // Methods
        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
        public bool IsActive => !IsRevoked && !IsExpired;

        // Constructors
        private RefreshToken() { }
        private RefreshToken(
            string tokenHash,
            DateTime expiresAt,
            DateTime createdAt,
            string? createdByIP)
        {
            TokenHash = tokenHash;
            ExpiresAt = expiresAt;
            CreatedAt = createdAt;
            CreatedByIP = createdByIP;
            IsRevoked = false;
        }

        //Factory
        public static Result<RefreshToken> Create(
            string tokenHash,
            DateTime expiresAt,
            string? createdByIP=null)
        {
            if (string.IsNullOrWhiteSpace(tokenHash))
                return  Result.Failure<RefreshToken>("Refresh token  can not be empty");
            if (expiresAt <= DateTime.UtcNow)
                return Result.Failure<RefreshToken>("Refresh token expiration must be set in the future");
            var refreshToken = new RefreshToken(
                tokenHash,
                expiresAt,
                DateTime.UtcNow,
                createdByIP);
            return Result.Success(refreshToken);
        }

        // Revoke
        public void Revoke(string? revokedByIP = null)
        {
            if (IsRevoked)
                return;
            IsRevoked = true;
            RevokedAt = DateTime.UtcNow;
            RevokedByIP = revokedByIP;
        }

        // Revoke and replace (Rotation)
        public Result RevokeAndRotation(Guid newTokenId, string? revokedByIP = null)
        {
            if (IsRevoked) return Result.Failure("Can not revoke an expired token");
            Revoke(revokedByIP);
            ReplacedByTokenId = newTokenId;
            return Result.Success();
        }

    }
}
