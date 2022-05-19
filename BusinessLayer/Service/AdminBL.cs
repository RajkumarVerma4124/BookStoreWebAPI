using BusinessLayer.Interface;
using CommonLayer.Models;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Service
{
    /// <summary>
    /// Created The Admin Business Layer Class To Implement IAdminBL Methods
    /// </summary>
    public class AdminBL: IAdminBL
    {
        /// <summary>
        /// Reference Object For Interface IAdminRL
        /// </summary>
        private readonly IAdminRL adminRL;

        /// <summary>
        /// Created Constructor With Dependency Injection For IAdminRL
        /// </summary>
        /// <param name="adminRL"></param>
        public AdminBL(IAdminRL adminRL)
        {
            this.adminRL = adminRL;
        }

        public AdminResponse AdminLogin(AdminModel adminLogin)
        {
            try
            {
                return adminRL.AdminLogin(adminLogin);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
