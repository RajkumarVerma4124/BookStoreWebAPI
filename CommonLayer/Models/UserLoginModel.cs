using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace CommonLayer.Models
{
    /// <summary>
    /// Created The Model Class Of UserModel 
    /// </summary>
    public class UserLoginModel
    {
        //Checking The Pattern For Email And Giving Required Annotations For EmailId property
        [Required(ErrorMessage = "{0} should not be empty")]
        [RegularExpression(@"^[a-zA-Z0-9]{3,}([._+-][0-9a-zA-Z]{2,})*@[0-9a-zA-Z]+[.]?([a-zA-Z]{2})+[.]([a-zA-Z]{3})+$", ErrorMessage = "Email id is not valid")]
        [DataType(DataType.EmailAddress)]
        public string EmailId { get; set; }

        //Checking The Pattern For Password And Giving Required Annotations For Password Property
        [Required(ErrorMessage = "{0} should not be empty")]
        [RegularExpression(@"(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*[\W]).{8,}$", ErrorMessage = "Passsword is not valid")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
