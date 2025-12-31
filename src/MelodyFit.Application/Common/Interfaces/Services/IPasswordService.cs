
namespace MelodyFit.Application.Common.Interfaces.Services
{
    public interface IPasswordService
    {
        Task<string> HashPassword(string password);
        Task<bool> Verify(string password, string hash);
    }
}
