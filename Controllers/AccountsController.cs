using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using WikipediaDAW.ContextModels;
using WikipediaDAW.Models;
using WikipediaDAW.RequestModels;

namespace WikipediaDAW.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {

        private readonly UserManager<User> _userManager;
        private readonly RoleManager<User> _roleManager;

        public AccountsController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)

        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Console.WriteLine(model.UserName);

            var user = new User { UserName = model.UserName, Email = model.Email };

            Console.WriteLine(user.ToString());

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            await _userManager.AddToRoleAsync(user, Roles.User);

            return Ok(new RegisterResponse(user, Roles.User));
        }
    }
}
