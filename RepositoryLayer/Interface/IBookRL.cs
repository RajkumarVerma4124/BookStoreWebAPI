using CommonLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interface
{
    public interface IBookRL
    {
        BookModel AddBook(AddBookModel bookModel);
        BookModel UpdateBook(BookModel updatebook);
        string DeleteBook(int bookId);
        BookModel GetBookById(int bookId);
        IList<BookModel> GetAllBooks();

    }
}
