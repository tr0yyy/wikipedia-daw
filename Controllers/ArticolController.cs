using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WikipediaDAW.ContextModels;
using WikipediaDAW.Models;
using WikipediaDAW.RequestModels;

namespace WikipediaDAW.Controllers
{
    [ApiController]
    [Authorize(Roles = Roles.Admin)]
    [Route("api/[controller]")]
    public class ArticolController : ControllerBase
    {
        UtilizatorContext _utilizatorContext { get; set; }

        private readonly ILogger<ArticolController> _logger;

        public ArticolController(UtilizatorContext utilizatorContext)
        {
            _utilizatorContext = utilizatorContext;
        }

        [HttpGet("first50")]
        public IEnumerable<Articol> Get()
        {
            DbSet<Articol> articols = _utilizatorContext.articole;
            IEnumerable<Articol> first50 = articols.OrderByDescending(u => u.Data_adaugarii).Take(50);
            return articols;
        }

        [HttpGet("all")]
        public IQueryable<Articol> GetAll()
        {
            DbSet<Articol> articols = _utilizatorContext.articole;
            return articols;
        }

        [HttpGet("{title}")]
        public async Task<Articol> GetArticol(string title)
        {
            DbSet<Articol> articols = _utilizatorContext.articole;
            title = title.ToLower();
            var result = await articols.Where(articol => articol.Titlu.ToLower() == title)
                .FirstAsync();
            return result;
        }

        [HttpPost("create/{title}")]
        public async Task<Articol?> CreateArticol([FromBody] ArticolCreate model)
        {
            DbSet<Articol> articols = _utilizatorContext.articole;
            var user = await _utilizatorContext.Users.Where(user => user.UserName == model.User)
                .FirstAsync();

            Articol articol = new()
            {
                Domeniu = model.Domeniu,
                Titlu = model.Titlu,
                Autor = (User)user,
                Data_adaugarii = DateTime.UtcNow,
                Continut = model.Continut,
                Protejat = model.Protejat
            };

            var result = await articols.AddAsync(articol);

            if (result != null)
            {
                await _utilizatorContext.SaveChangesAsync();

                return articol;
            }

            else return null;

        }

        [HttpPatch("update/{title}")]
        public async Task<Articol?> UpdateArticol([FromBody] ArticolCreate model)
        {
            DbSet<Articol> articols = _utilizatorContext.articole;
            var articol = await articols.Where(art => art.Titlu == model.Titlu).FirstAsync();
            if (articol != null)
            {
                articol.Continut = model.Continut;
                articol.Protejat = model.Protejat;
                articols.Update(articol);
                await _utilizatorContext.SaveChangesAsync();
                return articol;
            }
            return null;
        }
    }
}
