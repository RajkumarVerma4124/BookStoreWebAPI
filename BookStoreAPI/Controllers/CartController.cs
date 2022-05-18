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
    /// <summary>
    /// Created The Cart Controller For Http Requests And Response 👈 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        /// <summary>
        /// Object Reference For Interface ICartBL
        /// </summary>
        private readonly ICartBL cartBL;

        /// <summary>
        /// Constructor To Initialize The Instance Of Interface ICartBL
        /// </summary>
        /// <param name="cartBL"></param>
        public CartController(ICartBL cartBL)
        {
            this.cartBL = cartBL;
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
                    return Created("", new { success = true, message = "Book Added To Cart Successfully", data = resBook });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Book Addition To Cart Failed" });
                }
            }
            catch (Exception ex)
            {
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
                    return Ok(new { success = true, data = resBook });
                }
                else
                {
                    return NotFound(new { success = false, message = "Cart Not Found For Deletion" });
                }
            }
            catch (Exception ex)
            {
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
                    return Ok(new { success = true, message = "Updated The Cart Succesfully" });
                }
                else
                {
                    return NotFound(new { success = false, message = "Cart Not Found" });
                }
            }
            catch (Exception ex)
            {
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
                if (resCartlist.Count() > 0)
                {
                    return Ok(new { success = true, message = "Got All The Cart Details Succesfully", data = resCartlist });
                }
                else
                {
                    return NotFound(new { success = false, message = "Cart Details Not Found" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}
