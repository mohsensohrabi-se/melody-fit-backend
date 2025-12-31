namespace MelodyFit.API.Contracts.Auth
{
    public sealed class LoginUserRequest
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
