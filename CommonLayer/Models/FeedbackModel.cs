using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.Models
{
    /// <summary>
    /// Created The Model Class For Feedback
    /// </summary>
    public class FeedbackModel
    {
        public int BookId { get; set; }
        public string Comment { get; set; }
        public string Rating { get; set; }
    }
}
