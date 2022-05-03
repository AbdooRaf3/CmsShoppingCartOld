using Microsoft.EntityFrameworkCore;

namespace CmsShoppingCart.infrastructure
{
    public class CmsShoppingCartContext :DbContext
     
    {
        public CmsShoppingCartContext(DbContextOptions<CmsShoppingCartContext> options)
            : base(options) 
        { 
        }
    }
}
