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
    /// Created The Class For User Register Repository Layer
    /// </summary>
    public class UserRL : IUserRL
    {
        /// <summary>
        /// Reference Object For Sqlconnection and Iconfiguartion
        /// </summary>
        private SqlConnection sqlConnection;
        private readonly IConfiguration configuration;


        /// <summary>
        /// Declare And Intialize A Value For The Secret Key
        /// </summary>
        private static string Key = "47c53aa7571c33d2f98d02a4313c4ba1ea15e45c18794eb564b21c19591805ff";

        /// <summary>
        /// Reference Object For IConfiguaration
        /// </summary>
        /// <param name="configuration"></param>
        public UserRL(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// Method To Add Users To The Database Table Using Sql
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public UserModel Register(UserModel userReg)
        {
            try
            {
                using (sqlConnection = new SqlConnection(configuration["ConnectionString:BookStoreDB"]))
                {
                    SqlCommand command = new SqlCommand("spUserRegistation", sqlConnection);
                    //Setting command type to stored procedure
                    command.CommandType = CommandType.StoredProcedure;
                    //Adding parameters to stored procedures
                    command.Parameters.AddWithValue("@FullName", userReg.FullName);
                    command.Parameters.AddWithValue("@EmailId", userReg.EmailId);
                    command.Parameters.AddWithValue("@Password", PasswordEncrypt(userReg.Password));
                    command.Parameters.AddWithValue("@MobileNum", userReg.MobileNumber);
                    sqlConnection.Open();
                    int result = command.ExecuteNonQuery();
                    sqlConnection.Close();
                    if (result > 0)
                        return userReg;
                    else
                        return null;
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
        /// Method to login user by checking existing db table with user credentials
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        public UserResponse Login(UserLoginModel userLogin)
        {
            try
            {
                using (sqlConnection = new SqlConnection(configuration["ConnectionString:BookStoreDB"]))
                {
                    UserResponse loginResponse = new UserResponse();
                    if (string.IsNullOrEmpty(userLogin.EmailId) || string.IsNullOrEmpty(userLogin.Password))
                        return null;
                    else
                    {
                        SqlCommand command = new SqlCommand("spUserLogin", sqlConnection);
                        //Setting command type to stored procedure
                        command.CommandType = CommandType.StoredProcedure;
                        //Add parameters to stored procedures
                        command.Parameters.AddWithValue("@EmailId", userLogin.EmailId);
                        //Open Sql Connection
                        sqlConnection.Open();
                        //Returns object of result set
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            int userId = 0;
                            //Will Loop until rows are null
                            while (reader.Read())
                            {
                                loginResponse.EmailId = reader["EmailId"] == DBNull.Value ? default : reader["EmailId"].ToString();
                                loginResponse.FullName = reader["FullName"] == DBNull.Value ? default : reader["FullName"].ToString();
                                loginResponse.Password = reader["Password"] == DBNull.Value ? default : reader["Password"].ToString();
                                loginResponse.MobileNumber = Convert.ToInt64(reader["MobileNumber"] == DBNull.Value ? default : reader["MobileNumber"]);
                                userId = Convert.ToInt32(reader["UserId"] == DBNull.Value ? default : reader["UserId"]);
                            }
                            sqlConnection.Close();
                            string decryptPass = PasswordDecrypt(loginResponse.Password);
                            if (decryptPass == userLogin.Password)
                            {
                                loginResponse.Token = GenerateSecurityToken(loginResponse.EmailId, userId);
                                return loginResponse;
                            }
                            else
                                return null;
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
        /// Method To Send Token To The Registered Emailid For The User Who Forgots The Password
        /// </summary>
        /// <param name="emailId"></param>
        /// <returns></returns>
        public UserResponse ForgotPassword(ForgotPasswordModel forgotUserPass)
        {
            try
            {
                UserResponse userDetails = new UserResponse();
                using (sqlConnection = new SqlConnection(configuration["ConnectionString:BookStoreDB"]))
                {
                    SqlCommand command = new SqlCommand("spForgotPassword", sqlConnection);
                    //Setting command type to stored procedure
                    command.CommandType = CommandType.StoredProcedure;
                    //Add parameters to stored procedures
                    command.Parameters.AddWithValue("@EmailId", forgotUserPass.EmailId);
                    //Open Sql Connection
                    sqlConnection.Open();
                    //Returns object of result set
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        int userId = 0;
                        //Will Loop until rows are null
                        while (reader.Read())
                        {
                            userDetails.FullName = reader["FullName"] == DBNull.Value ? default : reader["FullName"].ToString();
                            userDetails.EmailId = reader["EmailId"] == DBNull.Value ? default : reader["EmailId"].ToString();
                            userId = Convert.ToInt32(reader["UserId"] == DBNull.Value ? default : reader["UserId"]);
                        }
                        sqlConnection.Close();
                        userDetails.Token = GenerateSecurityToken(userDetails.EmailId, userId);
                        new Msmq().SendMessage(userDetails.Token, userDetails.EmailId, userDetails.FullName);
                        return userDetails;
                    }
                    else
                        return null;
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
        /// Method To Reset The Password For Autheniticated EmailId After Token AUthorization 
        /// </summary>
        /// <param name="emailId"></param>
        /// <returns></returns>
        public string ResetPassword(ResetPasswordModel resetPassword, string emailId)
        {
            try
            {
                using (sqlConnection = new SqlConnection(configuration["ConnectionString:BookStoreDB"]))
                {
                    if (resetPassword.Password.Equals(resetPassword.ConfirmPassword))
                    {
                        SqlCommand command = new SqlCommand("spResetPassword", sqlConnection);
                        //Setting command type to stored procedure
                        command.CommandType = CommandType.StoredProcedure;
                        //Add parameters to stored procedures
                        command.Parameters.AddWithValue("@EmailId", emailId);
                        command.Parameters.AddWithValue("@confirmPassword", PasswordEncrypt(resetPassword.ConfirmPassword));
                        //Open Sql Connection
                        sqlConnection.Open();
                        //Returns object of result set
                        int result = command.ExecuteNonQuery();
                        sqlConnection.Close();
                        if (result > 0)
                        {
                            return "Resetted The Password SuccessFully";
                        }
                        else
                            return "Pasword Resetting Failed";
                    }
                    else
                        return "Password Does Not Match";
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
        /// Method To Encrypt The Password To Store Into The DB
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string PasswordEncrypt(string password)
        {
            try
            {
                if (string.IsNullOrEmpty(password))
                    return null;
                else
                {
                    password += Key;
                    var passwordBytes = Encoding.UTF8.GetBytes(password);
                    return Convert.ToBase64String(passwordBytes);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method To Decrypt The Password From The DB
        /// </summary>
        /// <param name="encodedPassword"></param>
        /// <returns></returns>
        public static string PasswordDecrypt(string encodedPassword)
        {
            try
            {
                //Decrypting the password
                if (string.IsNullOrEmpty(encodedPassword))
                    return null;
                else
                {
                    var encodedBytes = Convert.FromBase64String(encodedPassword);
                    var res = Encoding.UTF8.GetString(encodedBytes);
                    var resPass = res.Substring(0, res.Length - Key.Length);
                    return resPass;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method To Generate Security Token For A User
        /// </summary>
        /// <param name="emailId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private string GenerateSecurityToken(string emailId, long userId)
        {
            try
            {
                //Genearting A Json Web Toekn For Authorization
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var claims = new[]
                {
                    new Claim(ClaimTypes.Role, "User"),
                    new Claim(ClaimTypes.Email, emailId),
                    new Claim("UserId", userId.ToString())
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
