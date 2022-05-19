using CommonLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    public interface IAdminBL
    {
        AdminResponse AdminLogin(AdminModel adminLogin);
    }
}
