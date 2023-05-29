using LeaveManagementSystem.Core.Domain.IdentityEntities;
using LeaveManagementSystem.Core.DTO;
using LeaveManagementSystem.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace LeaveManagementSystem.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [Route("Admin/[controller]/[action]")]
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
            List<RoleOptions> role = Enum.GetValues(typeof(RoleOptions)).Cast<RoleOptions>().ToList();

            ViewBag.Roles = role.Select(temp =>
            new SelectListItem()
            {
                Text = temp.ToString(),
                Value = temp.ToString()
            });

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
                if (registerDTO.Role == RoleOptions.Admin)
                {
                    //Create 'Admin' role
                    if (await _roleManager.FindByNameAsync(RoleOptions.Admin.ToString()) == null)
                    {
                        ApplicationRole applicationRole = new ApplicationRole()
                        {
                            Name = RoleOptions.Admin.ToString()
                        };

                        await _roleManager.CreateAsync(applicationRole);
                    }

                    //Add the new user into 'Admin' role
                    await _userManager.AddToRoleAsync(user, RoleOptions.Admin.ToString());

                    //Sign in
                    //await _signInManager.SignInAsync(user, false);
                }
                else
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

                return RedirectToAction(nameof(UserController.Index), "User", new
                {
                    Area = "Admin"
                });
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
    }
}