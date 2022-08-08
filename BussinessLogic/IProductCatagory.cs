using DomainLayer.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinesLogic.Interface
{
    public interface IProductCatagory
    {
        Task<IEnumerable<ProductsModel>> FilterByBrand(string name);
        Task<IEnumerable<ProductsModel>> GetProducts();
        void Create(ProductsModel entity);
        void Update(ProductsModel entity);
        void Delete(ProductsModel entity);
        ProductsModel Details(int id);
        Task<IEnumerable<ProductsModel>> Search(string name);
        void Save();
        Task<IEnumerable<ProductsModel>> SortByPriceAscending();
        Task<IEnumerable<ProductsModel>> SortByPriceDescending();
    }
}
