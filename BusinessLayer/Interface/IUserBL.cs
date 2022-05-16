using CommonLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    /// <summary>
    /// Created The Interface For User Business Layer Class
    /// </summary>
    public interface IUserBL
    {
        UserModel Register(UserModel userReg);
        UserResponse Login(UserLoginModel userLogin);
        UserResponse ForgotPassword(ForgotPasswordModel forgotUserPass);
        string ResetPassword(ResetPasswordModel resetPassword, string emailId);
    }
}
