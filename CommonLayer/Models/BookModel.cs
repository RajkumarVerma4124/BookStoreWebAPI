using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.Models
{
    public class BookModel
    {
        public int BookId { get; set; }
        public string BookName { get; set; }
        public string AuthorName { get; set; }
        public double DiscountPrice { get; set; }
        public double ActualPrice { get; set; }
        public double TotalRating { get; set; }
        public long RatingCount { get; set; }
        public string BookImage { get; set; }
        public int BookQuantity { get; set; }
        public string BookDetails { get; set; }

    }
}
