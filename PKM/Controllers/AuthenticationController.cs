using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PKM.Model;
using PKM.Service;

namespace PKM.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        /// <summary>
        /// Login endpoint
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Login([FromBody] UserView body)
        {
            var token = _authenticationService.Login(body.Login, body.Password);
            
            return !token.IsNullOrEmpty() ? Ok($"Bearer {token}") : Unauthorized();
        }
    }
}