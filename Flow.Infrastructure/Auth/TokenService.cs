using Flow.Domain.User;
using Flow.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Flow.Infrastructure.Auth
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;
        private readonly FlowDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public TokenService(IConfiguration configuration, FlowDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _configuration = configuration;
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<(string AccessToken, string RefreshToken)> GenerateTokensAsync(ApplicationUser user)
        {
            var jwtConfig = _configuration.GetSection("JwtConfig");

            var secret = jwtConfig["Key"]
                ?? throw new InvalidOperationException("JWT Key missing in configuration.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };


            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var tokenValidityMins = int.TryParse(jwtConfig["TokenValidityMins"], out var mins)
                ? mins
                : 30;

            var expires = DateTime.UtcNow.AddMinutes(tokenValidityMins);

            var token = new JwtSecurityToken(
                issuer: jwtConfig["Issuer"],
                audience: jwtConfig["Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

            var refreshEntity = new RefreshToken
            {
                Token = refreshToken,
                ExpiresAt = DateTimeOffset.UtcNow.AddDays(7),  
                CreatedAt = DateTimeOffset.UtcNow,
                UserId = user.Id
            };

            _dbContext.RefreshTokens.Add(refreshEntity);
            await _dbContext.SaveChangesAsync();

            return (accessToken, refreshToken);
        }

    }
}
