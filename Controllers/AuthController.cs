using Microsoft.AspNetCore.Mvc;
using net.Models;
using System.Text.Json;

namespace net.Controllers;

public class AuthController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthController> _logger;
    private readonly string _baseUrl;

    public AuthController(HttpClient httpClient, IConfiguration configuration, ILogger<AuthController> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
        _baseUrl = configuration["ApiSettings:BaseUrl"];
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] RegisterRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(
                $"{_baseUrl}/login",
                new { email = request.Email, password = request.Password }
            );

            var responseContent = await response.Content.ReadAsStringAsync();
            _logger.LogInformation($"Intento de login para {request.Email}");
            
            if (string.IsNullOrEmpty(responseContent))
            {
                return Json(new { ok = false, message = "Error en el servidor" });
            }

            return Content(responseContent, "application/json");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en el login");
            return Json(new { ok = false, message = "Error en el servidor" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(
                $"{_baseUrl}/register",
                new { email = request.Email, password = request.Password }
            );

            var result = await response.Content.ReadAsStringAsync();
            _logger.LogInformation($"Registro intentado para {request.Email}");
            
            return Content(result, "application/json");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en el registro");
            return Json(new { ok = false, message = "Error en el servidor" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> ResendCode([FromBody] ResendCodeRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(
                $"{_baseUrl}/resend",
                new { email = request.Email }
            );

            var result = await response.Content.ReadAsStringAsync();
            _logger.LogInformation($"Reenvío de código solicitado para {request.Email}");
            
            return Content(result, "application/json");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al reenviar el código");
            return Json(new { ok = false, message = "Error en el servidor" });
        }
    }

    public IActionResult Confirm()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ConfirmCode([FromBody] ConfirmCodeRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(
                $"{_baseUrl}/confirm",
                new { email = request.Email, code = request.Code }
            );

            var result = await response.Content.ReadAsStringAsync();
            _logger.LogInformation($"Confirmación intentada para {request.Email}");
            
            return Content(result, "application/json");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en la confirmación");
            return Json(new { ok = false, message = "Error en el servidor" });
        }
    }

    public IActionResult Restricted()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Callback([FromQuery] string code)
    {
        try 
        {
            _logger.LogInformation($"Iniciando callback con código: {code}");
            
            var response = await _httpClient.PostAsJsonAsync(
                $"{_baseUrl}/callback",
                new { code = code }
            );

            var result = await response.Content.ReadAsStringAsync();
            _logger.LogInformation($"Respuesta del servidor: {result}");

            // Deserializamos la respuesta
            var jsonResponse = JsonSerializer.Deserialize<JsonElement>(result);
            var ok = jsonResponse.GetProperty("ok").GetBoolean();

            if (ok)
            {
                // Si es exitoso, pasamos los tokens a la vista
                return View(new
                {
                    ok = true,
                    idToken = jsonResponse.GetProperty("id_token").GetString(),
                    accessToken = jsonResponse.GetProperty("access_token").GetString(),
                    refreshToken = jsonResponse.GetProperty("refresh_token").GetString()
                });
            }
            else
            {
                // Si hay error, redirigimos directamente a restricted
                return RedirectToAction("Restricted", "Home");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al procesar el callback");
            return RedirectToAction("Restricted", "Home");
        }
    }

    [HttpGet]
    public async Task<IActionResult> GoogleAuth()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/google");
            var result = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("Solicitando URL de autenticación de Google");
            
            return Content(result, "application/json");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener la URL de autenticación de Google");
            return Json(new { ok = false, message = "Error en el servidor" });
        }
    }
} 