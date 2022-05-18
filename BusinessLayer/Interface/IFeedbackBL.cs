using CommonLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    public interface IFeedbackBL
    {
        FeedbackModel AddFeedback(FeedbackModel feedback, int userId);
        IList<FeedbackResponse> GetAllFeedbackDetails(int bookId);
    }
}
