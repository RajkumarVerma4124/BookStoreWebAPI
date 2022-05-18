using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CommonLayer.Models
{
    /// <summary>
    /// Created The Model Class For Order Response
    /// </summary>
    public class OrderResponse
    {
        public string BookName { get; set; }
        public string AuthorName { get; set; }
        public string BookImage { get; set; }
        public int OrderId { get; set; }
        [JsonIgnore]
        public DateTime OrderDateTime { get; set; }
        public object OrderDate { get; set; }
        public double OrderTotalPrice { get; set; }
        public double ActualTotalPrice { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public int AddressId { get; set; }
        public int BookQuantity { get; set; }

    }
}
