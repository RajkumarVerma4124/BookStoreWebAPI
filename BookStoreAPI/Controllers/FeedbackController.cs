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
    /// Created The Feedback Controller For Http Requests And Response 👈 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        /// <summary>
        /// Object Reference For Interface IFeedbackBL,ILogger
        /// </summary>
        private readonly IFeedbackBL feedbackBL;
        private readonly ILogger<FeedbackController> logger;


        /// <summary>
        /// Constructor To Initialize The Instance Of Interface IFeedbackBL,ILogger
        /// </summary>
        /// <param name="feedbackBL"></param>
        /// <param name="logger"></param>
        public FeedbackController(IFeedbackBL feedbackBL, ILogger<FeedbackController> logger)
        {
            this.feedbackBL = feedbackBL;
            this.logger = logger;
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
    }
}
