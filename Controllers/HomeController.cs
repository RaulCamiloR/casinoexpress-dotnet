using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using net.Models;

namespace net.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Dashboard()
    {
        return View();
    }

    public IActionResult Confirm()
    {
        return View();
    }

    public IActionResult Restricted()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string email, string password)
    {
        // En un caso real, aquí validarías las credenciales
        return Json(new { success = true });
    }

    [HttpPost]
    public IActionResult Register(string email, string password)
    {
        // Imprimir en la consola del servidor
        _logger.LogInformation($"Nuevo registro - Email: {email}, Password: {password}");
        return Json(new { success = true });
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
