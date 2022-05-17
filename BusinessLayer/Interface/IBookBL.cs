﻿using CommonLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    public interface IBookBL
    {
        BookModel AddBook(AddBookModel bookModel);
        BookModel UpdateBook(BookModel updatebook);
        string DeleteBook(int bookId);
        BookModel GetBookById(int bookId);
        IList<BookModel> GetAllBooks();
    }
}
