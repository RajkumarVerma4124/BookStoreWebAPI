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
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        /// <summary>
        /// Object Reference For Interface IAdminBL,ILogger
        /// </summary>
        private readonly IAdminBL adminBL;
        private readonly ILogger<AdminController> logger;


        /// <summary>
        /// Constructor To Initialize The Instance Of Interface IAdminBL,ILogger
        /// </summary>
        /// <param name="adminBL"></param>
        /// <param name="logger"></param>
        public AdminController(IAdminBL adminBL, ILogger<AdminController> logger)
        {
            this.adminBL = adminBL;
            this.logger = logger;
        }

        /// <summary>
        /// Post Request For Login Admin (POST: /api/admin/login)
        /// </summary>
        /// <param name="adminLogin"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public IActionResult Login(AdminModel adminLogin)
        {
            try
            {
                var resAdmin = adminBL.AdminLogin(adminLogin);
                if (resAdmin != null)
                {
                    logger.LogInformation("Admin Login Successfull");
                    return Ok(new { success = true, message = "Admin Login Successfull", data = resAdmin });
                }
                else
                {
                    logger.LogWarning("Admin Login Failed Check EmailId And Password");
                    return BadRequest(new { success = false, message = "Admin Login Failed Check EmailId And Password" });
                }
            }
            catch (Exception)
            {
                logger.LogError("Admin Login Failed Check EmailId And Password");
                return BadRequest(new { success = false, message = "Admin Login Failed Check EmailId And Password" });
            }
        }
    }
}
