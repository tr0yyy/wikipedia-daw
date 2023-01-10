using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FluentResults;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using WikipediaDAW.Models;
using WikipediaDAW.RequestModels;

namespace WikipediaDAW.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;

    public AuthService(UserManager<User> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<Result<string>> Register(RegisterRequest model)
    {
        var userByEmail = await _userManager.FindByEmailAsync(model.Email);
        var userByUsername = await _userManager.FindByNameAsync(model.UserName);
        if (userByEmail is not null || userByUsername is not null)
        {
            return Result.Fail($"User with email {model.Email} or username {model.UserName} already exists.");
        }

        User user = new()
        {
            Email = model.Email,
            UserName = model.UserName,
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            return Result.Fail($"Unable to register user {model.UserName} errors: {GetErrorsText(result.Errors)}");
        }

        await _userManager.AddToRoleAsync(user, Roles.User);

        return await Login(new LoginRequest { UserName = model.UserName, Password = model.Password });
    }

    public async Task<Result<string>> Login(LoginRequest request)
    {
        var user = await _userManager.FindByNameAsync(request.UserName);

        if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            return Result.Fail($"Unable to authenticate user {request.UserName}. Check Password!");
        }

        var authClaims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var userRoles = await _userManager.GetRolesAsync(user);

        authClaims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));

        var token = GetToken(authClaims);

        return Result.Ok(new JwtSecurityTokenHandler().WriteToken(token));
    }

    private JwtSecurityToken GetToken(IEnumerable<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddHours(3),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

        return token;
    }

    public static string getUserFromJwt(string JwtToken)
    {
        var token = new JwtSecurityTokenHandler().ReadJwtToken(JwtToken.Split(" ")[1]);
        return token.Claims.First(claim => claim.Type == ClaimTypes.Name).Value;
    }

    private string GetErrorsText(IEnumerable<IdentityError> errors)
    {
        return string.Join(", ", errors.Select(error => error.Description).ToArray());
    }
}