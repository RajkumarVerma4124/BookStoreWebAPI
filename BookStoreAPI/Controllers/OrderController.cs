﻿using BusinessLayer.Interface;
using CommonLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

namespace BookStoreAPI.Controllers
{
    /// <summary>
    /// Created The Order Controller For Http Requests And Response 👈 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        /// <summary>
        /// Object Reference For Interface IOrderBL
        /// </summary>
        private readonly IOrderBL orderBL;

        /// <summary>
        /// Constructor To Initialize The Instance Of Interface IOrderBL
        /// </summary>
        /// <param name="orderBL"></param>
        public OrderController(IOrderBL orderBL)
        {
            this.orderBL = orderBL;
        }

        /// <summary>
        /// Post Request For Adding A Order(POST: /api/order/add)
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpPost("Add")]
        public IActionResult AddOrder(OrderModel order)
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var resBookOrder = this.orderBL.AddOrder(order, userId);
                if (resBookOrder != null)
                {
                    return Created("", new { success = true, message = "Order Placed Successfully", data = resBookOrder });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Order Addition Failed" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Post Request For Getting All Orders (POST: /api/order/getall)
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetAll")]
        public IActionResult GetAllOrderDetails()
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var resOrderList = this.orderBL.GetAllOrderDetails(userId);
                if (resOrderList != null)
                {
                    return Ok(new { success = true, message = "Got All The Order Details Succesfully", data = resOrderList });
                }
                else
                {
                    return NotFound(new { success = false, message = "Order Details Not Found" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}