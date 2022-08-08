using DomainLayer;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogic.Orders.Admin
{
   public  interface IOrderDetailsOperations : IGenericRepositoryOperation<UserCheckOut>
    {
        Task<IEnumerable<UserCheckOut>> GetAll();
        Task Add(UserCheckOut data);
        Task Edit(UserCheckOut entity);
        Task Delete(UserCheckOut entity);
    }
}
