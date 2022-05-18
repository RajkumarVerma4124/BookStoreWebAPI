using BusinessLayer.Interface;
using CommonLayer.Models;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;


namespace BusinessLayer.Service
{
    /// <summary>
    /// Created The Feedback Business Layer Class To Implement IAddressBL Methods
    /// </summary>
    public class FeedbackBL: IFeedbackBL
    {
        /// <summary>
        /// Reference Object For Interface IBookRL
        /// </summary>
        private readonly IFeedbackRL feedbackRL;

        /// <summary>
        /// Created Constructor With Dependency Injection For IFeedbackRL
        /// </summary>
        /// <param name="addressRL"></param>
        public FeedbackBL(IFeedbackRL feedbackRL)
        {
            this.feedbackRL = feedbackRL;
        }

        public FeedbackModel AddFeedback(FeedbackModel feedback, int userId)
        {
            try
            {
                return feedbackRL.AddFeedback(feedback, userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IList<FeedbackResponse> GetAllFeedbackDetails(int bookId)
        {
            try
            {
                return feedbackRL.GetAllFeedbackDetails(bookId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
