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
    /// Created The Address Controller For Http Requests And Response 👈 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AddressController : ControllerBase
    {
        /// <summary>
        /// Object Reference For Interface IAddressBL
        /// </summary>
        private readonly IAddressBL addressBL;

        /// <summary>
        /// Constructor To Initialize The Instance Of Interface IAddressBL
        /// </summary>
        /// <param name="addressBL"></param>
        public AddressController(IAddressBL addressBL)
        {
            this.addressBL = addressBL;
        }

        /// <summary>
        /// Post Request For Adding A Address To Cart(POST: /api/address/add)
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        [HttpPost("Add")]
        public IActionResult AddAddress(AddressModel address)
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var resAddress = this.addressBL.AddAddress(address, userId);
                if (resAddress != null)
                {
                    return Created("", new { success = true, message = "Address Added Successfully", data = resAddress });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Address Addition Failed" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }


        /// <summary>
        /// Put Request For Updating A Address (PUT: /api/address/update)
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        [HttpPut("Update")]
        public IActionResult UpdateAddress(AddressResponse address)
        {
            try
            {
                string resAddressStr = this.addressBL.UpdateAddress(address);
                if (!string.IsNullOrEmpty(resAddressStr))
                {
                    return Ok(new { success = true, message = resAddressStr });
                }
                else
                {
                    return NotFound(new { success = false, message = "Address Not Found For Update" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }


        /// <summary>
        /// Delete Request For Deleting A Address(DELETE: /api/address/delete)
        /// </summary>
        /// <param name="addressId"></param>
        /// <returns></returns>
        [HttpDelete("Delete")]
        public IActionResult DeleteAddress(int addressId)
        {
            try
            {
                var resAddrStr = this.addressBL.DeleteAddress(addressId);
                if (!string.IsNullOrEmpty(resAddrStr))
                {
                    return Ok(new { success = true, data = resAddrStr });
                }
                else
                {
                    return NotFound(new { success = false, message = "Address Not Found For Deletion" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }


        /// <summary>
        /// Post Request For Getting Address (POST: /api/address/get)
        /// </summary>
        /// <returns></returns>
        [HttpPost("Get")]
        public IActionResult GetAddressDetails(int typeId)
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var resAddrlist = this.addressBL.GetAddressDetails(userId, typeId);
                if (resAddrlist != null)
                {
                    return Ok(new { success = true, message = "Got The Address Details Succesfully", data = resAddrlist });
                }
                else
                {
                    return NotFound(new { success = false, message = "Address Details Not Found" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Post Request For Getting All Address (POST: /api/address/getall)
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetAll")]
        public IActionResult GetAllAddressDetails()
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var resAddrlist = this.addressBL.GetAllAddressDetails(userId);
                if (resAddrlist.Count() > 0)
                {
                    return Ok(new { success = true, message = "Got The Address Details Succesfully", data = resAddrlist });
                }
                else
                {
                    return NotFound(new { success = false, message = "Address Details Not Found" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}
