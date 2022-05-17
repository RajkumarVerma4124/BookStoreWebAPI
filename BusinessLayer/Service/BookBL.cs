using BusinessLayer.Interface;
using CommonLayer.Models;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Service
{
    /// <summary>
    /// Created The User Business Layer Class To Implement IUserBL Methods
    /// </summary>

    public class BookBL : IBookBL
    {
        /// <summary>
        /// Reference Object For Interface IBookRL
        /// </summary>
        private readonly IBookRL bookRL;

        /// <summary>
        /// Created Constructor With Dependency Injection For IBookRL
        /// </summary>
        /// <param name="userRL"></param>
        public BookBL(IBookRL bookRL)
        {
            this.bookRL = bookRL;
        }
        public BookModel AddBook(AddBookModel bookModel)
        {
            try
            {
                return bookRL.AddBook(bookModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string DeleteBook(int bookId)
        {
            try
            {
                return bookRL.DeleteBook(bookId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IList<BookModel> GetAllBooks()
        {
            try
            {
                return bookRL.GetAllBooks();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public BookModel GetBookById(int bookId)
        {
            try
            {
                return bookRL.GetBookById(bookId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public BookModel UpdateBook(BookModel updatebook)
        {
            try
            {
                return bookRL.UpdateBook(updatebook);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
