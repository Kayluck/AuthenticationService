using ProductCatalogue.AuthenticationService.DTOs;

namespace ProductCatalogue.AuthenticationService.Interfaces
{
    public interface IUserRepository
    {
        UserDto AuthenticateUser(LoginRequestDto loginRequestDto);
        bool CreateUser(CreateUserDto createUserDto);
        bool IsUserExisting(string email);
    }
}
