using System;
using System.Collections.Generic;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Web;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Configuration;

namespace WebApplication1.Models
{
    public class Authentication
    {
        public static string GenerateToken()
        {
            // Hardcoded values
            var key = "C1CF487DC4C4175B6618DE4F55CA4fgsdfgsdgdsf";
            var issuer = "https://localhost:44335";
            var audience = "SecureApiUser";
            int expireDays = 30;

            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(JwtRegisteredClaimNames.Sub, "your-subject-here")
                // Add other claims as needed
            }),
                Expires = DateTime.UtcNow.AddDays(expireDays),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}