using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WikipediaDAW.ContextModels;
using WikipediaDAW.Models;

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
        public IQueryable<Articol> Get()
        {
            Console.WriteLine("test");
            DbSet<Articol> articols = _utilizatorContext.articole;
            return articols;
        }
    }
}
