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
    /// Created The Class For Feedback Repository Layer
    /// </summary>
    public class FeedbackRL: IFeedbackRL
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
        public FeedbackRL(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// Method to add feedback
        /// </summary>
        /// <param name="feedback"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public FeedbackModel AddFeedback(FeedbackModel feedback, int userId)
        {
            try
            {
                using (sqlConnection = new SqlConnection(configuration["ConnectionString:BookStoreDB"]))
                {
                    SqlCommand command = new SqlCommand("spAddFeedback", sqlConnection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Comment", feedback.Comment);
                    command.Parameters.AddWithValue("@Rating", feedback.Rating);
                    command.Parameters.AddWithValue("@BookId", feedback.BookId);
                    command.Parameters.AddWithValue("@UserId", userId);
                    sqlConnection.Open();
                    int result = Convert.ToInt32(command.ExecuteScalar());
                    sqlConnection.Close();
                    if (result != 2 && result != 1)
                    {
                        return feedback;
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
        /// Method To Get All FeedBack Details
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        public IList<FeedbackResponse> GetAllFeedbackDetails(int bookId)
        {
            try
            {
                using (sqlConnection = new SqlConnection(configuration["ConnectionString:BookStoreDB"]))
                {
                    FeedbackResponse feedbackRes = null;
                    IList<FeedbackResponse> feedbackList = new List<FeedbackResponse>();
                    SqlCommand command = new SqlCommand("spGetALLFeedback", sqlConnection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@BookId", bookId);
                    sqlConnection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        //Will Loop until rows are null
                        while (reader.Read())
                        {
                            FeedbackResponse feedbackModel = new FeedbackResponse();
                            feedbackRes = ReadFeedbackDetails(reader, feedbackModel);
                            feedbackList.Add(feedbackRes);
                        }
                        sqlConnection.Close();
                        return feedbackList;
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
        public FeedbackResponse ReadFeedbackDetails(SqlDataReader reader, FeedbackResponse feedbackModel)
        {
            //Storing details that are retrived
            feedbackModel.FeedbackId = Convert.ToInt32(reader["FeedbackId"] == DBNull.Value ? default : reader["FeedbackId"]);
            feedbackModel.UserId = Convert.ToInt32(reader["UserId"] == DBNull.Value ? default : reader["UserId"]);
            feedbackModel.BookId = Convert.ToInt32(reader["BookId"] == DBNull.Value ? default : reader["BookId"]);
            feedbackModel.Comment = reader["Comment"] == DBNull.Value ? default : reader["Comment"].ToString();
            feedbackModel.Rating = Convert.ToInt32(reader["Rating"] == DBNull.Value ? default : reader["Rating"]);
            feedbackModel.FullName = reader["FullName"] == DBNull.Value ? default : reader["FullName"].ToString();
            return feedbackModel;
        }

    }
}
