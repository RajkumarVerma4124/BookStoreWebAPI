using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CommonLayer.Models
{
    /// <summary>
    /// Created The Model For Forgot Password
    /// </summary>
    public class ForgotPasswordModel
    {
        //Checking The Pattern For Email And Giving Required Annotations For EmailId property
        [Required(ErrorMessage = "{0} should not be empty")]
        [RegularExpression(@"^[a-zA-Z0-9]{3,}([._+-][0-9a-zA-Z]{2,})*@[0-9a-zA-Z]+[.]?([a-zA-Z]{2})+[.]([a-zA-Z]{3})+$", ErrorMessage = "Email id is not valid")]
        [DataType(DataType.EmailAddress)]
        public string EmailId { get; set; }
    }
}
