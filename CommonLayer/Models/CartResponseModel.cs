using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CommonLayer.Models
{
    /// <summary>
    /// Created The Model For Cart Response
    /// </summary>
    public class CartResponseModel
    {
        public int CartId { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public int BookQuantity { get; set; }
        public string BookName { get; set; }
        public string AuthorName { get; set; }
        public double DiscountPrice { get; set; }
        public double ActualPrice { get; set; }
        public string BookImage { get; set; }

    }
}
