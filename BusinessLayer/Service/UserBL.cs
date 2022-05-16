using BusinessLayer.Interface;
using CommonLayer.Models;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Service
{
    /// <summary>
    /// Created The User Business Layer Class To Implement IUserBL Methods
    /// </summary>
    public class UserBL : IUserBL
    {
        /// <summary>
        /// Reference Object For Interface IUserRL
        /// </summary>
        private readonly IUserRL userRL;

        /// <summary>
        /// Created Constructor With Dependency Injection For IUSerRL
        /// </summary>
        /// <param name="userRL"></param>
        public UserBL(IUserRL userRL)
        {
            this.userRL = userRL;
        }

        public UserResponse ForgotPassword(ForgotPasswordModel forgotUserPass)
        {
            try
            {
                return userRL.ForgotPassword(forgotUserPass);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public UserResponse Login(UserLoginModel userLogin)
        {
            try
            {
                return userRL.Login(userLogin);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public UserModel Register(UserModel userReg)
        {
            try
            {
                return userRL.Register(userReg);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public string ResetPassword(ResetPasswordModel resetPassword, string emailId)
        {
            try
            {
                return userRL.ResetPassword(resetPassword, emailId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
