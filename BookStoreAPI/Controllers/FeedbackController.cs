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
    /// Created The Feedback Controller For Http Requests And Response 👈 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        /// <summary>
        /// Object Reference For Interface IFeedbackBL
        /// </summary>
        private readonly IFeedbackBL feedbackBL;

        /// <summary>
        /// Constructor To Initialize The Instance Of Interface IFeedbackBL
        /// </summary>
        /// <param name="feedbackBL"></param>
        public FeedbackController(IFeedbackBL feedbackBL)
        {
            this.feedbackBL = feedbackBL;
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
                    return Created("", new { success = true, message = "Feedback Added Successfully", data = resFeedBack });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Feedback Addition Failed" });
                }
            }
            catch (Exception ex)
            {
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
                    return Ok(new { success = true, message = "Got All The Feedback Details Succesfully", data = resFeedBackList });
                }
                else
                {
                    return NotFound(new { success = false, message = "Feedback Details Not Found" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}
