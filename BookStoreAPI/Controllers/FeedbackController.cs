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
    /// Created The Feedback Controller For Http Requests And Response 👈 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        /// <summary>
        /// Object Reference For Interface IFeedbackBL,ILogger,IDistributedCache
        /// </summary>
        private readonly IFeedbackBL feedbackBL;
        private readonly ILogger<FeedbackController> logger;
        private readonly IDistributedCache distributedCache;

        /// <summary>
        /// Constructor To Initialize The Instance Of Interface IFeedbackBL,ILogger,IDistributedCache
        /// </summary>
        /// <param name="feedbackBL"></param>
        /// <param name="logger"></param>
        /// <param name="distributedCache"></param>
        public FeedbackController(IFeedbackBL feedbackBL, ILogger<FeedbackController> logger, IDistributedCache distributedCache)
        {
            this.feedbackBL = feedbackBL;
            this.logger = logger;
            this.distributedCache = distributedCache;
        }

        /// <summary>
        /// Post Request For Adding A Feedback To Book(POST: /api/feedback/add)
        /// </summary>
        /// <param name="feedback"></param>
        /// <returns></returns>
        [HttpPost("Add")]
        [Authorize(Roles = Role.User)]
        public IActionResult AddFeedback(FeedbackModel feedback)
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var resFeedBack = this.feedbackBL.AddFeedback(feedback, userId);
                if (resFeedBack != null)
                {
                    logger.LogInformation("Feedback Added Successfully");
                    return Created("", new { success = true, message = "Feedback Added Successfully", data = resFeedBack });
                }
                else
                {
                    logger.LogWarning("Feedback Addition Failed");
                    return BadRequest(new { success = false, message = "Feedback Addition Failed" });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }


        /// <summary>
        /// Post Request For Getting All Feedbacks (POST: /api/feedback/getall)
        /// </summary>
        /// <param name="bookid"></param>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public IActionResult GetAllFeedbackDetails(int bookid)
        {
            try
            {
                var resFeedBackList = this.feedbackBL.GetAllFeedbackDetails(bookid);
                if (resFeedBackList != null)
                {
                    logger.LogInformation("Got All The Feedback Details Succesfully");
                    return Ok(new { success = true, message = "Got All The Feedback Details Succesfully", data = resFeedBackList });
                }
                else
                {
                    logger.LogWarning("Feedback Details Not Found");
                    return NotFound(new { success = false, message = "Feedback Details Not Found" });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Get Request For Getting All feedbacklist Using Redis(GET: /api/wishlist/redis)
        /// </summary>
        /// <returns></returns>
        [HttpGet("redis")]
        public async Task<IActionResult> GetAllFeedbackUsingRedisCache(int bookid)
        {
            try
            {
                var cacheKey = "feedbackList";
                string serializedFeedbackList;
                var feedbackList = new List<FeedbackResponse>();
                var redisFeedbackList = await distributedCache.GetAsync(cacheKey);
                if (redisFeedbackList != null)
                {
                    logger.LogDebug("Getting The List From Redis Cache");
                    serializedFeedbackList = Encoding.UTF8.GetString(redisFeedbackList);
                    feedbackList = JsonConvert.DeserializeObject<List<FeedbackResponse>>(serializedFeedbackList);
                }
                else
                {
                    logger.LogDebug("Setting The feedbackList List To Cache Which Request For First Time");
                    feedbackList = feedbackBL.GetAllFeedbackDetails(bookid).ToList();
                    serializedFeedbackList = JsonConvert.SerializeObject(feedbackList);
                    redisFeedbackList = Encoding.UTF8.GetBytes(serializedFeedbackList);
                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                    await distributedCache.SetAsync(cacheKey, redisFeedbackList, options);
                }
                logger.LogInformation("Got The FeedbackList Successfully Using Redis");
                return Ok(feedbackList);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "Exception Thrown...");
                return NotFound(new { success = false, message = ex.Message });
            }
        }
    }
}
