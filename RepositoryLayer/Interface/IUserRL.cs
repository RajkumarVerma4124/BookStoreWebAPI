using CommonLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interface
{
    /// <summary>
    /// Created The User Interface For User Repository Layer Class
    /// </summary>
    public interface IUserRL
    {
        UserModel Register(UserModel userReg);
        UserResponse Login(UserLoginModel userLogin);
        UserResponse ForgotPassword(ForgotPasswordModel forgotUserPass);
        string ResetPassword(ResetPasswordModel resetPassword, string emailId);
    }
}
