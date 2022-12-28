using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WikipediaDAW.ContextModels;
using WikipediaDAW.Models;

namespace WikipediaDAW.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ArticolController : ControllerBase
    {
        UtilizatorContext _utilizatorContext { get; set; }

        private readonly ILogger<ArticolController> _logger;

        public ArticolController(UtilizatorContext utilizatorContext)
        {
            _utilizatorContext = utilizatorContext;
        }

        [HttpGet]
        public IQueryable<Articol> Get()
        {
            DbSet<Articol> articols = _utilizatorContext.articole;
            return articols;
        }
    }
}
