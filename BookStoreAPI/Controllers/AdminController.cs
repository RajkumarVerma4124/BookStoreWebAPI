using BusinessLayer.Interface;
using CommonLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace BookStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        /// <summary>
        /// Object Reference For Interface IAdminBL
        /// </summary>
        private readonly IAdminBL adminBL;

        /// <summary>
        /// Constructor To Initialize The Instance Of Interface IAdminBL
        /// </summary>
        /// <param name="adminBL"></param>
        public AdminController(IAdminBL adminBL)
        {
            this.adminBL = adminBL;
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
                    return Ok(new { success = true, message = "Admin Login Successfully", data = resAdmin });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Admin Login Failed Check EmailId And Password" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}
