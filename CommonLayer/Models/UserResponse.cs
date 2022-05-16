using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.Models
{
    /// <summary>
    /// Created The Model For User Response
    /// </summary>
    public class UserResponse
    {
        public int UserId { get; set; }
        public string EmailId { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public long MobileNumber { get; set; }
        public string Token { get; set; }
    }
}
