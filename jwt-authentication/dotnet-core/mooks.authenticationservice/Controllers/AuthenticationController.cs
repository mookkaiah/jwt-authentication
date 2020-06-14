namespace mooks.authenticationservice.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using mooks.authenticationservice.contracts.requests;
    using mooks.authenticationservice.contracts.response;
    using mooks.authenticationservice.Domain.Identity;
    using mooks.authenticationservice.Services;

    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public AuthenticationController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUser request)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(new RegistrationResult {
                    Success = false,
                    Messages = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                });
            }

            var newUser = new User
            {
                Email = request.Email,
                Password = request.Password
            };

            var registerResponse =  await _identityService.RegisterAsync(newUser);
            return Ok(registerResponse);
        }

        [HttpPost("updateuser")]
        public async Task<IActionResult> UpdateUser([FromBody] RegisterUser request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new RegistrationResult
                {
                    Success = false,
                    Messages = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                });
            }

            var updateUser = new User
            {
                Email = request.Email,
                Password = request.Password
            };

            var updateResponse = await _identityService.UpdateUser(updateUser);
            return Ok(updateResponse);
        }

        [HttpPost("changepassword")]
        public async Task<IActionResult> ChangePassword([FromBody] RegisterUser request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new RegistrationResult
                {
                    Success = false,
                    Messages = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                });
            }

            var updateUser = new User
            {
                Email = request.Email,
                Password = request.Password
            };

            var changePasswordResponse = await _identityService.ChangePassword(updateUser);
            return Ok(changePasswordResponse);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUser request)
        {
           if (!ModelState.IsValid)
            {
                return BadRequest(new LoginResult
                {
                    Success = false,
                    Messages = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                });
            }

            var newUser = new User
            {
                Email = request.Email,
                Password = request.Password
            };

            var loginResponse = await _identityService.LoginAsync(newUser);

            return Ok(loginResponse);
        }
    }
}