using Microsoft.EntityFrameworkCore;
using System.Drawing;
using WikipediaDAW.Models;

namespace WikipediaDAW.ContextModels
{
    public class UtilizatorContext : DbContext
    {
        public DbSet<Utilizator> utilizatori { get; set; }
        public DbSet<Articol> articole { get; set; }
        public DbSet<Istoric_editare> istorici_editare { get; set; }
        public DbSet<Istoric_admin> istorici_admin { get; set; }


        public UtilizatorContext(DbContextOptions options) : base(options)
        {
        }

    }
}
