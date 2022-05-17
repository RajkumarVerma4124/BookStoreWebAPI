using CommonLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    /// <summary>
    /// Created The Interface For Wishlist Business Layer Class
    /// </summary>
    public interface IWishlistBL
    {
        string AddBookToWishlist(int bookId, int userId);
        string DeleteWishlist(int wishlistId);
        IList<WishListResponse> GetAllWishlistDetails(int userId);
    }
}
