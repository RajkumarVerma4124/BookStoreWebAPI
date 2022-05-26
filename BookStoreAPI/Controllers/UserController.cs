using BusinessLayer.Interface;
using CommonLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        /// Object Reference For Interface IUserBL,ILogger
        /// </summary>
        private readonly IUserBL userBL;
        private readonly ILogger<UserController> logger;

        /// <summary>
        /// Constructor To Initialize The Instance Of Interface IUserBL,ILogger
        /// </summary>
        /// <param name="userBL"></param>
        /// <param name="logger"></param>
        public UserController(IUserBL userBL, ILogger<UserController> logger)
        {
            this.userBL = userBL;
            this.logger = logger;
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
                    logger.LogInformation("Registeration Successfull");
                    return Created("User Added Successfully", new { success = true, data = resUser });
                }
                else
                {
                    logger.LogWarning("The Email Already Exists");
                    return BadRequest(new { success = false, message = "Email Already Exists" });
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Registeration Unsuccessfull");
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
                    logger.LogInformation("Login Successfull");
                    return Ok(new { success = true, message = "Login Successfully", Email = resUser.EmailId, FullName = resUser.FullName, MobileNum = resUser.MobileNumber, token = resUser.Token });
                }
                else
                {
                    logger.LogWarning("Login Failed Check EmailId And Password");
                    return BadRequest(new { success = false, message = "Login Failed Check EmailId And Password" });
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Login Unsuccessfull");
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
                    logger.LogInformation("Reset Link Sent Successfully");
                    return Ok(new { success = true, message = "Reset Link Sent Successfully" });
                }
                else
                {
                    logger.LogWarning("Entered Email Id Isn't Registered");
                    return BadRequest(new { success = false, message = "Entered Email Id Isn't Registered" });
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Reset Link Failed");
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Patch Request For Resetting Password For Existing User (PUT: /api/user/resetpassword)
        /// </summary>
        /// <param name="resetPassword"></param>
        /// <returns></returns>
        [HttpPatch("ResetPassword")]
        [Authorize(Roles = Role.User)] //👈 For Authorized User Only
        public IActionResult ResetPassword(ResetPasswordModel resetPassword)
        {
            try
            {
                //Getting THe Email Of Authorized User Using Claims Of Jwt
                var emailId = User.FindFirst(ClaimTypes.Email).Value;
                var resMessage = userBL.ResetPassword(resetPassword, emailId);
                if (!resMessage.Contains("Not") || !resMessage.Contains("Failed"))
                {
                    logger.LogInformation(resMessage);
                    return Ok(new { success = true, message = resMessage });
                }
                else
                {
                    logger.LogWarning(resMessage);
                    return BadRequest(new { success = false, message = resMessage });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return NotFound(new { success = false, message = ex.Message });
            }
        }
    }
}
