using DomainLayer.AddToCart;
using RepositoryLayer.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BussinessLogic
{
    public interface ICartOperations : IGenericRepositoryOperation<Cart>
    {
        Task CartAdd(Cart data);
        Task CartDelete(Cart entity);
        Task<IEnumerable<Cart>> CartData();
        Task CartEdit(Cart entity);
        Cart CartGetById(int Id);

    }
}