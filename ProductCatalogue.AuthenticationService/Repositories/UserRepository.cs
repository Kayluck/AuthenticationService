using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductCatalogue.AuthenticationService.Data;
using ProductCatalogue.AuthenticationService.DTOs;
using ProductCatalogue.AuthenticationService.Entities;
using ProductCatalogue.AuthenticationService.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace ProductCatalogue.AuthenticationService.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<ApplicationDbContext> _logger;

        public UserRepository(ApplicationDbContext dbContext, IMapper mapper, ILogger<ApplicationDbContext> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public bool CreateUser(CreateUserDto createUserDto)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                var user = _mapper.Map<User>(createUserDto);

                _dbContext.Users.Add(user);
                _dbContext.SaveChanges(); 

                var userSecret = _mapper.Map<UserSecret>(createUserDto);
                userSecret.UserId = user.Id;

                _dbContext.UserSecrets.Add(userSecret);
                _dbContext.SaveChanges();

                transaction.Commit();

                return true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError($"An error occurred while creating user with email {createUserDto.Email}", ex.Message);
                return false;
            }
        }

        public UserDto AuthenticateUser(LoginRequestDto loginRequestDto)
        {
            var user =  _dbContext.Users
                .FirstOrDefault(u => u.Username == loginRequestDto.EmailOrUsername || u.Email == loginRequestDto.EmailOrUsername);

            if (user == null)
            {
                return null;
            }

            var userSecret = _dbContext.UserSecrets.SingleOrDefault(us => us.UserId == user.Id);

            if (userSecret == null)
            {
                return null;
            }

            if (!VerifyPassword(loginRequestDto.Password, userSecret.Password, userSecret.PasswordKey))
            {
                return null;
            }

            var userDto = _mapper.Map<UserDto>(user);

            return userDto;
        }

        private bool VerifyPassword(string password, byte[] storedPassword, byte[] key)
        {
            using (var hmac = new HMACSHA512(key))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(storedPassword);
            }
        }

        public bool IsUserExisting(string email)
        {
            return _dbContext.Users.Where(x => x.Email == email && x.IsActive).Any();
        }
    }
}
