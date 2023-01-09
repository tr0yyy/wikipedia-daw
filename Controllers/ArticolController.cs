using FluentResults;
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
        public IEnumerable<ArticolCreate> Get()
        {
            DbSet<Articol> articols = _utilizatorContext.articole;
            IEnumerable<ArticolCreate> first50 = articols.OrderByDescending(u => u.Data_adaugarii)
                .Select(x => new ArticolCreate(x.Domeniu, x.Titlu, x.Continut, x.Autor.UserName, x.Protejat, x.Data_adaugarii))
                .Take(50);
            return first50;
        }

        [HttpGet("all")]
        public IQueryable<ArticolCreate> GetAll()
        {
            DbSet<Articol> articols = _utilizatorContext.articole;
            return articols.Select(x => new ArticolCreate(x.Domeniu, x.Titlu, x.Continut, x.Autor.UserName, x.Protejat, x.Data_adaugarii));
        }

        [HttpGet("{title}")]
        public async Task<ArticolCreate> GetArticol(string title)
        {
            Console.WriteLine("testget");
            DbSet<Articol> articols = _utilizatorContext.articole;
            title = title.ToLower();
            var result = await articols.Where(articol => articol.Titlu.ToLower() == title)
                .Select(x => new ArticolCreate(x.Domeniu, x.Titlu, x.Continut, x.Autor.UserName, x.Protejat, x.Data_adaugarii))
                .FirstAsync();
            return result;
        }

        [HttpPost("create")]
        public async Task<Result<string>> CreateArticol([FromBody] ArticolCreate model)
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

                return Result.Ok("Succesfully updated!");
            }

            else return Result.Fail("Error!");

        }

        [HttpPost("update-articol")]
        public async Task<Result<string>> UpdateArticol([FromBody] ArticolCreate model)
        {
            DbSet<Articol> articols = _utilizatorContext.articole;
            var articol = await articols.Where(art => art.Titlu == model.Titlu).FirstAsync();
            if (articol != null)
            {
                articol.Continut = model.Continut;
                articol.Protejat = model.Protejat;
                articols.Update(articol);
                await _utilizatorContext.SaveChangesAsync();
                return Result.Ok("Succesfully updated!");
            }
            return Result.Fail("Error!");
        }
    }

}
