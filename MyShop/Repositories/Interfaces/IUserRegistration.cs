using MyShop.Models;

namespace MyShop.Repositories.Interfaces
{
    public interface IUserRegistration
    {

        public Task<int> InsertUser(UserModel model);
        public Task<UserModel> ValidateUser(UserLoginModel user);
        //public Task<int> ChangePassword(UserPasswordChangeModel userPassword);



    }
}
