using DomainLayer;
using Microsoft.Extensions.Configuration;
using RepositoryLayer;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogic.Orders
{
    public class AddressOperations : IAddressOperations
    {
        private readonly IGenericRepositoryOperation<Address> _userAddress;
        public AddressOperations(IGenericRepositoryOperation<Address> userAddress)
        {
            _userAddress = userAddress;
        }

        public async Task AddAddress(Address address)
        {
            _userAddress.Add(address);
        }

        public async Task DeleteAddress(Address address)
        {
            _userAddress.Delete(address);
        }

        public async Task<IEnumerable<Address>> GetAddress()
        {
           return _userAddress.GetAll();
        }

        public async Task<Address> GetAddressById(int id)
        {
            //return _userAddress.GetAll().Where(val=> val.id.Equals(id)).FirstOrDefault();
            return _userAddress.GetById(id);
        }

        public async Task UpdateAddress(Address address)
        {
           _userAddress.Update(address);
        }
    }
}
