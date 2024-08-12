using GiyimSitesi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;
using GiyimSitesi.Context.Entity;


namespace GiyimSitesi.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<Kullanici> _userManager;
        private SignInManager<Kullanici> _signInManager;
        private RoleManager<IdentityRole> _roleManager;


        public AccountController(UserManager<Kullanici> userManager, SignInManager<Kullanici> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                ModelState.AddModelError("", "Kullanıcı Bulunamadı");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);

            if (result.Succeeded)
            {

                return Redirect("~/");
            }

            return View(model);
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Kullanici user = new Kullanici
            {
                Name = model.FirstName,
                Surname = model.LastName,
                Email = model.Email,
                UserName = model.UserName,


            };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                ////Cart objesi olustur
                ///Like objesi olustur



                //generite token
                return RedirectToAction("login", "account");
            }
            ModelState.AddModelError("", "Bilinmeyen Bir Hata Oluştu Tekrar Deneyiniz");
            return View(model);
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return Redirect("~/");
        }
        public IActionResult AccesDenied()
        {
            return View();
        }
    }
}
