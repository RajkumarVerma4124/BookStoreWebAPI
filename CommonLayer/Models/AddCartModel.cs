using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CommonLayer.Models
{
    /// <summary>
    /// Created The Model To Add Cart Details
    /// </summary>
    public class AddCartModel
    {
        [Required]
        public int BookId { get; set; }
        public int BookQuantity { get; set; }
    }
}

