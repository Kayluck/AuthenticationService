using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using ProductCatalogue.AuthenticationService.DTOs;
using ProductCatalogue.AuthenticationService.Interfaces;
using ProductCatalogue.AuthenticationService.Utilities;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ProductCatalogue.AuthenticationService.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<AuthService> _logger;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, ILogger<AuthService> logger, IMapper mapper, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _logger = logger;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<GenericResponse<SignUpResponse>> SignUp(SignUpRequest signRequest)
        {
            try
            {
                GenericResponse<SignUpResponse> response = new("", "", null);

                _logger.LogInformation($"Sign up request initiated for user with email {signRequest.Email}. Full request: {signRequest}");

                if (_userRepository.IsUserExisting(signRequest.Email))
                {
                    _logger.LogError($"User sign up failed - User with email {signRequest.Email} already exists");

                    return new GenericResponse<SignUpResponse>(CommonResponseCodes.Failure, $"User with email {signRequest.Email} already exists", null);
                }

                byte[] passwordHash, passwordKey;

                using (var hmac = new HMACSHA512())
                {
                    passwordKey = hmac.Key;
                    passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(signRequest.Password));
                }

                CreateUserDto createUserDto = _mapper.Map<CreateUserDto>(signRequest);
                createUserDto.Username = $"{signRequest.Firstname}.{signRequest.Lastname}";
                createUserDto.Password = passwordHash;
                createUserDto.PasswordKey = passwordKey;

                bool userCreated = _userRepository.CreateUser(createUserDto);

                if (userCreated)
                {
                    response.Data = _mapper.Map<SignUpResponse>(createUserDto);

                    _logger.LogInformation($"User sign up successful {response.Data}");

                    return response = new GenericResponse<SignUpResponse>(CommonResponseCodes.Success, "Sign up successful", response.Data);
                }

                _logger.LogError($"User sign up with request: {signRequest} failed");

                return response = new GenericResponse<SignUpResponse>(CommonResponseCodes.Failure, "User sign up failed", null);
            }
            catch (Exception e)
            {
                _logger.LogError($"An error occured while signing up user: {signRequest}", e.Message, e);
                throw;
            }
        }

        public async Task<GenericResponse<LoginResponse>> Login(LoginRequestDto loginRequestDto)
        {
            try
            {
                GenericResponse<LoginResponse> response = new("", "", null);

                UserDto user = _userRepository.AuthenticateUser(loginRequestDto);

                if (user == null)
                {
                    _logger.LogError("Invalid login credentials");
                    return response = new GenericResponse<LoginResponse>(CommonResponseCodes.Failure, "Invalid login credentials", null);
                }

                var token = GenerateToken(user);

                LoginResponse loginResponse = _mapper.Map<LoginResponse>(user);
                loginResponse.Token = token;

                _logger.LogInformation($"Login successful for user: {user}");

                return response = new GenericResponse<LoginResponse>(CommonResponseCodes.Success, "Login successful", loginResponse);
            }
            catch (Exception e)
            {
                _logger.LogError("An error occurred while logging in user {e}", e.Message);
                throw;
            }
        }


        private string GenerateToken(UserDto user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Firstname + user.Lastname),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Username),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
