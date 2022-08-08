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
    public class CartDetailsOperations : GenericRepositoryOperation<CartDetails>,ICartDetailsOperations
    {
        private readonly IGenericRepositoryOperation<CartDetails> _repo;
        private readonly ProductDbContext _dbContext;

        public CartDetailsOperations(ProductDbContext dbContext, IGenericRepositoryOperation<CartDetails> repo) : base(dbContext)
        {
            _dbContext = dbContext;
            _repo = repo;
        }
        public async Task CartDetailsAdd(CartDetails data)
        {
            _repo.Add(data);
            _repo.Save();
        }

        public async Task CartDetailsDelete(CartDetails entity)
        {
            _repo.Delete(entity);
            _repo.Save();
        }

        public IEnumerable<CartDetails> CartDetailsData()
        {
            return _repo.GetAll();
        }

        public async Task CartDetailsEdit(CartDetails entity)
        {
            _repo.Update(entity);
            _repo.Save();
        }

        public CartDetails CartDetailsGetById(int Id)
        {
            return _repo.GetById(Id);
        }

    }
}
