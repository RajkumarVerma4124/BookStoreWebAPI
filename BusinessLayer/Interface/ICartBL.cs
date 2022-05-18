using CommonLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    /// <summary>
    /// Created The User Business Layer Interface For Cart
    /// </summary>
    public interface ICartBL
    {
        AddCartModel AddBookToCart(AddCartModel cartModel, int userId);
        string DeleteCart(int cartId, int userId);
        string UpdateBook(int cartId, int bookquantity, int userId);
        IList<CartResponseModel> GetAllCartDetails(int userId);
    }
}
