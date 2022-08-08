using DomainLayer;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogic.Orders
{
    public class CheckOutOperations : ICheckOutOperations
    {
        private readonly IGenericRepositoryOperation<UserCheckOut> _checkOutOperations;
        public CheckOutOperations(IGenericRepositoryOperation<UserCheckOut> checkOutOperations)
        {
            _checkOutOperations = checkOutOperations;
        }

        public async Task AddCheckOut(UserCheckOut userCheckOut)
        {
            _checkOutOperations.Add(userCheckOut);
        }

        public async Task DeleteCheckOut(UserCheckOut userCheckOut)
        {
            _checkOutOperations.Delete(userCheckOut);
        }

        public async Task<IEnumerable<UserCheckOut>> GetCheckOut()
        {
            return _checkOutOperations.GetAll();
        }

        public async Task<UserCheckOut> GetCheckOutById(int id)
        {
            return _checkOutOperations.GetById(id);
        }

        public async Task UpdateCheckOut(UserCheckOut userCheckOut)
        {
            _checkOutOperations.Update(userCheckOut);
        }
    }    
}

