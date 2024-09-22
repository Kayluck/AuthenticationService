using AutoMapper;
using ProductCatalogue.AuthenticationService.DTOs;
using ProductCatalogue.AuthenticationService.Entities;

namespace ProductCatalogue.AuthenticationService.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, SignUpRequest>()
                .ReverseMap();

            CreateMap<User, CreateUserDto>()
             .ReverseMap();

            CreateMap<SignUpRequest, CreateUserDto>()
                .ForMember(dest => dest.Password, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordKey, opt => opt.Ignore())
              .ReverseMap();

            CreateMap<SignUpResponse, CreateUserDto>()
            .ReverseMap();

            CreateMap<UserDto, User>()
           .ReverseMap();

            CreateMap<UserSecret, CreateUserDto>()
            .ReverseMap();

            CreateMap <LoginResponse, UserDto>()
             .ReverseMap();
        }
    }
}
