using CommonLayer.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace RepositoryLayer.Service
{
    /// <summary>
    /// Created The Class For Address Repository Layer
    /// </summary>
    public class AddressRL: IAddressRL
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
        public AddressRL(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// Method to add address
        /// </summary>
        /// <param name="address"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public AddressModel AddAddress(AddressModel address, int userId)
        {
            try
            {
                using (sqlConnection = new SqlConnection(configuration["ConnectionString:BookStoreDB"]))
                {
                    SqlCommand command = new SqlCommand("spAddAddress", sqlConnection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Address", address.Address);
                    command.Parameters.AddWithValue("@City", address.City);
                    command.Parameters.AddWithValue("@State", address.State);
                    command.Parameters.AddWithValue("@TypeId", address.TypeId);
                    command.Parameters.AddWithValue("@UserId", userId);
                    sqlConnection.Open();
                    int result = Convert.ToInt32(command.ExecuteScalar());
                    sqlConnection.Close();
                    if (result != 2)
                    {
                        return address;
                    }
                    else
                    {
                        return null;
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
        /// Method to delete update a address
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public string UpdateAddress(AddressResponse address)
        {
            try
            {
                using (sqlConnection = new SqlConnection(configuration["ConnectionString:BookStoreDB"]))
                {
                    SqlCommand command = new SqlCommand("spUpdateAddress", sqlConnection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@AddressId", address.AddressId);
                    command.Parameters.AddWithValue("@Address", address.Address);
                    command.Parameters.AddWithValue("@City", address.City);
                    command.Parameters.AddWithValue("@State", address.State);
                    command.Parameters.AddWithValue("@TypeId", address.TypeId);
                    sqlConnection.Open();
                    int result = Convert.ToInt32(command.ExecuteScalar());
                    sqlConnection.Close();
                    if (result != 2 && result != 1)
                    {
                        return "Address Updated successfully";
                    }
                    else
                    {
                        return null;
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
        /// Method To Delete A Address
        /// </summary>
        /// <param name="addressId"></param>
        /// <returns></returns>
        public string DeleteAddress(int addressId)
        {
            try
            {
                using (sqlConnection = new SqlConnection(configuration["ConnectionString:BookStoreDB"]))
                {
                    SqlCommand command = new SqlCommand("spDeleteAddress", sqlConnection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@AddressId", addressId);
                    sqlConnection.Open();
                    int result = command.ExecuteNonQuery();
                    sqlConnection.Close();
                    if (result > 0)
                    {
                        return "Address Deleted Successfully";
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
        /// Method To Get Address Details
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public IList<AddressResponse> GetAddressDetails(int userId, int typeId)
        {
            try
            {
                using (sqlConnection = new SqlConnection(configuration["ConnectionString:BookStoreDB"]))
                {
                    AddressResponse addrRes = null;
                    IList<AddressResponse> addrList = new List<AddressResponse>();
                    SqlCommand command = new SqlCommand("spGetAddress", sqlConnection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@TypeId", typeId);
                    sqlConnection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        //Will Loop until rows are null
                        while (reader.Read())
                        {
                            AddressResponse addrModel = new AddressResponse();
                            addrRes = ReadCartDetails(reader, addrModel);
                            addrList.Add(addrRes);
                        }
                        sqlConnection.Close();
                        return addrList;
                    }
                    else
                        return default;
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
        /// Method To Get All Address Details
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IList<AddressResponse> GetAllAddressDetails(int userId)
        {
            try
            {
                using (sqlConnection = new SqlConnection(configuration["ConnectionString:BookStoreDB"]))
                {
                    AddressResponse addrRes = null;
                    IList<AddressResponse> addrList = new List<AddressResponse>();
                    SqlCommand command = new SqlCommand("spGetAllAddress", sqlConnection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserId", userId);
                    sqlConnection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        //Will Loop until rows are null
                        while (reader.Read())
                        {
                            AddressResponse addrModel = new AddressResponse();
                            addrRes = ReadCartDetails(reader, addrModel);
                            addrList.Add(addrRes);
                        }
                        sqlConnection.Close();
                        return addrList;
                    }
                    else
                        return default;
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
        /// Method to take values from db using sql data reader to model object
        /// </summary>
        /// <returns></returns>
        public AddressResponse ReadCartDetails(SqlDataReader reader, AddressResponse addrModel)
        {
            //Storing details that are retrived
            addrModel.AddressId = Convert.ToInt32(reader["AddressId"] == DBNull.Value ? default : reader["AddressId"]);
            addrModel.Address = reader["Address"] == DBNull.Value ? default : reader["Address"].ToString();
            addrModel.City = reader["City"] == DBNull.Value ? default : reader["City"].ToString();
            addrModel.State = reader["State"] == DBNull.Value ? default : reader["State"].ToString();
            addrModel.TypeId = Convert.ToInt32(reader["TypeId"] == DBNull.Value ? default : reader["TypeId"]);
            return addrModel;
        }
    }
}

