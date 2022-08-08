using DomainLayer.AddToCart;
using Repository;
using RepositoryLayer;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogic
{
    public class CartOperations: GenericRepositoryOperation<Cart>, ICartOperations
    {
        private readonly IGenericRepositoryOperation<Cart> _repo;
        private readonly ProductDbContext _dbContext;

        public CartOperations(ProductDbContext dbContext, IGenericRepositoryOperation<Cart> repo) : base(dbContext)
        {
            _dbContext = dbContext;
            _repo = repo;
        }
        public async Task CartAdd(Cart data)
        {
            _repo.Add(data);
            _repo.Save();
        }

        public async Task CartDelete(Cart entity)
        {
            _repo.Delete(entity);
            _repo.Save();
        }

        public async Task<IEnumerable<Cart>> CartData()
        {
            return await _repo.GetAll(x => x.cartDetails);
        }

        public async Task CartEdit(Cart entity)
        {
            _repo.Update(entity);
            _repo.Save();
        }

        public Cart CartGetById(int Id)
        {
            return _repo.GetById(Id);
        }
    }
}
