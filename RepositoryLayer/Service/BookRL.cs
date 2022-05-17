using CommonLayer.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace RepositoryLayer.Service
{
    /// <summary>
    /// Created The Class For Book Repository Layer
    /// </summary>
    public class BookRL: IBookRL
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
        public BookRL(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public BookModel AddBook(AddBookModel bookModel)
        {
            try
            {
                using (sqlConnection = new SqlConnection(configuration["ConnectionString:BookStoreDB"]))
                {
                    SqlCommand command = new SqlCommand("spAddBook", sqlConnection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@BookName", bookModel.BookName);
                    command.Parameters.AddWithValue("@AuthorName", bookModel.AuthorName);
                    command.Parameters.AddWithValue("@Rating", bookModel.TotalRating);
                    command.Parameters.AddWithValue("@RatingCount", bookModel.RatingCount);
                    command.Parameters.AddWithValue("@DiscountPrice", bookModel.DiscountPrice);
                    command.Parameters.AddWithValue("@ActualPrice", bookModel.ActualPrice);
                    command.Parameters.AddWithValue("@BookImage", bookModel.BookImage);
                    command.Parameters.AddWithValue("@BookQuantity", bookModel.BookQuantity);
                    command.Parameters.AddWithValue("@BookDetails", bookModel.BookDetails);
                    command.Parameters.Add("@BookId", SqlDbType.Int).Direction = ParameterDirection.Output;
                    sqlConnection.Open();
                    int result = command.ExecuteNonQuery();
                    int bookId = Convert.ToInt32(command.Parameters["@BookId"].Value.ToString());
                    sqlConnection.Close();
                    if (result > 0)
                    {
                        BookModel bookResponse = new BookModel()
                        {
                            BookId = bookId,
                            BookName = bookModel.BookName,
                            AuthorName = bookModel.AuthorName,
                            DiscountPrice = bookModel.DiscountPrice,
                            ActualPrice = bookModel.ActualPrice,
                            BookDetails = bookModel.BookDetails,
                            TotalRating = bookModel.TotalRating,
                            RatingCount = bookModel.RatingCount,
                            BookImage = bookModel.BookImage,
                            BookQuantity = bookModel.BookQuantity,
                        };
                        return bookResponse;
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

        public BookModel UpdateBook(BookModel updatebook)
        {
            try
            {
                using (sqlConnection = new SqlConnection(configuration["ConnectionString:BookStoreDB"]))
                {
                    SqlCommand command = new SqlCommand("spUpdateBook", sqlConnection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@BookId", updatebook.BookId);
                    command.Parameters.AddWithValue("@BookName", updatebook.BookName);
                    command.Parameters.AddWithValue("@AuthorName", updatebook.AuthorName);
                    command.Parameters.AddWithValue("@Rating", updatebook.TotalRating);
                    command.Parameters.AddWithValue("@RatingCount", updatebook.RatingCount);
                    command.Parameters.AddWithValue("@DiscountPrice", updatebook.DiscountPrice);
                    command.Parameters.AddWithValue("@ActualPrice", updatebook.ActualPrice);
                    command.Parameters.AddWithValue("@BookImage", updatebook.BookImage);
                    command.Parameters.AddWithValue("@BookQuantity", updatebook.BookQuantity);
                    command.Parameters.AddWithValue("@BookDetails", updatebook.BookDetails);
                    sqlConnection.Open();
                    int result = command.ExecuteNonQuery();
                    sqlConnection.Close();
                    if (result > 0)
                    {
                        return updatebook;
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

        public string DeleteBook(int bookId)
        {
            try
            {
                using (sqlConnection = new SqlConnection(configuration["ConnectionString:BookStoreDB"]))
                {
                    SqlCommand command = new SqlCommand("spDeleteBook", sqlConnection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@BookId", bookId);
                    sqlConnection.Open();
                    int result = command.ExecuteNonQuery();
                    sqlConnection.Close();
                    if (result > 0)
                    {
                        return "Deleted Book Successfully";
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

        public BookModel GetBookById(int bookId)
        {
            try
            {
                using (sqlConnection = new SqlConnection(configuration["ConnectionString:BookStoreDB"]))
                {
                    BookModel model = new BookModel();
                    BookModel bookRes = null;
                    SqlCommand command = new SqlCommand("spGetBookById", sqlConnection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@BookId", bookId);
                    sqlConnection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        //Will Loop until rows are null
                        while (reader.Read())
                        {
                            bookRes = ReadBooksDetails(reader, model);
                        }
                        sqlConnection.Close();
                        return bookRes;
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

        public IList<BookModel> GetAllBooks()
        {
            try
            {
                using (sqlConnection = new SqlConnection(configuration["ConnectionString:BookStoreDB"]))
                {
                    BookModel bookRes = null;
                    IList<BookModel> bookList = new List<BookModel>();
                    SqlCommand command = new SqlCommand("spGetAllBook", sqlConnection);
                    command.CommandType = CommandType.StoredProcedure;
                    sqlConnection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        //Will Loop until rows are null
                        while (reader.Read())
                        {
                            BookModel model = new BookModel();
                            bookRes = ReadBooksDetails(reader, model);
                            bookList.Add(bookRes);
                        }
                        sqlConnection.Close();
                        return bookList;
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
        public BookModel ReadBooksDetails(SqlDataReader reader, BookModel bookModel)
        {
            //Storing details that are retrived
            bookModel.BookId = Convert.ToInt32(reader["BookId"] == DBNull.Value ? default : reader["BookId"]);
            bookModel.BookName = reader["BookName"] == DBNull.Value ? default : reader["BookName"].ToString();
            bookModel.AuthorName = reader["AuthorName"] == DBNull.Value ? default : reader["AuthorName"].ToString();
            bookModel.DiscountPrice = Math.Round(Convert.ToDouble(reader["DiscountPrice"] == DBNull.Value ? default : reader["DiscountPrice"]),2);
            bookModel.ActualPrice = Math.Round(Convert.ToDouble(reader["ActualPrice"] == DBNull.Value ? default : reader["ActualPrice"]),2);
            bookModel.BookDetails = reader["BookDetails"] == DBNull.Value ? default : reader["BookDetails"].ToString();
            bookModel.TotalRating = Math.Round(Convert.ToDouble(reader["Rating"] == DBNull.Value ? default : reader["Rating"]),2);
            bookModel.RatingCount = Convert.ToInt32(reader["RatingCount"] == DBNull.Value ? default : reader["RatingCount"]);
            bookModel.BookImage = reader["BookImage"] == DBNull.Value ? default : reader["BookImage"].ToString();
            bookModel.BookQuantity = Convert.ToInt32(reader["BookQuantity"] == DBNull.Value ? default : reader["BookQuantity"]);
            return bookModel;
        }
    }
}
