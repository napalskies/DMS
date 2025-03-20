using MyDMS.Domain;

namespace MyDMS.Infrastructure
{
    public interface ITokenRepository
    {
        public RefreshToken GetToken(string userID);
        public void AddToken(RefreshToken refreshToken);
        public void RemoveToken(string userID);
        public void UpdateToken(string userID, string refreshToken);
    }
}
