using Microsoft.AspNetCore.Mvc;
using MovieAPI.Services;

namespace MovieAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService; 
        }
        

        /// <summary>
        /// This end-poing for Register new User
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("register")]

        public async Task<IActionResult> RegisterAsync([FromBody] RegisterModel model)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var result =await _authService.RegisterAsync(model);

            if(!result.IsAuthenticated) 
                return BadRequest(result.Message);

            return Ok(result);
            
        }

        /// <summary>
        /// This end-point for retrieving the token for user by giving Email@Password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("token")]

        public async Task<IActionResult> GetTokenAsync([FromBody] TokenRequestModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _authService.GetTokenAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);

        }

        /// <summary>
        /// This end-point for assign specific role to User
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("addrole")]

        public async Task<IActionResult> AddRoleAsync([FromBody] AddRoleModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _authService.AddRoleAsync(model);

            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return Ok(model);

        }

    }
}
