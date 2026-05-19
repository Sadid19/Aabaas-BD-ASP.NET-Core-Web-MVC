using BLL.DTOs;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    public class AccountController : Controller
    {
        AccountService accountService;

        public AccountController(AccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string email, string password)
        {
            UserDTO user = accountService.Login(email, password);
            if (user == null)
            {
                ViewBag.Error = "Invalid email or password.";
                return View();
            }

            HttpContext.Session.SetInt32("UserId", user.UserId);
            HttpContext.Session.SetString("UserName", user.Name);
            HttpContext.Session.SetString("UserEmail", user.Email);

            if (accountService.IsAdmin(user.Email))
            {
                HttpContext.Session.SetString("IsAdmin", "true");
                return RedirectToAction("Index", "Admin");
            }

            HttpContext.Session.SetString("IsAdmin", "false");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new UserDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(UserDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = accountService.Register(model);
            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(model);
            }

            HttpContext.Session.SetInt32("UserId", result.User.UserId);
            HttpContext.Session.SetString("UserName", result.User.Name);
            HttpContext.Session.SetString("UserEmail", result.User.Email);
            HttpContext.Session.SetString("IsAdmin", "false");
            TempData["Success"] = result.Message;
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
