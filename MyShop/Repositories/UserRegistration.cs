using Dapper;
using MyShop.Context;
using MyShop.Models;
using MyShop.Repositories.Interfaces;
using System.Collections.Generic;
using System.Reflection;
using System;

namespace MyShop.Repositories
{
    public class UserRegistration:IUserRegistration
    {
        private readonly DbContext _context;
        
        public UserRegistration(DbContext context)
        {
            _context = context;
        }
        public async Task<int> InsertUser(UserModel model)
        {

            var query = @" DECLARE @salt UNIQUEIDENTIFIER=NEWID();
                           DECLARE @idd UNIQUEIDENTIFIER=NEWID();
                               insert into tblUsers(id,userTypeid,userName,dateOfBirth,gender,emailId,PasswordHash,salt,mobile,
                               createdBy,createdDate,modifiedBy,modifiedDate,isDeleted)
                               values (@idd,0,@userName,@dateOfBirth,@gender,@emailId,HASHBYTES('sha2_512',@password+cast(@salt as varchar(100))),@salt,@mobile,
                               @idd,GETDATE(),@idd,GETDATE(),'false')";
            //int result = 0;
            using (var connection = _context.CreateConnection())
            {
               
                var result = await connection.ExecuteAsync(query, model);
             
                return result;
            }
            
        }

        public async Task<UserModel> ValidateUser(UserLoginModel user)
        {
            try
            {
                int result = 0;
                UserModel user1 = new UserModel();
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();
                    user1 = await connection.QueryFirstOrDefaultAsync<UserModel>(@" 
                                 Select * from tblUsers where userName=@UserName 
                                        and PasswordHash =HASHBYTES('SHA2_512', (@Password + convert(nvarchar(max),salt)))", user);

                    if (user1 == null)
                        return user1;
                    else
                        return user1;

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
