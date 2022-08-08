using DomainLayer.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.AddToCart
{
    public class CartDetails
    {
        public int id { get; set; }
        public int productId { get; set; }
        public ProductsModel product { get; set; }
        public int? quantity { get; set; }
        public int? price { get; set; }
    }
}
