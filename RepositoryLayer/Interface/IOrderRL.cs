using CommonLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interface
{
    /// <summary>
    /// Created The Interface For Order Repository layer
    /// </summary>
    public interface IOrderRL
    {
        IList<string> AddOrder(OrderModel order, int userId);
        IList<OrderResponse> GetAllOrderDetails(int userId);
    }
}
