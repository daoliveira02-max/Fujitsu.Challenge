using Fujitsu.Challenge.API.Interfaces;
using Fujitsu.Challenge.API.Models.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Fujitsu.Challenge.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            var result = _authService.Login(request);
            
            if (!result.IsSuccess) 
                return Unauthorized(result.FailureReason);

            return Ok(result);
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterRequest request)
        {
            var result = _authService.Register(request);

            if (!result.IsSuccess) 
                return BadRequest(result.FailureReason);

            return Ok(result);
        }
    }
}
