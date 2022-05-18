using CommonLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    /// <summary>
    /// Created The Address Business Layer Interface For Address
    /// </summary>
    public interface IAddressBL
    {
        public AddressModel AddAddress(AddressModel address, int userId);
        string UpdateAddress(AddressResponse address);
        string DeleteAddress(int addressId);
        IList<AddressResponse> GetAddressDetails(int userId, int typeId);
        IList<AddressResponse> GetAllAddressDetails(int userId);
    }
}
