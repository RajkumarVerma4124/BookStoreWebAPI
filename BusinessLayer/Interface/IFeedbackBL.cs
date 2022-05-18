using CommonLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    /// <summary>
    /// Created The Interface For Feedback Business Layer 
    /// </summary>
    public interface IFeedbackBL
    {
        FeedbackModel AddFeedback(FeedbackModel feedback, int userId);
        IList<FeedbackResponse> GetAllFeedbackDetails(int bookId);
    }
}
