using CommonLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interface
{
    /// <summary>
    /// Created The Interface For Address Repository layer
    /// </summary>
    public interface IAddressRL
    {
        public AddressModel AddAddress(AddressModel address, int userId);
        string UpdateAddress(AddressResponse address);
        string DeleteAddress(int addressId);
        IList<AddressResponse> GetAddressDetails(int userId, int typeId);
        IList<AddressResponse> GetAllAddressDetails(int userId);
    }
}
