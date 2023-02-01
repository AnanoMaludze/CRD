using CRD.Interfaces;
using CRD.Models;
using CRD.Repository;
using CRD.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;

namespace CRD.Services
{
    public class UserService : BaseService, IUserService
    {
        protected readonly UserRepository _userRepository;

        private readonly IHttpContextAccessor _httpContentAccessor;

        private readonly IConfiguration configuration;

        private readonly IAuthService _authService;
        public UserService(IConfiguration configuration, 
            IAuthService authService, 
            IHttpContextAccessor httpContentAccessor, UserRepository userRepository) : base(configuration)
        {
            _authService = authService;
            _userRepository = userRepository;
            _httpContentAccessor = httpContentAccessor;
        }

        public string GetUserID()
        {
            var result = string.Empty;

            if (_httpContentAccessor.HttpContext != null)
            {
                result = _httpContentAccessor.HttpContext.User?.Identity?.Name;
            }
            return result;
        }

        public async Task<GenericResponse<User>> GetUserByID(int userID)
        {
            try
            {
                var tw = GetTransactionWrapperWithoutTransaction();

                var user = await _userRepository.GetUserByID(userID, tw);

                if (user == null)
                {
                    return new GenericResponse<User>(status: Enums.StatusCode.USER_DOES_NOT_EXIST);
                }
                return new GenericResponse<User>(status: Enums.StatusCode.SUCCESS, user);
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public async Task<GenericResponse<User>> Login(UserLoginRequestDto request)
        {
            try
            {
                string passwordHash = request.Password.GetHash<SHA256>();

                var tw = GetTransactionWrapperWithoutTransaction();

                var user = await _userRepository.GetUserByUserame(request.Username, tw);

                if (user == null)
                {
                    return new GenericResponse<User>(status: Enums.StatusCode.USER_DOES_NOT_EXIST);
                }
                if (!user.PasswordHash.Equals(passwordHash))
                {
                    return new GenericResponse<User>(status: Enums.StatusCode.INCORRECT_USER_CREDENTIALS);
                }

                string token = _authService.CreateToken(new CreateTokenModel
                {
                    Username = request.Username,
                    UserID = user.ID
                });

                return new GenericResponse<User>(status: Enums.StatusCode.SUCCESS, user);
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public async Task<GenericResponse<User>> Register(UserRequestDto request)
        {
            try
            {

                if (request.Password.Length < 8 )
                {
                    return new GenericResponse<User>(status: Enums.StatusCode.PASSWORD_SHOULD_HAVE_AT_LEAST_8_CHARACTERS);
                   
                }

                if (request.Password.Contains(" "))
                {
                    return new GenericResponse<User>(status: Enums.StatusCode.PASSWORD_SHOULD_NOT_CONTAIN_SPACE);

                }
                if(!request.Password.Any(char.IsLower) || !request.Password.Any(char.IsUpper))
                {
                    return new GenericResponse<User>(status: Enums.StatusCode.PASSWORD_SHOULD_CONTAIN_AT_LEAST_ONE_LOWER_AND_ONE_UPPER_CASE);

                }

                string passwordHash = request.Password.GetHash<SHA256>();

                request.Password = passwordHash;

                using (var tw = GetTransactionWrapper())
                {
                    var registeredUserID = await _userRepository.Register(request, tw);

                    var user = await _userRepository.GetUserByID(registeredUserID, tw);

                    if (user == null)
                    {
                        return new GenericResponse<User>(status: Enums.StatusCode.REGISTRATION_ERROR);
                    }
                    tw.Transaction.Commit();
                    return new GenericResponse<User>(status: Enums.StatusCode.SUCCESS, user);
                }

            }
            catch (Exception e)
            {

                throw;
            }
        }
    }
}
