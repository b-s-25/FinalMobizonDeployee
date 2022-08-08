using DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogic.Orders
{
    public interface IAddressOperations
    {
        Task<IEnumerable<Address>> GetAddress();
        Task<Address> GetAddressById(int id);
        Task AddAddress(Address address);
        Task UpdateAddress(Address address);
        Task DeleteAddress(Address address);
    }
}
