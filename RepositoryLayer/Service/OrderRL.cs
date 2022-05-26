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
    /// Created The Class For Order Repository Layer
    /// </summary>
    public class OrderRL : IOrderRL
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
        public OrderRL(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// Method to add order
        /// </summary>
        /// <param name="order"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IList<string> AddOrder(OrderModel order, int userId)
        {
            using (sqlConnection = new SqlConnection(configuration["ConnectionString:BookStoreDB"]))
            {
                //Open the connection
                sqlConnection.Open();
                //Start a local transactions
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                //Enlist a command int the current transaction
                SqlCommand command = sqlConnection.CreateCommand();
                //Setting the command to transaction
                command.Transaction = sqlTransaction;
                try
                {
                    IList<CartResponseModel> cartList = new List<CartResponseModel>();
                    IList<string> orderSuccess = new List<string>();
                    command = new SqlCommand("sp_GetCartDetails", sqlConnection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Transaction = sqlTransaction;
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        //Will Loop until rows are null
                        while (reader.Read())
                        {
                            CartResponseModel model = new CartResponseModel();
                            model.UserId = Convert.ToInt32(reader["UserId"] == DBNull.Value ? default : reader["UserId"]);
                            model.BookId = Convert.ToInt32(reader["BookId"] == DBNull.Value ? default : reader["BookId"]);
                            cartList.Add(model);
                        }
                        //Closing the reader
                        reader.Close();
                        foreach (var cart in cartList)
                        {
                            command = new SqlCommand("spAddOrders", sqlConnection);
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@AddressId", order.AddressId);
                            command.Parameters.AddWithValue("@BookId", cart.BookId);
                            command.Parameters.AddWithValue("@UserId", cart.UserId);
                            command.Transaction = sqlTransaction;
                            int result = Convert.ToInt32(command.ExecuteScalar());
                            if (result != 2 && result != 1 && result != 3)
                            {
                                orderSuccess.Add("Order Purchased Succesfully");
                            }
                            else
                            {
                                sqlTransaction.Rollback();
                                return null;
                            }
                        }
                        sqlTransaction.Commit();
                        sqlConnection.Close();
                        return orderSuccess;
                    }
                    else
                        return null;
                }
                catch (Exception ex)
                {
                    sqlTransaction.Rollback();
                    throw ex;
                }
                finally
                {
                    sqlConnection.Close();
                }
            }
        }

        /// <summary>
        /// Method To Get All Order Details
        /// </summary>
        /// <returns></returns>
        public IList<OrderResponse> GetAllOrderDetails(int userId)
        {
            try
            {
                using (sqlConnection = new SqlConnection(configuration["ConnectionString:BookStoreDB"]))
                {
                    OrderResponse orderRes = null;
                    IList<OrderResponse> ordersList = new List<OrderResponse>();
                    SqlCommand command = new SqlCommand("spGetAllOrders", sqlConnection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserId", userId);
                    sqlConnection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        //Will Loop until rows are null
                        while (reader.Read())
                        {
                            OrderResponse feedbackModel = new OrderResponse();
                            orderRes = ReadOrderDetails(reader, feedbackModel);
                            ordersList.Add(orderRes);
                        }
                        sqlConnection.Close();
                        return ordersList;
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
        public OrderResponse ReadOrderDetails(SqlDataReader reader, OrderResponse orderModel)
        {
            //Storing details that are retrived
            orderModel.OrderId = Convert.ToInt32(reader["OrderId"] == DBNull.Value ? default : reader["OrderId"]);
            orderModel.UserId = Convert.ToInt32(reader["UserId"] == DBNull.Value ? default : reader["UserId"]);
            orderModel.BookId = Convert.ToInt32(reader["BookId"] == DBNull.Value ? default : reader["BookId"]);
            orderModel.AddressId = Convert.ToInt32(reader["AddressId"] == DBNull.Value ? default : reader["AddressId"]);
            orderModel.BookName = reader["BookName"] == DBNull.Value ? default : reader["BookName"].ToString();
            orderModel.AuthorName = reader["AuthorName"] == DBNull.Value ? default : reader["AuthorName"].ToString();
            orderModel.OrderDateTime = Convert.ToDateTime(reader["OrderDate"] == DBNull.Value ? default : reader["OrderDate"].ToString());
            orderModel.OrderDate = orderModel.OrderDateTime.ToString("MMMM dd");
            orderModel.BookImage = reader["BookImage"] == DBNull.Value ? default : reader["BookImage"].ToString();
            orderModel.BookQuantity = Convert.ToInt32(reader["BookQuantity"] == DBNull.Value ? default : reader["BookQuantity"]);
            orderModel.OrderTotalPrice = Math.Round(Convert.ToDouble(reader["OrderTotalPrice"] == DBNull.Value ? default : reader["OrderTotalPrice"]), 2);
            orderModel.ActualTotalPrice = Math.Round(Convert.ToDouble(reader["ActualTotalPrice"] == DBNull.Value ? default : reader["ActualTotalPrice"]), 2);
            return orderModel;
        }
    }
}
