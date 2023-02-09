using CRD.Interfaces;
using CRD.Repository;
using CRD.Utils;
using log4net;
using System.Security.Cryptography;

namespace CRD.Services
{
    public class UserService : BaseService, IUserService
    {
        protected readonly UserRepository _userRepository;

        private static readonly ILog log = LogManager.GetLogger("Rolling", nameof(UserService));
        private readonly IAuthService _authService;
        public UserService(IConfiguration configuration, 
            IAuthService authService, 
             UserRepository userRepository) : base(configuration)
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

                log.Error(e);
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
                log.Error(e);
                throw;
            }
        }

        public async Task<GenericResponse<int>> Register(UserRequestDto request)
        {
            try
            {

                if (request.Password.Length < 8 )
                {
                    return new GenericResponse<int>(status: Enums.StatusCode.PASSWORD_SHOULD_HAVE_AT_LEAST_8_CHARACTERS);
                }

                if (request.Password.Contains(" "))
                {
                    return new GenericResponse<int>(status: Enums.StatusCode.PASSWORD_SHOULD_NOT_CONTAIN_SPACE);
                }

                if(!request.Password.Any(char.IsLower) || !request.Password.Any(char.IsUpper))
                {
                    return new GenericResponse<int>(status: Enums.StatusCode.PASSWORD_SHOULD_CONTAIN_AT_LEAST_ONE_LOWER_AND_ONE_UPPER_CASE);
                }

                string passwordHash = request.Password.GetHash<SHA256>();

                request.Password = passwordHash;

                using (var tw = GetTransactionWrapper())
                {
                    var registeredUserID = await _userRepository.Register(request, tw);

                    tw.Transaction.Commit();

                    if (registeredUserID == 0)
                    {
                        return new GenericResponse<int>(status: Enums.StatusCode.REGISTRATION_ERROR);
                    }
                    
                    return new GenericResponse<int>(status: Enums.StatusCode.SUCCESS, registeredUserID);
                }

            }
            catch (Exception e)
            {
                log.Error(e);
                throw;
            }
        }
    }
}
