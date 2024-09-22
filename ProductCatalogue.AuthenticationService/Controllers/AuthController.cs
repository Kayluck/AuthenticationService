using Microsoft.AspNetCore.Mvc;
using ProductCatalogue.AuthenticationService.DTOs;
using ProductCatalogue.AuthenticationService.Interfaces;
using ProductCatalogue.AuthenticationService.Utilities;
using Swashbuckle.AspNetCore.Annotations;

namespace ProductCatalogue.AuthenticationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _authService;

        public AuthController(ILogger<AuthController> logger, IAuthService authService)
        {
            _logger = logger;
            _authService = authService;
        }

        [HttpPost("SignUp")]
        [SwaggerOperation("User sign up", "Sign up a user on Product Catalogue Application")]
        public async Task<ActionResult<GenericResponse<SignUpResponse>>> Signup(SignUpRequest signRequest)
        {
            try
            {
                GenericResponse<SignUpResponse> signupResponse =
                              await _authService.SignUp(signRequest);

                if (signupResponse.StatusCode == CommonResponseCodes.Success)
                    return Ok(signupResponse);

                return BadRequest(signupResponse);
            }
            catch (Exception e)
            {
                _logger.LogError("An error occurred while signing up {e}", e.Message);

                return StatusCode(500, new GenericResponse(CommonResponseCodes.Failure, "An error occurred while signing up"));
            }
        }

        [HttpPost("Login")]
        [SwaggerOperation("User login", "Login to Product Catalogue Application")]
        public async Task<ActionResult<GenericResponse<LoginResponse>>> Login(LoginRequestDto loginRequest)
        {
            try
            {
                GenericResponse<LoginResponse> loginResponse =
                              await _authService.Login(loginRequest);

                if (loginResponse.StatusCode == CommonResponseCodes.Success)
                    return Ok(loginResponse);

                return BadRequest(loginResponse);
            }
            catch (Exception e)
            {
                _logger.LogError("An error occurred while logging in {e}", e.Message);

                return StatusCode(500, new GenericResponse(CommonResponseCodes.Failure, "An error occurred while logging in"));
            }
        }
    }
}
