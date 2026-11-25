using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TemperaturStyring.Models;

namespace TemperaturStyring.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly SignInManager<ApplicationUser> signInManager;
    private readonly UserManager<ApplicationUser> userManager;

    public AuthController(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager)
    {
        this.signInManager = signInManager;
        this.userManager = userManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        ApplicationUser user = new()
        {
            UserName = request.Email,
            Email = request.Email
        };

        IdentityResult result =
            await userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors.Select(error => error.Description));
        }

        return Ok("User created.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        Microsoft.AspNetCore.Identity.SignInResult result =
            await signInManager.PasswordSignInAsync(
                request.Email,
                request.Password,
                isPersistent: false,
                lockoutOnFailure: true);

        if (!result.Succeeded)
        {
            return Unauthorized("Invalid email or password.");
        }

        return Ok();
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return NoContent();
    }
}

public record RegisterRequest(string Email, string Password);

public record LoginRequest(string Email, string Password);