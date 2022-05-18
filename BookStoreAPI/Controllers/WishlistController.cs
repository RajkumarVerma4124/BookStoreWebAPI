using BusinessLayer.Interface;
using CommonLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;


namespace BookStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        /// <summary>
        /// Object Reference For Interface ICartBL
        /// </summary>
        private readonly IWishlistBL wishlistBL;

        /// <summary>
        /// Constructor To Initialize The Instance Of Interface IWishlistBL
        /// </summary>
        /// <param name="wishlistBL"></param>
        public WishlistController(IWishlistBL wishlistBL)
        {
            this.wishlistBL = wishlistBL;
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
                    return Created("", new { success = true, data = resBook });
                }
                else
                {
                    return NotFound(new { success = false, message = "The Given Book Was Not Found"});
                }
            }
            catch (Exception ex)
            {
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
                    return Ok(new { success = true, data = resBook });
                }
                else
                {
                    return NotFound(new { success = false, message = "Wishlist Not Found For Deletion" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Post Request For Getting All Wishlist Details (POST: /api/wishlist/getall)
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
                    return Ok(new { success = true, message = "Got All The Wishlist Details Succesfully", data = resWishlist });
                }
                else
                {
                    return NotFound(new { success = false, message = "Wishlist Details Not Found" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}
