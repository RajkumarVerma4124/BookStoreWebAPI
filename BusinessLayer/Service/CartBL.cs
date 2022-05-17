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
    public class CartBL : ICartBL
    {
        /// <summary>
        /// Reference Object For Interface IBookRL
        /// </summary>
        private readonly ICartRL cartRL;

        /// <summary>
        /// Created Constructor With Dependency Injection For IBookRL
        /// </summary>
        /// <param name="userRL"></param>
        public CartBL(ICartRL cartRL)
        {
            this.cartRL = cartRL;
        }

        public AddCartModel AddBookToCart(AddCartModel cartModel, int userId)
        {
            try
            {
                return cartRL.AddBookToCart(cartModel, userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string DeleteCart(int cartId, int userId)
        {
            try
            {
                return cartRL.DeleteCart(cartId, userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IList<CartResponseModel> GetAllCartDetails(int userId)
        {
            try
            {
                return cartRL.GetAllCartDetails(userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string UpdateBook(int cartId, int bookquantity, int userId)
        {
            try
            {
                return cartRL.UpdateBook(cartId, bookquantity, userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
