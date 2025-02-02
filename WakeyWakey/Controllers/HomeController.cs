﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WakeyWakey.Models;
using WakeyWakey.Services;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Text.RegularExpressions;


namespace WakeyWakey.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserApiService _userService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IUserApiService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            // Fetch the login validation result from the server
            var loginResult = await _userService.ValidateLogin(username, password);

            if (!loginResult.IsValid || !loginResult.UserId.HasValue)
            {
                if (!loginResult.UserId.HasValue)
                {
                    _logger.LogWarning($"Failed login attempt for non-existent user: {username}");
                }
                else
                {
                    _logger.LogWarning($"Failed login attempt for user: {username} due to incorrect password.");
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View();
            }

            // Successful login
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.NameIdentifier, loginResult.UserId.Value.ToString())
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Dashboard");
        }

        [HttpPost]
        public async Task<IActionResult> Register(string username, string password, string email)
        {
            var emailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
            var usernameRegex = new Regex(@"^[A-Za-z][A-Za-z0-9_]*$");

            if (!emailRegex.IsMatch(email))
            {
                ModelState.AddModelError("email", "Invalid email format.");
                return View();
            }

            if (!usernameRegex.IsMatch(username))
            {
                ModelState.AddModelError("username", "Username must start with a letter and can only contain letters, numbers, and underscores.");
                return View();
            }
            
            try {
                await _userService.ValidateRegister(username, email, password);
            } catch (Exception e)
            {
                _logger.LogError(e.Message);
                ModelState.AddModelError(string.Empty, "Invalid register attempt.");
                return View();
            }

            return RedirectToAction("Login", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
    }
}
