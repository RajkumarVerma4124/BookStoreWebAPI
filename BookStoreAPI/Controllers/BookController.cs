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
    /// Created The Book Controller For Http Requests And Response 👈 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        /// <summary>
        /// Object Reference For Interface IBookBL
        /// </summary>
        private readonly IBookBL bookBL;

        /// <summary>
        /// Constructor To Initialize The Instance Of Interface IBookBL
        /// </summary>
        /// <param name="userBL"></param>
        public BookController(IBookBL bookBL)
        {
            this.bookBL = bookBL;
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
                    return Created("Book Added Successfully", new { success = true, data = resBook });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Book Addition Failed" });
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
                    return Ok(new { success = true, message= "Updated The Book Succesfully", data = resBook });
                }
                else
                {
                    return NotFound(new { success = false, message = "Book Not Found" });
                }
            }
            catch (Exception ex)
            {
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
                    return Ok(new { success = true, data = resBook });
                }
                else
                {
                    return NotFound(new { success = false, message = "Book Not Found For Deletion" });
                }
            }
            catch (Exception ex)
            {
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
                    return Ok(new { success = true, message = "Got The Book Succesfully", data = resBook });
                }
                else
                {
                    return NotFound(new { success = false, message = "Book Not Found" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }


        /// <summary>
        /// Post Request For Getting All Books (POST: /api/book/getall)
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
                    return Ok(new { success = true, message = "Got ALL The Book Succesfully", data = resBookList });
                }
                else
                {
                    return NotFound(new { success = false, message = "Books Not Found" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}
