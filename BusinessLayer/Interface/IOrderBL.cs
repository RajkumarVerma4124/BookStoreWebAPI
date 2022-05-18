using CommonLayer.Models;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    /// <summary>
    /// Created The Interface For Order Business Layer 
    /// </summary>
    public interface IOrderBL
    {
        OrderModel AddOrder(OrderModel order, int userId);
        IList<OrderResponse> GetAllOrderDetails(int userId);
    }
}
