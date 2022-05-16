using CommonLayer.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Service
{
    /// <summary>
    /// Created The Class For User Register Repository Layer
    /// </summary>
    public class UserRL
    {
        /// <summary>
        /// Reference Object For Sqlconnection and Iconfiguartion
        /// </summary>
        private SqlConnection sqlConnection;
        private readonly IConfiguration configuration;

        /// <summary>
        /// Reference Object For IConfiguaration
        /// </summary>
        /// <param name="configuration"></param>
        public UserRL(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
    }
}
