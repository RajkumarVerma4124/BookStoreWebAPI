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
    /// Created The Class For WIshlist Repository Layer
    /// </summary>
    public class WishlistRL: IWishlistRL
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
        public WishlistRL(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// Method To Add Book To Wishlist
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string AddBookToWishlist(int bookId, int userId)
        {
            try
            {
                using (sqlConnection = new SqlConnection(configuration["ConnectionString:BookStoreDB"]))
                {
                    SqlCommand command = new SqlCommand("spAddWishlist", sqlConnection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@BookId", bookId);
                    sqlConnection.Open();
                    int result = Convert.ToInt32(command.ExecuteScalar());
                    sqlConnection.Close();
                    if (result == 1)
                    {
                        return "Book Already Added To Wishlist";
                    }
                    else if (result == 2)
                    {
                        return null;
                    }
                    else
                    {
                        return "Book Added To Wishlist Successfully";
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
        /// Method To Delete a Wishlist
        /// </summary>
        /// <param name="wishlistId"></param>
        /// <returns></returns>
        public string DeleteWishlist(int wishlistId)
        {
            try
            {
                using (sqlConnection = new SqlConnection(configuration["ConnectionString:BookStoreDB"]))
                {
                    SqlCommand command = new SqlCommand("spDeleteWishlist", sqlConnection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@WishlistId", wishlistId);
                    sqlConnection.Open();
                    int result = command.ExecuteNonQuery();
                    sqlConnection.Close();
                    if (result > 0)
                    {
                        return "Wishlist Deleted Successfully";
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
        /// Method to get all wishlist
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IList<WishListResponse> GetAllWishlistDetails(int userId)
        {
            try
            {
                using (sqlConnection = new SqlConnection(configuration["ConnectionString:BookStoreDB"]))
                {
                    WishListResponse wishlistRes = null;
                    IList<WishListResponse> wishlistResList = new List<WishListResponse>();
                    SqlCommand command = new SqlCommand("sp_GetAllWishlist", sqlConnection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserId", userId);
                    sqlConnection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        //Will Loop until rows are null
                        while (reader.Read())
                        {
                            WishListResponse wishlist = new WishListResponse();
                            wishlistRes = ReadWishlistDetails(reader, wishlist);
                            wishlistResList.Add(wishlistRes);
                        }
                        sqlConnection.Close();
                        return wishlistResList;
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
        public WishListResponse ReadWishlistDetails(SqlDataReader reader, WishListResponse wishlistModel)
        {
            //Storing details that are retrived
            wishlistModel.WishlistId = Convert.ToInt32(reader["WishlistId"] == DBNull.Value ? default : reader["WishlistId"]);
            wishlistModel.UserId = Convert.ToInt32(reader["UserId"] == DBNull.Value ? default : reader["UserId"]);
            wishlistModel.BookId = Convert.ToInt32(reader["BookId"] == DBNull.Value ? default : reader["BookId"]);
            wishlistModel.BookName = reader["BookName"] == DBNull.Value ? default : reader["BookName"].ToString();
            wishlistModel.AuthorName = reader["AuthorName"] == DBNull.Value ? default : reader["AuthorName"].ToString();
            wishlistModel.DiscountPrice = Math.Round(Convert.ToDouble(reader["DiscountPrice"] == DBNull.Value ? default : reader["DiscountPrice"]), 2);
            wishlistModel.ActualPrice = Math.Round(Convert.ToDouble(reader["ActualPrice"] == DBNull.Value ? default : reader["ActualPrice"]), 2);
            wishlistModel.BookImage = reader["BookImage"] == DBNull.Value ? default : reader["BookImage"].ToString();
            return wishlistModel;
        }
    }
}
