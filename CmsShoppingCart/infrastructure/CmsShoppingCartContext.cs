using CmsShoppingCart.Models;
using Microsoft.EntityFrameworkCore;

namespace CmsShoppingCart.infrastructure
{
    public class CmsShoppingCartContext :DbContext
     
    {
        public CmsShoppingCartContext(DbContextOptions<CmsShoppingCartContext> options)
            : base(options) 
        { 
        }

        public DbSet<Page> Pages { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

    }
}
