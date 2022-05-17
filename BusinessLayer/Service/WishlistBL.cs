using BusinessLayer.Interface;
using CommonLayer.Models;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Service
{
    /// <summary>
    /// Created The User Business Layer Class To Implement IWishListBL Methods
    /// </summary>
    public class WishlistBL: IWishlistBL
    {
        /// <summary>
        /// Reference Object For Interface IBookRL
        /// </summary>
        private readonly IWishlistRL wishlistRL;

        /// <summary>
        /// Created Constructor With Dependency Injection For IBookRL
        /// </summary>
        /// <param name="wishlistRL"></param>
        public WishlistBL(IWishlistRL wishlistRL)
        {
            this.wishlistRL = wishlistRL;
        }

        public string AddBookToWishlist(int bookId, int userId)
        {
            try
            {
                return wishlistRL.AddBookToWishlist(bookId, userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string DeleteWishlist(int wishlistId)
        {
            try
            {
                return wishlistRL.DeleteWishlist(wishlistId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IList<WishListResponse> GetAllWishlistDetails(int userId)
        {
            try
            {
                return wishlistRL.GetAllWishlistDetails(userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
