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
    /// Created The Book Controller For Http Requests And Response 👈 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        /// <summary>
        /// Object Reference For Interface IBookBL,ILogger,IDistributedCache
        /// </summary>
        private readonly IBookBL bookBL;
        private readonly ILogger<BookController> logger;
        private readonly IDistributedCache distributedCache;

        /// <summary>
        /// Constructor To Initialize The Instance Of Interface IBookBL,ILogger,IDistributedCache
        /// </summary>
        /// <param name="userBL"></param>
        /// <param name="logger"></param>
        public BookController(IBookBL bookBL, ILogger<BookController> logger, IDistributedCache distributedCache)
        {
            this.bookBL = bookBL;
            this.logger = logger;
            this.distributedCache = distributedCache;
        }

        /// <summary>
        /// Post Request For Adding A New Book (POST: /api/book/add)
        /// </summary>
        /// <param name="bookModel"></param>
        /// <returns></returns>
        [HttpPost("Add")]
        [Authorize(Roles = Role.Admin)]
        public IActionResult AddBook(AddBookModel bookModel)
        {
            try
            {
                var resBook = this.bookBL.AddBook(bookModel);
                if (resBook != null)
                {
                    logger.LogInformation("Book Added Successfully");
                    return Created("Book Added Successfully", new { success = true, data = resBook });
                }
                else
                {
                    logger.LogWarning("Book Addition Failed");
                    return BadRequest(new { success = false, message = "Book Addition Failed" });
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
        /// <param name="bookModel"></param>
        /// <returns></returns>
        [HttpPut("Update")]
        [Authorize(Roles = Role.Admin)]
        public IActionResult UpdateBook(BookModel bookModel)
        {
            try
            {
                var resBook = this.bookBL.UpdateBook(bookModel);
                if (resBook != null)
                {
                    logger.LogInformation("Updated The Book Succesfully");
                    return Ok(new { success = true, message= "Updated The Book Succesfully", data = resBook });
                }
                else
                {
                    logger.LogWarning("Book Not Found");
                    return NotFound(new { success = false, message = "Book Not Found" });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Delete Request For Deleting A Book (DELETE: /api/book/delete)
        /// </summary>
        /// <param name="bookModel"></param>
        /// <returns></returns>
        [HttpDelete("Delete")]
        [Authorize(Roles = Role.Admin)]
        public IActionResult DeleteBook(int bookId)
        {
            try
            {
                var resBook = this.bookBL.DeleteBook(bookId);
                if (!string.IsNullOrEmpty(resBook))
                {
                    logger.LogInformation(resBook);
                    return Ok(new { success = true, data = resBook });
                }
                else
                {
                    logger.LogWarning("Book Not Found For Deletion");
                    return NotFound(new { success = false, message = "Book Not Found For Deletion" });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Post Request For Getting A Book By Id (POST: /api/book/getbook)
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        [HttpGet("Get")]
        public IActionResult GetBookById(int bookId)
        {
            try
            {
                var resBook = this.bookBL.GetBookById(bookId);
                if (resBook != null)
                {
                    logger.LogInformation("Got The Book Succesfully");
                    return Ok(new { success = true, message = "Got The Book Succesfully", data = resBook });
                }
                else
                {
                    logger.LogWarning("Book Not Found");
                    return NotFound(new { success = false, message = "Book Not Found" });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }


        /// <summary>
        /// Get Request For Getting All Books (GET: /api/book/getall)
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public IActionResult GetAllBooks()
        {
            try
            {
                var resBookList = this.bookBL.GetAllBooks();
                if (resBookList.Count() > 0)
                {
                    logger.LogInformation("Got ALL The Book Succesfully");
                    return Ok(new { success = true, message = "Got ALL The Book Succesfully", data = resBookList });
                }
                else
                {
                    logger.LogWarning("Book Not Found");
                    return NotFound(new { success = false, message = "Books Not Found" });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Get Request For Getting All Books Using Redis (GET: /api/book/redis)
        /// </summary>
        /// <returns></returns>
        [HttpGet("redis")]
        public async Task<IActionResult> GetAllBooksRedisCache()
        {
            try
            {
                var cacheKey = "bookList";
                string serializedBookList;
                var bookList = new List<BookModel>();
                var redisBookList = await distributedCache.GetAsync(cacheKey);
                if (redisBookList != null)
                {
                    logger.LogDebug("Getting The List From Redis Cache");
                    serializedBookList = Encoding.UTF8.GetString(redisBookList);
                    bookList = JsonConvert.DeserializeObject<List<BookModel>>(serializedBookList);
                }
                else
                {
                    logger.LogDebug("Setting The BooksList List To Cache Which Request For First Time");
                    bookList = bookBL.GetAllBooks().ToList();
                    serializedBookList = JsonConvert.SerializeObject(bookList);
                    redisBookList = Encoding.UTF8.GetBytes(serializedBookList);
                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                    await distributedCache.SetAsync(cacheKey, redisBookList, options);
                }
                logger.LogInformation("Got The BooksList Successfully Using Redis");
                return Ok(bookList);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "Exception Thrown...");
                return NotFound(new { success = false, message = ex.Message });
            }
        }
    }
}
