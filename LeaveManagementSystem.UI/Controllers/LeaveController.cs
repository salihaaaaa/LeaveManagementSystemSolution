using LeaveManagementSystem.Core.Domain.Entities;
using LeaveManagementSystem.Core.Domain.IdentityEntities;
using LeaveManagementSystem.Core.Domain.RepositoryContracts;
using LeaveManagementSystem.Core.DTO;
using LeaveManagementSystem.Core.Enums;
using LeaveManagementSystem.Core.ServiceContracts;
using LeaveManagementSystem.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace LeaveManagementSystem.UI.Controllers
{
    [Route("[controller]/[action]")]
    public class LeaveController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILeaveTypesGetterService _leaveTypesGetterService;
        private readonly ILeavesAdderService _leavesAdderService;
        private readonly ILeavesGetterService _leavesGetterService;
        private readonly ILeavesUpdaterService _leavesUpdaterService;
        private readonly ILeavesDeleterService _leavesDeleterService;

        public LeaveController(UserManager<ApplicationUser> userManager, ILeaveTypesGetterService leaveTypesGetterService, ILeavesAdderService leavesAdderService, ILeavesGetterService leavesGetterService, ILeavesUpdaterService leavesUpdaterService, ILeavesDeleterService leavesDeleterService)
        {
            _userManager = userManager;
            _leaveTypesGetterService = leaveTypesGetterService;
            _leavesAdderService = leavesAdderService;
            _leavesGetterService = leavesGetterService;
            _leavesUpdaterService = leavesUpdaterService;
            _leavesDeleterService = leavesDeleterService;
        }

        [HttpGet]
        public IActionResult Index()
        { 
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            List<LeaveTypeResponse> leaveTypes = await _leaveTypesGetterService.GetAllLeaveTypes();

            ViewBag.LeaveTypes = leaveTypes.Select(temp =>
            new SelectListItem()
            {
                Text = temp.LeaveTypeName,
                Value = temp.LeaveTypeID.ToString()
            });

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(LeaveAddRequest leaveAddRequest)
        {
            if (!ModelState.IsValid)
            {
                List<LeaveTypeResponse> leaveTypes = await _leaveTypesGetterService.GetAllLeaveTypes();

                ViewBag.LeaveTypes = leaveTypes.Select(temp =>
                new SelectListItem()
                {
                    Text = temp.LeaveTypeName,
                    Value = temp.LeaveTypeID.ToString()
                });

                ViewBag.Errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                .SelectMany(e => e.ErrorMessage);
                return View();
            }

            LeaveResponse leaveResponse = await _leavesAdderService.AddLeave(leaveAddRequest);
            return RedirectToAction(nameof(LeaveController.Index), "Leave");
        }
    }
}
