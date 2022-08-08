using BusinesLogic.Interface;
using DomainLayer.Product;
using Repository;
using RepositoryLayer;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinesLogic
{
    public class ProductCatagory : IProductCatagory
    {

        ProductDbContext _Context;
        IGenericRepositoryOperation<ProductsModel> _repo;
        public ProductCatagory(IGenericRepositoryOperation<ProductsModel> repo)
        {
            _repo = repo;
        }

        public void Create(ProductsModel entity)
        {
            _repo.Add(entity);
            _repo.Save();
        }


        public void Delete(ProductsModel entity)
        {
            entity.isActive = true;
            _repo.Update(entity);
            _repo.Save();
        }

        public ProductsModel Details(int id)
        {
            return _repo.GetById(id);
        }

        public async Task<IEnumerable<ProductsModel>> GetProducts()
        {
            var result = await _repo.GetAll(n1 => n1.specification);
            return result.Where(x=>x.isActive.Equals(false));
        }
        public async Task<IEnumerable<ProductsModel>> Search(string name)
        {
            var data = _repo.GetAll().Where(x => x.productName.ToLower().StartsWith(name.ToLower()) && x.isActive.Equals(false));
            return data;
        }

        public void Save()
        {
            _repo.Save();
        }

        public void Update(ProductsModel entity)
        {
            _repo.Update(entity);
            _repo.Save();
        }
        public async Task<IEnumerable<ProductsModel>> FilterByBrand(string name)
        {
            var data = _repo.GetAll(n1 => n1.specification).Result.Where(c => c.productModel.ToLower().Equals(name) && c.isActive.Equals(false));
            return data.OrderBy(c => c.productModel);
        }

        public async Task<IEnumerable<ProductsModel>> SortByPriceAscending()
        {
            return _repo.GetAll(n1 => n1.specification).Result.OrderBy(c => c.productPrice).Where(x => x.isActive.Equals(false));
        }

        public async Task<IEnumerable<ProductsModel>> SortByPriceDescending()
        {
            return _repo.GetAll(n1 => n1.specification).Result.OrderByDescending(c => c.productPrice).Where(x => x.isActive.Equals(false));
        }
    }
}

