﻿using FluentResults;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using WikipediaDAW.ContextModels;
using WikipediaDAW.Models;
using WikipediaDAW.RequestModels;
using WikipediaDAW.Services;

namespace WikipediaDAW.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticolController : ControllerBase
    {
        UtilizatorContext _utilizatorContext { get; set; }

        private readonly ILogger<ArticolController> _logger;
        private readonly UserManager<User> _userManager;

        public ArticolController(UtilizatorContext utilizatorContext, UserManager<User> userManager)
        {
            _utilizatorContext = utilizatorContext;
            _userManager = userManager;
        }

        [HttpGet("first50")]
        public IEnumerable<ArticolCreate> Get()
        {
            DbSet<Articol> articols = _utilizatorContext.articole;
            IEnumerable<ArticolCreate> first50 = articols.OrderByDescending(u => u.Data_adaugarii)
                .Select(x => new ArticolCreate(x.Domeniu.Name, x.Titlu, x.Continut, x.Autor.UserName, x.Protejat, x.Data_adaugarii))
                .Take(50);
            return first50;
        }

        [HttpGet("all")]
        public IQueryable<ArticolCreate> GetAll()
        {
            DbSet<Articol> articols = _utilizatorContext.articole;
            return articols.Select(x => new ArticolCreate(x.Domeniu.Name, x.Titlu, x.Continut, x.Autor.UserName, x.Protejat, x.Data_adaugarii));
        }

        [HttpGet("{title}")]
        public async Task<ArticolCreate> GetArticol(string title)
        {
            DbSet<Articol> articols = _utilizatorContext.articole;
            title = title.ToLower();
            var result = await articols.Where(articol => articol.Titlu == title)
                .Select(x => new ArticolCreate(x.Domeniu.Name, x.Titlu, x.Continut, x.Autor.UserName, x.Protejat, x.Data_adaugarii))
                .FirstAsync();
            return result;
        }

        [HttpGet("alltitles/{title}")]
        public IEnumerable<String> GetAllTitles(string title)
        {

            return _utilizatorContext.articole.Where(articol => articol.Titlu.ToLower().Contains(title.ToLower()))
                .Select(x => new String(x.Titlu));
        }

        [HttpGet("alldomains")]
        public IEnumerable<String> GetDomains()
        {
            return _utilizatorContext.domeniu.Select(x => x.Name).Distinct().ToList();
        }

        [HttpGet("articole-domeniu/{domeniu}")]
        public IEnumerable<ArticolCreate> GetArticlesByDomain(string domeniu) 
        {
            return _utilizatorContext.articole.Where(articol => articol.Domeniu.Name.ToLower() == domeniu)
                .Select(x => new ArticolCreate(x.Domeniu.Name, x.Titlu, x.Continut, x.Autor.UserName, x.Protejat, x.Data_adaugarii))
                .ToList();
        }


        [HttpPost("create")]
        public async Task<Result<string>> CreateArticol([FromBody] ArticolCreate model)
        {
            DbSet<Articol> articols = _utilizatorContext.articole;

            var domeniu = await _utilizatorContext.domeniu.Where(d => d.Name == model.Domeniu)
                .FirstAsync();

            Articol articol = new()
            {
                Domeniu = domeniu,
                Titlu = model.Titlu,
                Data_adaugarii = DateTime.UtcNow,
                Continut = model.Continut,
                Protejat = model.Protejat
            };

            if (model.User != null)
            {
                articol.Autor = (User)await _utilizatorContext.Users.Where(user => user.UserName == model.User)
                    .FirstAsync();
            }

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
                if (articol.Continut != model.Continut)
                {
                    _utilizatorContext.istoric_articol.Add(new Istoric_Articol()
                    {
                        articol = articol,
                        continut_vechi = articol.Continut,
                        data_editarii = DateTime.UtcNow,
                    });
                }
                articol.Continut = model.Continut;
                if (!StringValues.IsNullOrEmpty(Request.Headers.Authorization))
                {
                    var username = AuthService.getUserFromJwt(Request.Headers.Authorization);
                    var user = await _utilizatorContext.Users.FirstAsync(u => u.UserName == username);
                    var roles = await _userManager.GetRolesAsync((User)user);
                    if (articol.Autor == user || roles.Contains(Roles.Moderator))
                    {
                        articol.Protejat = model.Protejat;
                    }
                }
                articols.Update(articol);
                await _utilizatorContext.SaveChangesAsync();
                return Result.Ok("Succesfully updated!");
            }
            return Result.Fail("Error!");
 


        }

        [Authorize(Roles = Roles.Moderator)]
        [HttpPost("revert-articol")]
        public async Task<Result<string>> RevertArticol([FromBody] ArticolCreate model)
        {
            var username = AuthService.getUserFromJwt(Request.Headers.Authorization);
            var user = await _utilizatorContext.Users.FirstAsync(u => u.UserName == username);
            var roles = await _userManager.GetRolesAsync((User)user);
            DbSet<Articol> articols = _utilizatorContext.articole;
            var articol = await articols.Where(art => art.Titlu == model.Titlu).FirstAsync();
            if (articol != null)
            {
                var articol_vechi = await _utilizatorContext.istoric_articol.OrderByDescending(u => u.data_editarii).FirstAsync();
                articol.Continut = articol_vechi.continut_vechi;
                articols.Update(articol);
                _utilizatorContext.istoric_articol.Remove(articol_vechi);
                await _utilizatorContext.SaveChangesAsync();
                return Result.Ok("Succesfully updated!");
            }
            return Result.Fail("Error!");
        }
    }

}
