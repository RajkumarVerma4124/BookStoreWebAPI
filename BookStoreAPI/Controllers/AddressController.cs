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
    /// Created The Address Controller For Http Requests And Response 👈 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Role.User)]
    public class AddressController : ControllerBase
    {
        /// <summary>
        /// Object Reference For Interface IAddressBL,ILogger
        /// </summary>
        private readonly IAddressBL addressBL;
        private readonly ILogger<AddressController> logger;
        private readonly IDistributedCache distributedCache;


        /// <summary>
        /// Constructor To Initialize The Instance Of Interface IAddressBL,ILogger
        /// </summary>
        /// <param name="addressBL"></param>
        /// <param name="logger"></param>
        public AddressController(IAddressBL addressBL, ILogger<AddressController> logger, IDistributedCache distributedCache)
        {
            this.addressBL = addressBL;
            this.logger = logger;
            this.distributedCache = distributedCache;
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
                    logger.LogInformation("Address Added Successfully");
                    return Created("", new { success = true, message = "Address Added Successfully", data = resAddress });
                }
                else
                {
                    logger.LogWarning("Address Addition Failed");
                    return BadRequest(new { success = false, message = "Address Addition Failed" });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
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
                    logger.LogInformation(resAddressStr);
                    return Ok(new { success = true, message = resAddressStr });
                }
                else
                {
                    logger.LogWarning("Address Not Found For Update");
                    return NotFound(new { success = false, message = "Address Not Found For Update" });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
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
                    logger.LogInformation(resAddrStr);
                    return Ok(new { success = true, data = resAddrStr });
                }
                else
                {
                    logger.LogWarning("Address Not Found For Deletion");
                    return NotFound(new { success = false, message = "Address Not Found For Deletion" });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }


        /// <summary>
        /// Post Request For Getting Address (POST: /api/address/get)
        /// </summary>
        /// <returns></returns>
        [HttpGet("Get")]
        public IActionResult GetAddressDetails(int typeId)
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var resAddrlist = this.addressBL.GetAddressDetails(userId, typeId);
                if (resAddrlist != null)
                {
                    logger.LogInformation("Got The Address Details Succesfully");
                    return Ok(new { success = true, message = "Got The Address Details Succesfully", data = resAddrlist });
                }
                else
                {
                    logger.LogWarning("Address Details Not Found");
                    return NotFound(new { success = false, message = "Address Details Not Found" });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Post Request For Getting Address (POST: /api/address/get)
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetById")]
        public IActionResult GetAddressById(int addressId)
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var resAddrlist = this.addressBL.GetAddressById(userId, addressId);
                if (resAddrlist != null)
                {
                    logger.LogInformation("Got The Address Details Succesfully");
                    return Ok(new { success = true, message = "Got The Address Details Succesfully", data = resAddrlist });
                }
                else
                {
                    logger.LogWarning("Address Details Not Found");
                    return NotFound(new { success = false, message = "Address Details Not Found" });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Post Request For Getting All Address (POST: /api/address/getall)
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public IActionResult GetAllAddressDetails()
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var resAddrlist = this.addressBL.GetAllAddressDetails(userId);
                if (resAddrlist.Count() > 0)
                {
                    logger.LogInformation("Got The Address Details Succesfully");
                    return Ok(new { success = true, message = "Got The Address Details Succesfully", data = resAddrlist });
                }
                else
                {
                    logger.LogWarning("Address Details Not Found");
                    return NotFound(new { success = false, message = "Address Details Not Found" });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Get Request For Getting All Address Using Redis (GET: /api/book/redis)
        /// </summary>
        /// <returns></returns>
        [HttpGet("redis")]
        public async Task<IActionResult> GetAllAddressRedisCache()
        {
            try
            {
                var cacheKey = "addressList";
                string serializedAddressList;
                var addressList = new List<AddressResponse>();
                var redisAddressList = await distributedCache.GetAsync(cacheKey);
                if (redisAddressList != null)
                {
                    logger.LogDebug("Getting The List From Redis Cache");
                    serializedAddressList = Encoding.UTF8.GetString(redisAddressList);
                    addressList = JsonConvert.DeserializeObject<List<AddressResponse>>(serializedAddressList);
                }
                else
                {
                    logger.LogDebug("Setting The BooksList List To Cache Which Request For First Time");
                    //Getting The Id Of Authorized User Using Claims Of Jwt
                    int userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                    addressList = addressBL.GetAllAddressDetails(userId).ToList();
                    serializedAddressList = JsonConvert.SerializeObject(addressList);
                    redisAddressList = Encoding.UTF8.GetBytes(serializedAddressList);
                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                    await distributedCache.SetAsync(cacheKey, redisAddressList, options);
                }
                logger.LogInformation("Got The BooksList Successfully Using Redis");
                return Ok(addressList);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "Exception Thrown...");
                return NotFound(new { success = false, message = ex.Message });
            }
        }
    }
}
