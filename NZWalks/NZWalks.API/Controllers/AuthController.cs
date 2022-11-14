using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly ITokenHandler tokenHandler;

        public AuthController(IUserRepository userRepository, ITokenHandler tokenHandler)
        {
            this.userRepository = userRepository;
            this.tokenHandler = tokenHandler;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> LoginAsync(Models.DTO.LoginRequest loginRequest)
        {
            //validate incoming request

            //check user is authenticated
            //check username and password
            var user = await userRepository.AuthenticateUser(loginRequest.UserName, loginRequest.Password);
            if (user!=null)
            {
                //generate jwt token
                var token= await tokenHandler.CreateTokenAsync(user);
                return Ok(token);
            }
            return BadRequest("Username or password is incorrect.");
        }
    }
}
