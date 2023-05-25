using LeaveManagementSystem.Core.Domain.IdentityEntities;
using LeaveManagementSystem.Core.DTO;
using LeaveManagementSystem.Core.Enums;
using LeaveManagementSystem.UI.Areas.Admin.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace LeaveManagementSystem.UI.Controllers
{
    [Route("[controller]/[action]")]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Register()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values
                    .SelectMany(value => value.Errors)
                    .SelectMany(error => error.ErrorMessage);
                return View(registerDTO);
            }

            ApplicationUser user = new ApplicationUser()
            {
                Email = registerDTO.Email,
                Name = registerDTO.Name,
                UserName = registerDTO.Email,
                PhoneNumber = registerDTO.Phone
            };

            IdentityResult result = await _userManager.CreateAsync(user, registerDTO.Password);

            if (result.Succeeded)
            {
                //Check user role
                if (registerDTO.Role == RoleOptions.Employee)
                {
                    //Create 'Employee' role
                    if (await _roleManager.FindByNameAsync(RoleOptions.Employee.ToString()) == null)
                    {
                        ApplicationRole applicationRole = new ApplicationRole()
                        {
                            Name = RoleOptions.Employee.ToString()
                        };

                        await _roleManager.CreateAsync(applicationRole);
                    }

                    //Add the new user into 'Employee' role
                    await _userManager.AddToRoleAsync(user, RoleOptions.Employee.ToString());
                }

                //Sign in
                await _signInManager.SignInAsync(user, false);

                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            else
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("Register", error.Description);
                }

                return View(registerDTO);
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginDTO, string? ReturnUrl)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values
                    .SelectMany(value => value.Errors)
                    .SelectMany(error => error.ErrorMessage);

                return View(loginDTO);
            }

            var result = await _signInManager.PasswordSignInAsync(loginDTO.Email, loginDTO.Password, false, false);

            if (result.Succeeded)
            {
                ApplicationUser user = await _userManager.FindByEmailAsync(loginDTO.Email);

                if (user != null)
                {
                    //If user is 'Admin'
                    if (await _userManager.IsInRoleAsync(user, RoleOptions.Admin.ToString()))
                    {
                        return RedirectToAction(nameof(HomeController.Index), "Home", new
                        {
                            area = "Admin"
                        });
                    }
                    //If user is 'Employee'
                    else
                    {
                        return RedirectToAction(nameof(HomeController.Index), "Home");
                    }
                }

                //Make successful redirection only if url is the same url domain
                if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                {
                    return LocalRedirect(ReturnUrl);
                }
            }

            ModelState.AddModelError("Login", "Invalid email or password");

            return View(loginDTO);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(AccountController.Login), "Account");
        }

        public async Task<IActionResult> IsEmailAlreadyRegistered(string email)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return Json(true); //Email not exist in database
            }
            else
            {
                return Json(false); //Email already exist in database
            }
        }
    }
}
