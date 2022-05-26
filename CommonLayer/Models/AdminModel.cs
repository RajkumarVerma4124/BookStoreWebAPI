using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CommonLayer.Models
{
    public class AdminModel
    {
        [Required]
        public string EmailId { get; set; }
        [Required]
        public string Password { get; set; }

    }
}
