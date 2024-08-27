using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Models;
using System.Security.Cryptography;
using System.Text;
using System;
using salesWebMvc.Models;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Services;

public class AccountController : Controller
{
    private readonly SalesWebMvcContext _context;
    private readonly UserService _userService;
    public AccountController(SalesWebMvcContext context, UserService userService)
    {
        _context = context;
        _userService = userService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        var user = await _userService.FindByEmailAsync(email);

        if (user != null && VerifyPassword(password, user.PasswordHash))
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.NameIdentifier, user.Name) // Adiciona o nome do usuário
        };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            // Redirecionar para a página de vendedores (ou outra página desejada)
            return RedirectToAction("Index", "Sellers");
        }

        ViewBag.ErrorMessage = "Credenciais inválidas!";
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(UserRegisterModel model)
    {
        if (ModelState.IsValid)
        {
            // Mapear o modelo de registro para a entidade de usuário
            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                Cpf = model.Cpf,
                Phone = model.Phone,
                // Supondo que você esteja usando uma função para hashear a senha antes de salvar
                PasswordHash = HashPassword(model.Password)
            };

            // Adicionar o usuário ao contexto e salvar as mudanças
            await _userService.InsertAsync(user);

            // Redirecionar para a página de login, que é a Home/Index
            return RedirectToAction("Index", "Home");
        }

        // Se o modelo for inválido, retornar à view de registro com os erros
        return View(model);
    }


    private string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
    private bool VerifyPassword(string password, string storedHash)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            var enteredHash = Convert.ToBase64String(hashedBytes);

            return enteredHash == storedHash;
        }
    }


}
