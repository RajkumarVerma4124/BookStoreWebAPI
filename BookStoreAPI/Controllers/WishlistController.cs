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
    [Route("api/[controller]")]
    [Authorize(Roles = Role.User)]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        /// <summary>
        /// Object Reference For Interface ICartBL,ILogger,IDistributedCache
        /// </summary>
        private readonly IWishlistBL wishlistBL;
        private readonly ILogger<WishlistController> logger;
        private readonly IDistributedCache distributedCache;

        /// <summary>
        /// Constructor To Initialize The Instance Of Interface IWishlistBL,ILogger,IDistributedCache
        /// </summary>
        /// <param name="wishlistBL"></param>
        /// <param name="logger"></param>
        /// <param name="distributedCache"></param>
        public WishlistController(IWishlistBL wishlistBL, ILogger<WishlistController> logger, IDistributedCache distributedCache)
        {
            this.wishlistBL = wishlistBL;
            this.logger = logger;
            this.distributedCache = distributedCache;
        }

        /// <summary>
        /// Post Request For Adding A Book To Wishlist(POST: /api/wislist/add)
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        [HttpPost("Add")]
        public IActionResult AddBookToWishlist(int bookId)
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                string resBook = this.wishlistBL.AddBookToWishlist(bookId, userId);
                if (!string.IsNullOrEmpty(resBook))
                {
                    logger.LogInformation(resBook);
                    return Created("", new { success = true, data = resBook });
                }
                else
                {
                    logger.LogWarning("The Given Book Was Not Found");
                    return NotFound(new { success = false, message = "The Given Book Was Not Found"});
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Delete Request For Deleting A Wishlist(DELETE: /api/wishlist/delete)
        /// </summary>
        /// <param name="wishlistId"></param>
        /// <returns></returns>
        [HttpDelete("Delete")]
        public IActionResult DeleteWishlist(int wishlistId)
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var resBook = this.wishlistBL.DeleteWishlist(wishlistId);
                if (!string.IsNullOrEmpty(resBook))
                {
                    logger.LogInformation(resBook);
                    return Ok(new { success = true, data = resBook });
                }
                else
                {
                    logger.LogWarning("Wishlist Not Found For Deletion");
                    return NotFound(new { success = false, message = "Wishlist Not Found For Deletion" });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Get Request For Getting All Wishlist Details (GET: /api/wishlist/getall)
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public IActionResult GetAllWishlistDetails()
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var resWishlist = this.wishlistBL.GetAllWishlistDetails(userId);
                if (resWishlist.Count() > 0)
                {
                    logger.LogInformation("Got All The Wishlist Details Succesfully");
                    return Ok(new { success = true, message = "Got All The Wishlist Details Succesfully", data = resWishlist });
                }
                else
                {
                    logger.LogWarning("Wishlist Details Not Found");
                    return NotFound(new { success = false, message = "Wishlist Details Not Found" });
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
        public async Task<IActionResult> GetAllWishlistUsingRedisCache()
        {
            try
            {
                var cacheKey = "wishList";
                string serializedWishList;
                var wishList = new List<WishListResponse>();
                var redisWishList = await distributedCache.GetAsync(cacheKey);
                if (redisWishList != null)
                {
                    logger.LogDebug("Getting The List From Redis Cache");
                    serializedWishList = Encoding.UTF8.GetString(redisWishList);
                    wishList = JsonConvert.DeserializeObject<List<WishListResponse>>(serializedWishList);
                }
                else
                {
                    logger.LogDebug("Setting The Wishlist List To Cache Which Request For First Time");
                    //Getting The Id Of Authorized User Using Claims Of Jwt
                    int userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                    wishList = wishlistBL.GetAllWishlistDetails(userId).ToList();
                    serializedWishList = JsonConvert.SerializeObject(wishList);
                    redisWishList = Encoding.UTF8.GetBytes(serializedWishList);
                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                    await distributedCache.SetAsync(cacheKey, redisWishList, options);
                }
                logger.LogInformation("Got The WishList Successfully Using Redis");
                return Ok(wishList);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, " Exception Thrown...");
                return NotFound(new { success = false, message = ex.Message });
            }
        }
    }
}
