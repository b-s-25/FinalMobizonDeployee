using DomainLayer.AddToCart;
using RepositoryLayer.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BussinessLogic
{
    public interface ICartDetailsOperations : IGenericRepositoryOperation<CartDetails>
    {
        Task CartDetailsAdd(CartDetails data);
        Task CartDetailsDelete(CartDetails entity);
        IEnumerable<CartDetails> CartDetailsData();
        Task CartDetailsEdit(CartDetails entity);
        CartDetails CartDetailsGetById(int Id);

    }
}