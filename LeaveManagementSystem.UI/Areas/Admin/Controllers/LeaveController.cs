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

namespace LeaveManagementSystem.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [Route("Admin/[controller]/[action]")]
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
        public async Task<IActionResult> Index()
        {
            List<LeaveResponse> leaves = await _leavesGetterService.GetAllLeaves();
            return View(leaves);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            List<ApplicationUser> users = _userManager.Users.ToList();

            ViewBag.Users = users.Select(temp =>
            new SelectListItem()
            {
                Text = temp.Name,
                Value = temp.Id.ToString()
            });

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
                List<ApplicationUser> users = _userManager.Users.ToList();

                ViewBag.Users = users.Select(temp =>
                new SelectListItem()
                {
                    Text = temp.Name,
                    Value = temp.Id.ToString()
                });

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
            return RedirectToAction(nameof(LeaveController.Index), "Leave", new
            {
                Area = "Admin"
            });
        }

        [HttpGet]
        [Route("{leaveID}")]
        public async Task<IActionResult> Edit(Guid? leaveID)
        {
            LeaveResponse? leaveResponse = await _leavesGetterService.GetLeaveByLeaveID(leaveID);

            if (leaveResponse == null)
            {
                return RedirectToAction("Index");
            }

            LeaveUpdateRequest leaveUpdateRequest = leaveResponse.ToLeaveUpdateRequest();

            List<ApplicationUser> users = _userManager.Users.ToList();

            ViewBag.Users = users.Select(temp =>
            new SelectListItem()
            {
                Text = temp.Name,
                Value = temp.Id.ToString()
            });

            List<LeaveTypeResponse> leaveTypes = await _leaveTypesGetterService.GetAllLeaveTypes();

            ViewBag.LeaveTypes = leaveTypes.Select(temp =>
            new SelectListItem()
            {
                Text = temp.LeaveTypeName,
                Value = temp.LeaveTypeID.ToString()
            });

            List<StatusOptions> statuses = Enum.GetValues(typeof(StatusOptions)).Cast<StatusOptions>().ToList();

            ViewBag.Statuses = statuses.Select(temp =>
            new SelectListItem()
            {
                Text = temp.ToString(),
                Value = temp.ToString()
            });

            return View(leaveUpdateRequest);
        }

        [HttpPost]
        [Route("{leaveID}")]
        public async Task<IActionResult> Edit(LeaveUpdateRequest leaveUpdateRequest)
        {
            LeaveResponse? leaveResponse = await _leavesGetterService.GetLeaveByLeaveID(leaveUpdateRequest.LeaveID);

            if (leaveResponse == null)
            {
                return RedirectToAction("Index", "Leave", new
                {
                    area = "Admin"
                });
            }

            if (!ModelState.IsValid)
            {
                List<ApplicationUser> users = _userManager.Users.ToList();

                ViewBag.Users = users.Select(temp =>
                new SelectListItem()
                {
                    Text = temp.Name,
                    Value = temp.Id.ToString()
                });

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

                return View(leaveResponse.ToLeaveUpdateRequest());
            }

            LeaveResponse updateLeave = await _leavesUpdaterService.UpdateLeave(leaveUpdateRequest);
            return RedirectToAction("Index", "Leave", new
            {
                area = "Admin"
            });

        }

        [HttpGet]
        [Route("{leaveID}")]
        public async Task<IActionResult> Delete(Guid? leaveID)
        {
            LeaveResponse? leaveResponse = await _leavesGetterService.GetLeaveByLeaveID(leaveID);

            if (leaveResponse == null)
            {
                return RedirectToAction("Index");
            }

            List<ApplicationUser> users = _userManager.Users.ToList();

            ViewBag.Users = users.Select(temp =>
            new SelectListItem()
            {
                Text = temp.Name,
                Value = temp.Id.ToString()
            });

            List<LeaveTypeResponse> leaveTypes = await _leaveTypesGetterService.GetAllLeaveTypes();

            ViewBag.LeaveTypes = leaveTypes.Select(temp =>
            new SelectListItem()
            {
                Text = temp.LeaveTypeName,
                Value = temp.LeaveTypeID.ToString()
            });

            List<StatusOptions> statuses = Enum.GetValues(typeof(StatusOptions)).Cast<StatusOptions>().ToList();

            ViewBag.Statuses = statuses.Select(temp =>
            new SelectListItem()
            {
                Text = temp.ToString(),
                Value = temp.ToString()
            });

            return View(leaveResponse);
        }

        [HttpPost]
        [Route("{leaveID}")]
        public async Task<IActionResult> Delete(LeaveUpdateRequest leaveUpdateRequest)
        {
            LeaveResponse? leaveResponse = await _leavesGetterService.GetLeaveByLeaveID(leaveUpdateRequest.LeaveID);

            if (leaveResponse == null)
            {
                return RedirectToAction(nameof(LeaveController.Index), "Leave", new
                {
                    area = "Admin"
                });
            }

            await _leavesDeleterService.DeleteLeave(leaveResponse.LeaveID);
            return RedirectToAction(nameof(LeaveController.Index), "Leave", new
            {
                area = "Admin"
            });
        }
    }
}
