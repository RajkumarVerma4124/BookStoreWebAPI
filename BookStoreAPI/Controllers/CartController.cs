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
    /// Created The Cart Controller For Http Requests And Response 👈 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Role.User)]
    public class CartController : ControllerBase
    {
        /// <summary>
        /// Object Reference For Interface ICartBL,ILogger,IDistributedCache
        /// </summary>
        private readonly ICartBL cartBL;
        private readonly ILogger<CartController> logger;
        private readonly IDistributedCache distributedCache;

        /// <summary>
        /// Constructor To Initialize The Instance Of Interface ICartBL,ILogger,IDistributedCache
        /// </summary>
        /// <param name="cartBL"></param>
        /// <param name="logger"></param>
        public CartController(ICartBL cartBL, ILogger<CartController> logger, IDistributedCache distributedCache)
        {
            this.cartBL = cartBL;
            this.logger = logger;
            this.distributedCache = distributedCache;
        }

        /// <summary>
        /// Post Request For Adding A Book To Cart(POST: /api/cart/add)
        /// </summary>
        /// <param name="cartModel"></param>
        /// <returns></returns>
        [HttpPost("Add")]
        public IActionResult AddBookToCart(AddCartModel cartModel)
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var resBook = this.cartBL.AddBookToCart(cartModel, userId);
                if (resBook != null)
                {
                    logger.LogInformation("Book Added To Cart Successfully");
                    return Created("", new { success = true, message = "Book Added To Cart Successfully", data = resBook });
                }
                else
                {
                    logger.LogWarning("Book Addition To Cart Failed");
                    return BadRequest(new { success = false, message = "Book Addition To Cart Failed" });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Delete Request For Deleting A Cart(DELETE: /api/cart/delete)
        /// </summary>
        /// <param name="cartId"></param>
        /// <returns></returns>
        [HttpDelete("Delete")]
        public IActionResult DeleteCart(int cartId)
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var resBook = this.cartBL.DeleteCart(cartId, userId);
                if (!string.IsNullOrEmpty(resBook))
                {
                    logger.LogInformation(resBook);
                    return Ok(new { success = true, data = resBook });
                }
                else
                {
                    logger.LogWarning("Cart Not Found For Deletion");
                    return NotFound(new { success = false, message = "Cart Not Found For Deletion" });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Put Request For Updating A New Book (PUT: /api/book/update)
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="bookquantity"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPut("Update")]
        public IActionResult UpdateBook(int cartId, int bookquantity)
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var resBook = this.cartBL.UpdateBook(cartId, bookquantity, userId);
                if (resBook != null)
                {
                    logger.LogInformation("Updated The Cart Succesfully");
                    return Ok(new { success = true, message = "Updated The Cart Succesfully" });
                }
                else
                {
                    logger.LogWarning("Cart Not Found For Update");
                    return NotFound(new { success = false, message = "Cart Not Found For Update" });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Post Request For Getting All Carts Details (POST: /api/book/getall)
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public IActionResult GetAllCartDetails()
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var resCartlist = this.cartBL.GetAllCartDetails(userId);
                if (resCartlist !=  null)
                {
                    logger.LogInformation("Got All The Cart Details Succesfully");
                    return Ok(new { success = true, message = "Got All The Cart Details Succesfully", data = resCartlist });
                }
                else
                {
                    logger.LogWarning("Cart Details Not Found");
                    return NotFound(new { success = false, message = "Cart Details Not Found" });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }


        /// <summary>
        /// Get Request For Getting All cartlist Using Redis(GET: /api/wishlist/redis)
        /// </summary>
        /// <returns></returns>
        [HttpGet("redis")]
        public async Task<IActionResult> GetAllCartUsingRedisCache()
        {
            try
            {
                var cacheKey = "cartList";
                string serializedCartList;
                var cartList = new List<CartResponseModel>();
                var redisCartList = await distributedCache.GetAsync(cacheKey);
                if (redisCartList != null)
                {
                    logger.LogDebug("Getting The List From Redis Cache");
                    serializedCartList = Encoding.UTF8.GetString(redisCartList);
                    cartList = JsonConvert.DeserializeObject<List<CartResponseModel>>(serializedCartList);
                }
                else
                {
                    logger.LogDebug("Setting The CartList List To Cache Which Request For First Time");
                    //Getting The Id Of Authorized User Using Claims Of Jwt
                    int userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                    cartList = cartBL.GetAllCartDetails(userId).ToList();
                    serializedCartList = JsonConvert.SerializeObject(cartList);
                    redisCartList = Encoding.UTF8.GetBytes(serializedCartList);
                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                    await distributedCache.SetAsync(cacheKey, redisCartList, options);
                }
                logger.LogInformation("Got The CartList Successfully Using Redis");
                return Ok(cartList);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "Exception Thrown...");
                return NotFound(new { success = false, message = ex.Message });
            }
        }
    }
}
