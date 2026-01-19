using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Web.Controllers;

public class AuthController : Controller
{
    private readonly IAuthService authService;

    public AuthController(IAuthService authService)
    {
        this.authService = authService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        LoginRequestDTO loginRequestDTO = new();
        return View(loginRequestDTO);  
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Logout()
    {
        return View();
    }

}
