using MyDMS.Domain;
using MyDMS.Infrastructure.Data;

namespace MyDMS.Infrastructure
{
    public class TokenRepository : ITokenRepository
    {
        private readonly ApplicationDbContext _context;
        public TokenRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public RefreshToken GetToken(string userID)
        {
            return _context.Tokens.FirstOrDefault(t => t.UserId == userID);
        }

        public void AddToken(RefreshToken token)
        {
            _context.Tokens.Add(token);
            _context.SaveChanges();
        }

        public void RemoveToken(string userID)
        { 
            _context.Tokens.Remove(GetToken(userID));
        }

        public void UpdateToken(string userID, string refreshToken)
        {
            var token = GetToken(userID);
            token.Token = refreshToken;
            token.ExpiryDate = DateTime.Now.AddMinutes(30);
            _context.SaveChanges();
        }
    }
}
