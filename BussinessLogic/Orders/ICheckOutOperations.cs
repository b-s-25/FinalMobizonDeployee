using DomainLayer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BussinessLogic.Orders
{
    public interface ICheckOutOperations
    {
        Task<IEnumerable<UserCheckOut>> GetCheckOut();
        Task<UserCheckOut> GetCheckOutById(int id);
        Task AddCheckOut(UserCheckOut userCheckOut);
        Task UpdateCheckOut(UserCheckOut userCheckOut);
        Task DeleteCheckOut(UserCheckOut userCheckOut);
    }
}