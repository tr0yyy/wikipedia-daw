using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using WikipediaDAW.Models;

namespace WikipediaDAW.ContextModels
{
    public class UtilizatorContext : IdentityDbContext
    {
        public UtilizatorContext(DbContextOptions<UtilizatorContext> options) : base(options)
        {
        }

        public DbSet<Articol> articole { get; set; }

        public DbSet<Istoric_Articol> istoric_articol { get;set; }
     

    }
}
