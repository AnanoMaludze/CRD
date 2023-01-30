using CRD.Models;
using CRD.Services.AuthService;
using CRD.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace CRD.Services.UserService
{
    public class UserService : IUserService
    {
        public static User user = new User();
        private readonly IAuthService _authService;
        public UserService(IAuthService authService)
        {
            this._authService = authService;
        }

        private static List<User> users = new List<User>
            {
                new User
                { UserId = 1,
                    Name = "name",
                    Surname = "surname",
                    IdentityNumber = "0101010101",
                    Username= "username",
                    BirthDate = DateTime.Now,
                    PasswordHash = "asdasd"
                },
                 new User
                { UserId = 2,
                    Name = "iron man",
                    Surname = "secondSurname",
                    IdentityNumber = "01010121201",
                    Username= "secondusername",
                    BirthDate = DateTime.Now,
                    PasswordHash = "blablabla"
                },
            };


        public List<User> GetAllUsers()
        {
            return users;
        }

        public User GetUserByID(int userID)
        {
            var user = users.Find(x => x.UserId == userID);

            if (user is null)
            {
                return null;
            }

            return user;
        }

        public User Login(UserLoginRequestDto request)
        {
            string passwordHash = StringUtils.GetHash<SHA256>(request.Password);

            if (!user.Username.Equals(request.Username))
            {
                return null;
            }

            if (!request.Password.Equals(passwordHash))
            {
                return null;
            }

            string token = _authService.CreateToken(new CreateTokenModel
            {
                Password = passwordHash,
                Username = request.Username,
                UserID = request.IdentityNumber
            });

            return new User { };
        }

        public User Register(UserRequestDto request)
        {
            string passwordHash = StringUtils.GetHash<SHA256>(request.Password);

            user.Name = request.Name;
            user.Surname = request.Surname;
            user.IdentityNumber = request.IdentityNumber;
            user.Username = request.Username;
            user.BirthDate = request.BirthDate;
            user.PasswordHash = passwordHash;

            users.Add(user);

            return user;
        }
    }
}
