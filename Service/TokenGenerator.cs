using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StrivoLabsTest.Data.DTOs;
using StrivoLabsTest.Data.DTOs.Services;
using StrivoLabsTest.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StrivoLabsTest.Service
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly JwtConfig _jwtConfig;

        public TokenGenerator(IOptions<JwtConfig> jwtOptions)
        {
            _jwtConfig = jwtOptions.Value;
        }

        public TokenResponse GenerateJwtToken(ServiceModel serviceModel)
        {
            try
            {
                var claims = new[]
                       {
                new Claim("ServiceId", serviceModel.ServiceId.ToString()),
                new Claim(ClaimTypes.Name, serviceModel.Name)
            };


                var key = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_jwtConfig.Secret)
                 );

                var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
                  );
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddMinutes(_jwtConfig.TokenValidityPeriod),
                    SigningCredentials = credentials
                };


                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var resToken = tokenHandler.WriteToken(token);
                var tokenResponse = new TokenResponse
                {
                    Token = resToken,
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddHours(_jwtConfig.TokenValidityPeriod)
                };
                return tokenResponse;
            }
            catch (Exception ex)
            {
                return new TokenResponse();
            }
           
        }
    }
}
