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

        private readonly IConfiguration configuration;

        private readonly IAuthService _authService;
        public UserService(IConfiguration configuration, IAuthService authService, UserRepository userRepository) : base(configuration)
        {
            _authService = authService;
            _userRepository = userRepository;
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
                    Password = passwordHash,
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
