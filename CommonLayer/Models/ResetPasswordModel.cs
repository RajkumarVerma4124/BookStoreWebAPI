using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CommonLayer.Models
{
    /// <summary>
    /// Created The Model For Resetting The Password
    /// </summary>
    public class ResetPasswordModel
    {
        //Checking The Pattern For Password And Giving Required Annotations For Password Property
        [Required(ErrorMessage = "{0} should not be empty")]
        [RegularExpression(@"(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*[\W]).{8,}$", ErrorMessage = "Passsword is not valid")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        //Checking The Pattern For Password And Giving Required Annotations For Password Property
        [Required(ErrorMessage = "{0} should not be empty")]
        [RegularExpression(@"(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*[\W]).{8,}$", ErrorMessage = "Passsword is not valid")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
