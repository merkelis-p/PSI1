﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WakeyWakey.Models;
using WakeyWakey.Services;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;


namespace WakeyWakey.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApiService<User> _userService;
        ApiService<Event> _apiService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ApiService<User> userService, ApiService<Event> apiService)
        {
            _logger = logger;
            _userService = userService;
            _apiService = apiService;
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
            using var hmac = new HMACSHA512();

            var newUser = new User
            {
                Username = username,
                Email = email,
                Password = password
            };

            await _userService.AddAsync(newUser);

            // Log the user in after registering
            HttpContext.Session.SetString("User", newUser.Username);
            return RedirectToAction("Index", "Dashboard");
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
