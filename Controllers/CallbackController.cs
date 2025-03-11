using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using net.Models;

namespace net.Controllers;

public class CallbackController : Controller
{
    private readonly ILogger<CallbackController> _logger;
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public CallbackController(ILogger<CallbackController> logger, HttpClient httpClient, IConfiguration configuration)
    {
        _logger = logger;
        _httpClient = httpClient;
        _baseUrl = configuration["ApiSettings:BaseUrl"];
    }

    [HttpGet]
    public IActionResult Index([FromQuery] string code)
    {
        ViewData["Code"] = code;
        _logger.LogInformation($"Callback recibido con código: {code}");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ProcessCode([FromBody] CallbackRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(
                $"{_baseUrl}/callback",
                new { code = request.Code }
            );

            var result = await response.Content.ReadAsStringAsync();
            _logger.LogInformation($"Procesando código de autenticación: {request.Code}");
            
            return Content(result, "application/json");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al procesar el código de autenticación");
            return Json(new { ok = false, message = "Error en el servidor" });
        }
    }
} 