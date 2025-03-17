using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MyDMS.Domain;
using MyDMS.Infrastructure;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;

namespace MyDMS.Application
{
    public class TokenService
    {
        private readonly IConfiguration _config;
        private readonly TokenRepository _tokenRepository;

        public TokenService(IConfiguration config, TokenRepository tokenRepository)
        {
            _config = config;
            _tokenRepository = tokenRepository;
        }

        public string GenerateToken(string username, string userId, string role)
        {
            var tokenSettings = _config.GetSection("JwtSettings");
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSettings["Key"] ?? ""));
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var claims = new Claim[] {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.UniqueName, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, role) };

            var jwtToken = new JwtSecurityToken(
                issuer: tokenSettings["Issuer"] ?? "",
                audience: tokenSettings["Audience"] ?? "",
                claims: claims,
                signingCredentials: credentials,
                expires: DateTime.Now.AddMinutes(60));
            Console.WriteLine(tokenSettings["ExpirationTime"]);
            Console.WriteLine(tokenSettings["Issuer"]);
            Console.WriteLine(tokenSettings["Audience"]);
            Console.WriteLine(tokenSettings["Key"]);
            Console.WriteLine(role);
            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }

        public string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }

        public void StoreRefreshToken(string userId, string token)
        {
            RefreshToken refreshToken = new RefreshToken
            {
                UserId = userId,
                Token = token,
                ExpiryDate = DateTime.Now.AddMinutes(30)
            };
            _tokenRepository.AddToken(refreshToken);
        }
    }
}
