using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using PKM.DataAccess;

namespace PKM.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UsersDbContext _dbContext;

        public AuthenticationService(UsersDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public string Login(string login, string password)
        {
            var userCredentials = _dbContext.UserCredentials.FirstOrDefault(c =>
                c.Login == login &&
                c.Password == BitConverter.ToString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(password)))
            );
            if (userCredentials == null) return null;
            
            const string mySecurityKey = "c96831fbac602345d6a9eaf5509c4b00";
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new("uuid", userCredentials.UserId.ToString()),
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(mySecurityKey)), SecurityAlgorithms.HmacSha256Signature)
            };

            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }
    }
}