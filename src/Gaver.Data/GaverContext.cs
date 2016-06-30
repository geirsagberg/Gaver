using Gaver.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gaver.Data
{
    public class GaverContext : DbContext
    {
        public GaverContext()
        {
            
        }

        public DbSet<Wish> MyProperty { get; set; }
    }
}
