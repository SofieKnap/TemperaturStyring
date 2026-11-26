using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TemperaturStyring.Controllers;

[Route("frontpage")]
public class FrontPageController : Controller
{
    private readonly IWebHostEnvironment environment;

    public FrontPageController(IWebHostEnvironment environment)
    {
        this.environment = environment;
    }

    [HttpGet]
    public IActionResult Index()
    {
        string filePath = Path.Combine(
            environment.WebRootPath!,
            "FrontPage.html");

        return PhysicalFile(filePath, "text/html");
    }
}