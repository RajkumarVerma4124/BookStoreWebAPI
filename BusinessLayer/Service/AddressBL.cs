using BusinessLayer.Interface;
using CommonLayer.Models;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Service
{
    /// <summary>
    /// Created The Address Business Layer Class To Implement IAddressBL Methods
    /// </summary>
    public class AddressBL: IAddressBL
    {
        /// <summary>
        /// Reference Object For Interface IBookRL
        /// </summary>
        private readonly IAddressRL addressRL;

        /// <summary>
        /// Created Constructor With Dependency Injection For IAddressRL
        /// </summary>
        /// <param name="addressRL"></param>
        public AddressBL(IAddressRL addressRL)
        {
            this.addressRL = addressRL;
        }

        public AddressModel AddAddress(AddressModel address, int userId)
        {
            try
            {
                return addressRL.AddAddress(address, userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string DeleteAddress(int addressId)
        {
            try
            {
                return addressRL.DeleteAddress(addressId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public AddressResponse GetAddressById(int userId, int addressId)
        {
            try
            {
                return addressRL.GetAddressById(userId, addressId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IList<AddressResponse> GetAddressDetails(int userId, int typeId)
        {
            try
            {
                return addressRL.GetAddressDetails(userId, typeId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IList<AddressResponse> GetAllAddressDetails(int userId)
        {
            try
            {
                return addressRL.GetAllAddressDetails(userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string UpdateAddress(AddressResponse address)
        {
            try
            {
                return addressRL.UpdateAddress(address);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
