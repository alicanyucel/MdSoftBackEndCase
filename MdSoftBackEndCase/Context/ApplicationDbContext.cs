using MdSoftBackEndCase.Models;
using Microsoft.EntityFrameworkCore;

namespace MdSoftBackEndCase.Context
{
  

    namespace DocumentManagementSystem.Data
    {
        public class AppDbContext : DbContext
        {
            public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

            // Document modelini veritabanı ile ilişkilendiriyoruz
            public DbSet<Document> Documents { get; set; }
        }
    }

}
