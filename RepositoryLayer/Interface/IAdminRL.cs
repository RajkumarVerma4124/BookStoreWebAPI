using CommonLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interface
{
    public interface IAdminRL
    {
        AdminResponse AdminLogin(AdminModel adminLogin);
    }
}
