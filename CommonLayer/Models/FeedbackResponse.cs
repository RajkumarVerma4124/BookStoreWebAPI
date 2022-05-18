using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.Models
{
    /// <summary>
    /// Created The Model Class For FeedbackResponse
    /// </summary>
    public class FeedbackResponse
    {
        public int FeedbackId { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
        public string FullName { get; set; }
    }
}
