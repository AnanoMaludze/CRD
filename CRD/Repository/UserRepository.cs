using CRD.Utils;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace CRD.Repository
{
    public class UserRepository : BaseRepository
    {
        public UserRepository(IConfiguration configuration) : base(configuration)
        {

        }

        public async Task<int> Register(UserRequestDto model, TransactionWrapper tw)
        {
            var userID = await tw.Connection.QueryFirstAsync<int>(@"
                                                            declare @ID int;
                                                            INSERT INTO [dbo].[User]
                                                            ([Name]
                                                           ,[Surname]
                                                           ,[Username]
                                                           ,[PasswordHash]
                                                           ,[IdentityNumber]
                                                           ,[BirthDate])
                                                        VALUES
                                                            (@Name
                                                            ,@Surname
                                                            ,@Username
                                                            ,@PasswordHash
                                                            ,@IdentityNumber
                                                            ,@BirthDate
                                                            )
                                                            set @ID = SCOPE_IDENTITY();
                                                            select @ID;
                                                        ", new
            {
                model.Name,
                model.Surname,
                model.Username,
                @PasswordHash = model.Password,
                model.IdentityNumber,
                model.BirthDate

            }, transaction: tw.Transaction);


            return userID;
        }

        public async Task<User> GetUserByUserame(string username, TransactionWrapper tw)
        {
            var user = await Connection.QueryFirstOrDefaultAsync<User>(@"select * from dbo.[User] where Username = @userName",
                new
                {
                    username = username,
                });

            return user;
        }

        public async Task<User> GetUserByID(int userID, TransactionWrapper tw)
        {
            var user = await tw.Connection.QueryFirstOrDefaultAsync<User>(@"select * from dbo.[User] where ID = @ID",
                new
                {
                    ID = userID,
                }, transaction: tw.Transaction);

            return user;
        }
    }
}
