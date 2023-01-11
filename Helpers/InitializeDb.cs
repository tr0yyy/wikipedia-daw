using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text;
using WikipediaDAW.Const;
using WikipediaDAW.ContextModels;
using WikipediaDAW.Models;

namespace WikipediaDAW.Helpers
{
    public static class InitializeDb
    {
        public static async Task Run(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var currentData = roleManager.Roles.Select(x => x.Name).ToList();
            if (!currentData.Contains(Roles.Admin))
                await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
            if (!currentData.Contains(Roles.Moderator))
                await roleManager.CreateAsync(new IdentityRole(Roles.Moderator));
            if (!currentData.Contains(Roles.User))
                await roleManager.CreateAsync(new IdentityRole(Roles.User));

            var context = serviceProvider.GetRequiredService<UtilizatorContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            var admin = await context.Users.FirstOrDefaultAsync(user => user.UserName == "admin");

            if (admin == null)
            {
                admin = new User
                {
                    UserName = "admin",
                    Email = "admin@a.a"
                };

                await userManager.CreateAsync((User)admin, "!@#4QWEr");

                await userManager.AddToRoleAsync((User)admin, Roles.Admin);
            }

            foreach(var domain in DomainsEnum.RolesEnumValues)
            {
                var domainDb = await context.domeniu.FirstOrDefaultAsync(domeniu => domeniu.Name == domain);
                if(domainDb == null)
                    await context.domeniu.AddAsync(new Domeniu() { Name = domain });
            }

            var articole = context.articole.Select(x => x.Titlu).ToList();

            var geografie = await context.domeniu.FirstAsync(x => x.Name == "Geografie");
            var arta = await context.domeniu.FirstAsync(x => x.Name == "Arta");

            if(!articole.Contains("Florența"))
            {
                var content = Encoding.Default.GetString(Properties.Resources.Florenta);
                await context.articole.AddAsync(new Articol()
                {
                    Titlu = "Florența",
                    Continut = content,
                    Autor = (User)admin,
                    Protejat = false,
                    Data_adaugarii = DateTime.UtcNow,
                    Domeniu= geografie
                });
            }
            if(!articole.Contains("Mona Lisa"))
            {
                var content = Encoding.Default.GetString(Properties.Resources.Mona_Lisa);
                await context.articole.AddAsync(new Articol()
                {
                    Titlu = "Mona Lisa",
                    Continut = content,
                    Autor = (User)admin,
                    Protejat = false,
                    Data_adaugarii = DateTime.UtcNow,
                    Domeniu = arta
                });
            }
            if(!articole.Contains("Sfumato"))
            {
                var content = Encoding.Default.GetString(Properties.Resources.Sfumato);
                await context.articole.AddAsync(new Articol()
                {
                    Titlu = "Sfumato",
                    Continut = content,
                    Autor = (User)admin,
                    Protejat = false,
                    Data_adaugarii = DateTime.UtcNow,
                    Domeniu = arta
                });
            }
            await context.SaveChangesAsync();
        }
    }
}
