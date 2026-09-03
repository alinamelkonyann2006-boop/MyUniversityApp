using Microsoft.AspNetCore.Mvc;

namespace MyUniversityApp.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}