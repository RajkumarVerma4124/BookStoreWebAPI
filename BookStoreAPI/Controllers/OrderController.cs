using BusinessLayer.Interface;
using CommonLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
    [Authorize(Roles = Role.User)]
    public class OrderController : ControllerBase
    {
        /// <summary>
        /// Object Reference For Interface IOrderBL,ILogger
        /// </summary>
        private readonly IOrderBL orderBL;
        private readonly ILogger<OrderController> logger;


        /// <summary>
        /// Constructor To Initialize The Instance Of Interface IOrderBL,ILogger
        /// </summary>
        /// <param name="orderBL"></param>
        /// <param name="logger"></param>
        public OrderController(IOrderBL orderBL, ILogger<OrderController> logger)
        {
            this.orderBL = orderBL;
            this.logger = logger;
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
                    logger.LogInformation(resBookOrder[0]);
                    return Created("", new { success = true, message = resBookOrder[0] });
                }
                else
                {
                    logger.LogWarning("Order Addition Failed");
                    return BadRequest(new { success = false, message = "Order Addition Failed" });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Post Request For Getting All Orders (POST: /api/order/getall)
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public IActionResult GetAllOrderDetails()
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var resOrderList = this.orderBL.GetAllOrderDetails(userId);
                if (resOrderList != null)
                {
                    logger.LogInformation("Got All The Order Details Succesfully");
                    return Ok(new { success = true, message = "Got All The Order Details Succesfully", data = resOrderList });
                }
                else
                {
                    logger.LogWarning("Order Details Not Found");
                    return NotFound(new { success = false, message = "Order Details Not Found" });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}
