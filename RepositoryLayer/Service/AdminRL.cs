using CommonLayer.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RepositoryLayer.Service
{
    /// <summary>
    /// Created The Class For Admin Register Repository Layer
    /// </summary>
    public class AdminRL : IAdminRL
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
        public AdminRL(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// Method to login admin by checking existing db table with admin credentials
        /// </summary>
        /// <param name="adminLogin"></param>
        /// <returns></returns>
        public AdminResponse AdminLogin(AdminModel adminLogin)
        {
            try
            {
                using (sqlConnection = new SqlConnection(configuration["ConnectionString:BookStoreDB"]))
                {
                    AdminResponse adminloginResponse = new AdminResponse();
                    if (string.IsNullOrEmpty(adminLogin.EmailId) || string.IsNullOrEmpty(adminLogin.Password))
                        return null;
                    else
                    {
                        SqlCommand command = new SqlCommand("spLoginAdmin", sqlConnection);
                        //Setting command type to stored procedure
                        command.CommandType = CommandType.StoredProcedure;
                        //Add parameters to stored procedures
                        command.Parameters.AddWithValue("@Email", adminLogin.EmailId);
                        command.Parameters.AddWithValue("@Password", adminLogin.Password);
                        //Open Sql Connection
                        sqlConnection.Open();
                        //Returns object of result set
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            //Will Loop until rows are null
                            while (reader.Read())
                            {
                                adminloginResponse.AdminId = Convert.ToInt32(reader["AdminId"] == DBNull.Value ? default : reader["AdminId"]);
                                adminloginResponse.Email = reader["Email"] == DBNull.Value ? default : reader["Email"].ToString();
                                adminloginResponse.FullName = reader["FullName"] == DBNull.Value ? default : reader["FullName"].ToString();
                                adminloginResponse.MobileNumber = reader["MobileNumber"] == DBNull.Value ? default : reader["MobileNumber"].ToString(); ;
                                adminloginResponse.Address = reader["Address"] == DBNull.Value ? default : reader["Address"].ToString(); ;
                                adminloginResponse.MobileNumber = reader["MobileNumber"] == DBNull.Value ? default : reader["MobileNumber"].ToString(); ;
                            }
                            sqlConnection.Close();
                            adminloginResponse.Token = GenerateSecurityToken(adminloginResponse.Email, adminloginResponse.AdminId);
                            return adminloginResponse;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        /// <summary>
        /// Method To Generate Security Token For A Admin
        /// </summary>
        /// <param name="emailId"></param>
        /// <param name="emailId"></param>
        /// <returns></returns>
        private string GenerateSecurityToken(string emailId, long adminId)
        {
            try
            {
                //Genearting A Json Web Token For Authorization
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var claims = new[]
                {
                    new Claim(ClaimTypes.Role, "Admin"),
                    new Claim(ClaimTypes.Email, emailId),
                    new Claim("AdminId", adminId.ToString())
                };
                var token = new JwtSecurityToken(
                  issuer: configuration["Jwt:Issuer"],
                  audience: configuration["Jwt:Audience"],
                  claims,
                  expires: DateTime.Now.AddHours(24),
                  signingCredentials: credentials
                );
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
