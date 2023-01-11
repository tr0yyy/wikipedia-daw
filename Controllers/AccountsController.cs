using FluentResults;
using IdentityServer4.Events;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WikipediaDAW.ContextModels;
using WikipediaDAW.Models;
using WikipediaDAW.RequestModels;
using WikipediaDAW.Services;

namespace WikipediaDAW.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {

        private readonly IAuthService _authService;
        private readonly UserManager<User> _userManager;

        UtilizatorContext _utilizatorContext { get; set; }

        public AccountsController(IAuthService authService, UserManager<User> userManager)
        {
            _userManager = userManager;
            _authService = authService;
        }

        [HttpGet("users/all")]
        public async Task<List<AccountInfo>> GetAll()
        {
            var users = _userManager.Users.ToList();
            var result = new List<AccountInfo>();

            foreach(User user in users)
            {
                var rol = await _userManager.GetRolesAsync(user);
                result.Add(new AccountInfo(rol, user.UserName));
            }
            return result;
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost("update-roles")]
        public async Task<IActionResult> updateRoles([FromBody] RolesRequest model)
        {
            var user = await _userManager.Users.FirstAsync(u => u.UserName == model.username);
            if(user != null)
            {
                var currentRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                await _userManager.AddToRolesAsync(user, model.roles);
                return Ok(Result.Ok());
            }
            return BadRequest(Result.Fail("Cannot update roles!"));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)

        {
            var response = await _authService.Register(model);

            if(!response.IsSuccess)
            {
                return BadRequest(exportResponse(response));
            }
            return Ok(exportResponse(response));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            var response = await _authService.Login(model);

            if (!response.IsSuccess)
            {
                return BadRequest(exportResponse(response));
            }
            return Ok(exportResponse(response));
        }

        private LoginResponse<string> exportResponse(Result<string> response)
        {
            return new LoginResponse<string>(response.IsSuccess, response.Errors?.Select(error => error.Message), response.Value);
        }
    }
    public class AccountInfo
    {
        public IList<String> roles { get; set; }
        public string username { get; set; }
        public AccountInfo(IList<String> roles, string username)
        {
            this.roles = roles;
            this.username = username;
        }
    }

    public class RolesRequest
    {
        public IList<string> roles { get; set; }

        public string username { get; set; }
    }

}
