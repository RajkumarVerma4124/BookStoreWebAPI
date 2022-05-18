using BusinessLayer.Interface;
using CommonLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace BookStoreAPI.Controllers
{
    /// <summary>
    /// Created The User Controller For Http Requests And Response 👈 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        /// <summary>
        /// Object Reference For Interface IUserBL
        /// </summary>
        private readonly IUserBL userBL;

        /// <summary>
        /// Constructor To Initialize The Instance Of Interface IUserBL
        /// </summary>
        /// <param name="userBL"></param>
        public UserController(IUserBL userBL)
        {
            this.userBL = userBL;
        }

        /// <summary>
        /// Post Request For Registering A New User (POST: /api/user/register)
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        [HttpPost("Register")]
        public IActionResult Register(UserModel userModel)
        {
            try
            {
                var resUser = userBL.Register(userModel);
                if (resUser != null)
                {
                    return Created("User Added Successfully", new { success = true, data = resUser });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Email Already Exists" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Post Request For Login Existing User (POST: /api/user/login)
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public IActionResult Login(UserLoginModel userLogin)
        {
            try
            {
                var resUser = userBL.Login(userLogin);
                if (resUser != null)
                {
                    return Ok(new { success = true, message = "Login Successfully", Email = resUser.EmailId, FullName = resUser.FullName, MobileNum = resUser.MobileNumber, token = resUser.Token });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Login Failed Check EmailId And Password" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Post Request For Forgot Password Existing User (POST: /api/user/forgotpassword)
        /// </summary>
        /// <param name="emailId"></param>
        /// <returns></returns>
        [HttpPost("ForgotPassword")]
        public IActionResult ForgotPassword(ForgotPasswordModel forgotPass)
        {
            try
            {
                var resUser = userBL.ForgotPassword(forgotPass);
                if (resUser != null)
                {
                    return Ok(new { success = true, message = "Reset Link Sent Successfully" });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Entered Email Id Isn't Registered" });
                }
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Patch Request For Resetting Password For Existing User (PUT: /api/user/resetpassword)
        /// </summary>
        /// <param name="resetPassword"></param>
        /// <returns></returns>
        [HttpPatch("ResetPassword")]
        [Authorize]  //👈 For Authorized User Only
        public IActionResult ResetPassword(ResetPasswordModel resetPassword)
        {
            try
            {
                //Getting THe Email Of Authorized User Using Claims Of Jwt
                var emailId = User.FindFirst(ClaimTypes.Email).Value;
                var resMessage = userBL.ResetPassword(resetPassword, emailId);
                if (!resMessage.Contains("Not") || !resMessage.Contains("Failed"))
                {
                    return Ok(new { success = true, message = resMessage });
                }
                else
                {
                    return BadRequest(new { success = false, message = resMessage });
                }
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }
    }
}
