using FluentResults;
using IdentityServer4.Events;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public AccountsController(IAuthService authService)
        {
            _authService = authService;
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
}
