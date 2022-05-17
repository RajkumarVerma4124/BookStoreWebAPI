using CommonLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interface
{
    /// <summary>
    /// Created The Interface For Wishlist Repository Layer Class
    /// </summary>
    public interface IWishlistRL
    {
        string AddBookToWishlist(int bookId, int userId);
        string DeleteWishlist(int wishlistId);
        IList<WishListResponse> GetAllWishlistDetails(int userId);
    }
}
