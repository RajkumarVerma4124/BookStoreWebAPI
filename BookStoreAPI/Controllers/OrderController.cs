using BusinessLayer.Interface;
using CommonLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

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
        /// Object Reference For Interface IOrderBL,ILogger,IDistributedCache
        /// </summary>
        private readonly IOrderBL orderBL;
        private readonly ILogger<OrderController> logger;
        private readonly IDistributedCache distributedCache;

        /// <summary>
        /// Constructor To Initialize The Instance Of Interface IOrderBL,ILogger,IDistributedCache
        /// </summary>
        /// <param name="orderBL"></param>
        /// <param name="logger"></param>
        /// <param name="distributedCache"></param>
        public OrderController(IOrderBL orderBL, ILogger<OrderController> logger, IDistributedCache distributedCache)
        {
            this.orderBL = orderBL;
            this.logger = logger;
            this.distributedCache = distributedCache;
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
        /// Get Request For Getting All Orders (GET: /api/order/getall)
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

        /// <summary>
        /// Get Request For Getting All Wishlist Using Redis(GET: /api/wishlist/redis)
        /// </summary>
        /// <returns></returns>
        [HttpGet("redis")]
        public async Task<IActionResult> GetAllOrderUsingRedisCache()
        {
            try
            {
                var cacheKey = "orderList";
                string serializedOrderList;
                var orderList = new List<OrderResponse>();
                var redisOrderList = await distributedCache.GetAsync(cacheKey);
                if (redisOrderList != null)
                {
                    logger.LogDebug("Getting The List From Redis Cache");
                    serializedOrderList = Encoding.UTF8.GetString(redisOrderList);
                    orderList = JsonConvert.DeserializeObject<List<OrderResponse>>(serializedOrderList);
                }
                else
                {
                    logger.LogDebug("Setting The orderList List To Cache Which Request For First Time");
                    //Getting The Id Of Authorized User Using Claims Of Jwt
                    int userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                    orderList = orderBL.GetAllOrderDetails(userId).ToList();
                    serializedOrderList = JsonConvert.SerializeObject(orderList);
                    redisOrderList = Encoding.UTF8.GetBytes(serializedOrderList);
                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                    await distributedCache.SetAsync(cacheKey, redisOrderList, options);
                }
                logger.LogInformation("Got The orderList Successfully Using Redis");
                return Ok(orderList);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "Exception Thrown...");
                return NotFound(new { success = false, message = ex.Message });
            }
        }
    }
}
