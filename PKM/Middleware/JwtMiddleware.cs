using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using PKM.DataAccess;

namespace PKM.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        
        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, UsersDbContext usersDbContext)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null)
            {
                AttachToContext(token, context, usersDbContext);
            }
            await _next(context);
        }

        private void AttachToContext(string token, HttpContext context, UsersDbContext usersDbContext)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("c96831fbac602345d6a9eaf5509c4b00")),
                ClockSkew = TimeSpan.Zero,
                ValidateIssuer = false,
                ValidateAudience = false
            }, out var validatedToken); 
            
            var jwtSecurityToken = (JwtSecurityToken) validatedToken;
            var userId = jwtSecurityToken.Claims.First(c => c.Type == "uuid").Value;

            context.Items["User"] = usersDbContext.Users.First(user => user.Guid.ToString() == userId);
        }
    }
}