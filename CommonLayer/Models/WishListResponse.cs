using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.Models
{
    /// <summary>
    /// Created The Model For Wishlist Response
    /// </summary>
    public class WishListResponse
    {
        public int WishlistId { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public string BookName { get; set; }
        public string AuthorName { get; set; }
        public double DiscountPrice { get; set; }
        public double ActualPrice { get; set; }
        public string BookImage { get; set; }

    }
}
