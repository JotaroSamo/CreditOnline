using BuissnesLogical.Service;
using LogicalModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BuissnesLogical.Interface;

namespace CreditOnline.Controllers
{
    public class AuthController : Controller
    {
        private readonly IRepository<User> _userService;
        private readonly IUserService _userCheck;
        public AuthController(IRepository<User> userService, IUserService userCheck)
        {
            _userService = userService;
            _userCheck = userCheck;
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
        public async Task<IActionResult> Register(User model)
        {
            try
            {
                if (await _userCheck.NotNullUser(model) == false)
                {
                    return View(model);
                }

                if (await _userCheck.GetCheckAsync(model) == null)
                {
                    await _userService.CreateAsync(model);

                    return RedirectToAction("Index", "Home");
                }



                return View(model);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login(User userAuthDto)
        {
            try
            {
                var user = await _userCheck.FindUser(userAuthDto);
                if (user == null)
                {

                    return RedirectToAction("Register");
                }

                var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
            new Claim(ClaimTypes.Role, user.UserTypes.ToString())
        };

                if (user.UserTypes.ToString() == "Администратор")
                {

                    var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimIdentity));
                    return RedirectToAction("Index", "Admin");
                }


                var userClaimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(userClaimsIdentity));
                return RedirectToAction("UserTools", "User");
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
