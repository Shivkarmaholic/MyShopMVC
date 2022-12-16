using MyShop.Enums;
using System.ComponentModel.DataAnnotations;

namespace MyShop.Models
{
    public class UserModel :BaseModel
    {
        //UserTypeId,UserName,DateOfBirth,Gender,EmailId,Password,Mobile,IsTermsAccepted
        //insert into tblUsers(UserTypeId,UserName,Gender,EmailId,Password,Mobile,IsTermsAccepted)
      //  values(98,'manya','male','ffuiydsuf','5787fdg',8853545,'true')
        public int UserTypeId { get; set; } 
        public string? UserName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public GenderOptions Gender { get; set; }
        public string? EmailId { get; set; }
        public string? Password { get; set; }
        public long Mobile { get; set; }        
        public bool IsTermsAccepted { get; set; }


    }

    public class UserRegisterModel:BaseModel
    {       
        public string? UserName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public GenderOptions Gender { get; set; }
        public string? EmailId { get; set; }
        public string? Password { get; set; }
        public long Mobile { get; set; }
        public bool IsTermsAccepted { get; set; }


    }

    public class UserLoginModel:BaseModel
    {       
        public string? UserName { get; set; }
        public string? Password { get; set; }

    }
}
