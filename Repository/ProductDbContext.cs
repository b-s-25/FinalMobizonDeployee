using DomainLayer;
using DomainLayer.AddToCart;
using DomainLayer.AdminSettings;
using DomainLayer.Product;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ProductDbContext : IdentityDbContext<ApplicationUser>
    {
        public ProductDbContext(DbContextOptions<ProductDbContext>options) : base(options)
        {

        }
        public DbSet<Registration> Register { get; set; }
        public DbSet<ApplicationUser> applicationUsers { get; set; }
        public DbSet<Login> Login { get; set; }
        public DbSet<About> About { get; set; }
        public DbSet<PrivacyAndPolicy> PrivacyAndPolicy { get; set; }
        public DbSet<MasterData> MasterDatas { get; set; }
        public DbSet<ProductsModel> ProductsModel { get; set; }
        public DbSet<UserCheckOut> UserCheckOut { get; set; }
        public DbSet<AdminContact> contact { get; set; }
        public DbSet<Cart> cart { get; set; }
    }
}
