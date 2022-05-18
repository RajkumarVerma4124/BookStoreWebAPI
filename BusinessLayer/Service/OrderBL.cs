using BusinessLayer.Interface;
using CommonLayer.Models;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Service
{
    /// <summary>
    /// Created The Order Business Layer Class To Implement IOrderBL Methods
    /// </summary>
    public class OrderBL: IOrderBL
    {
        /// <summary>
        /// Reference Object For Interface IOrderRL
        /// </summary>
        private readonly IOrderRL orderRL;

        /// <summary>
        /// Created Constructor With Dependency Injection For IOrderRL
        /// </summary>
        /// <param name="addressRL"></param>
        public OrderBL(IOrderRL orderRL)
        {
            this.orderRL = orderRL;
        }

        public OrderModel AddOrder(OrderModel order, int userId)
        {
            try
            {
                return orderRL.AddOrder(order, userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IList<OrderResponse> GetAllOrderDetails(int userId)
        {
            try
            {
                return orderRL.GetAllOrderDetails(userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
