using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.Models
{
    /// <summary>
    /// Creating the model to add books
    /// </summary>
    public class AddBookModel
    {
        public string BookName { get; set; }
        public string AuthorName { get; set; }
        public float DiscountPrice { get; set; }
        public float ActualPrice { get; set; }
        public float TotalRating { get; set; }
        public long RatingCount { get; set; }
        public string BookImage { get; set; }
        public int BookQuantity { get; set; }
        public string BookDetails { get; set; }
    }
}
