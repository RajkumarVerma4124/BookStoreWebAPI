using CommonLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interface
{
    /// <summary>
    /// Created The Interface For Feedback Repository layer
    /// </summary>
    public interface IFeedbackRL
    {
        FeedbackModel AddFeedback(FeedbackModel feedback, int userId);
        IList<FeedbackResponse> GetAllFeedbackDetails(int bookId);
    }
}
