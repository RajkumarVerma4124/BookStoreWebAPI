using CommonLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interface
{
    public interface ICartRL
    {
        AddCartModel AddBookToCart(AddCartModel cartModel, int userId);
        string DeleteCart(int cartId, int userId);
        string UpdateBook(int cartId, int bookquantity, int userId);
        IList<CartResponseModel> GetAllCartDetails(int userId);
    }
}
