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
    /// Created The Class For Cart Repository Layer
    /// </summary>
    public class CartRL: ICartRL
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
        public CartRL(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// Method To Add Book To Cart
        /// </summary>
        /// <param name="cartModel"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public AddCartModel AddBookToCart(AddCartModel cartModel, int userId)
        {
            try
            {
                using (sqlConnection = new SqlConnection(configuration["ConnectionString:BookStoreDB"]))
                {
                    SqlCommand command = new SqlCommand("spAddBookToCart", sqlConnection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@BookQuantity", cartModel.BookQuantity);
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@BookId", cartModel.BookId);                
                    sqlConnection.Open();
                    int result = command.ExecuteNonQuery();
                    sqlConnection.Close();
                    if (result > 0)
                    {
                        return cartModel;
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
        /// Method To Delete A Cart
        /// </summary>
        /// <param name="cartId"></param>
        /// <returns></returns>
        public string DeleteCart(int cartId, int userId)
        {
            try
            {
                using (sqlConnection = new SqlConnection(configuration["ConnectionString:BookStoreDB"]))
                {
                    SqlCommand command = new SqlCommand("spDeleteCart", sqlConnection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CartId", cartId);
                    command.Parameters.AddWithValue("@UserId", userId);
                    sqlConnection.Open();
                    int result = command.ExecuteNonQuery();
                    sqlConnection.Close();
                    if (result > 0)
                    {
                        return "Cart Deleted Successfully";
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
        /// Method To Update The Cart
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="bookquantity"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string UpdateBook(int cartId, int bookquantity, int userId)
        {
            try
            {
                using (sqlConnection = new SqlConnection(configuration["ConnectionString:BookStoreDB"]))
                {
                    SqlCommand command = new SqlCommand("spUpdateCart", sqlConnection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CartId", cartId);
                    command.Parameters.AddWithValue("@BookQuantity", bookquantity);
                    command.Parameters.AddWithValue("@UserId", userId);
                    sqlConnection.Open();
                    int result = command.ExecuteNonQuery();
                    sqlConnection.Close();
                    if (result > 0)
                    {
                        return "Updated The Data Succesfully";
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
        /// Method To Get All Cart Details
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IList<CartResponseModel> GetAllCartDetails(int userId)
        {
            try
            {
                using (sqlConnection = new SqlConnection(configuration["ConnectionString:BookStoreDB"]))
                {
                    CartResponseModel cartRes = null;
                    IList<CartResponseModel> cartList = new List<CartResponseModel>();
                    SqlCommand command = new SqlCommand("sp_GetCartDetails", sqlConnection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserId", userId);
                    sqlConnection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        //Will Loop until rows are null
                        while (reader.Read())
                        {
                            CartResponseModel model = new CartResponseModel();
                            cartRes = ReadCartDetails(reader, model);
                            cartList.Add(cartRes);
                        }
                        sqlConnection.Close();
                        return cartList;
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
        /// Method to take values from db using sql data reader to model object
        /// </summary>
        /// <returns></returns>
        public CartResponseModel ReadCartDetails(SqlDataReader reader, CartResponseModel cartModel)
        {
            //Storing details that are retrived
            cartModel.CartId = Convert.ToInt32(reader["CartId"] == DBNull.Value ? default : reader["CartId"]);
            cartModel.UserId = Convert.ToInt32(reader["UserId"] == DBNull.Value ? default : reader["UserId"]);
            cartModel.BookId = Convert.ToInt32(reader["BookId"] == DBNull.Value ? default : reader["BookId"]);
            cartModel.BookQuantity = Convert.ToInt32(reader["BookQuantity"] == DBNull.Value ? default : reader["BookQuantity"]);
            cartModel.BookName = reader["BookName"] == DBNull.Value ? default : reader["BookName"].ToString();
            cartModel.AuthorName = reader["AuthorName"] == DBNull.Value ? default : reader["AuthorName"].ToString();
            cartModel.DiscountPrice = Math.Round(Convert.ToDouble(reader["DiscountPrice"] == DBNull.Value ? default : reader["DiscountPrice"]), 2);
            cartModel.ActualPrice = Math.Round(Convert.ToDouble(reader["ActualPrice"] == DBNull.Value ? default : reader["ActualPrice"]), 2);
            cartModel.BookImage = reader["BookImage"] == DBNull.Value ? default : reader["BookImage"].ToString();
            return cartModel;
        }
    }
}
