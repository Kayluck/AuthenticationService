using ProductCatalogue.AuthenticationService.DTOs;

namespace ProductCatalogue.AuthenticationService.Interfaces
{
    public interface IAuthService
    {
        Task<GenericResponse<LoginResponse>> Login(LoginRequestDto loginRequestDto);
        Task<GenericResponse<SignUpResponse>> SignUp(SignUpRequest signRequest);
    }
}
