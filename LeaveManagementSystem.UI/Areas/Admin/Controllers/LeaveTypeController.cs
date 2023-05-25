using LeaveManagementSystem.Core.Domain.Entities;
using LeaveManagementSystem.Core.DTO;
using LeaveManagementSystem.Core.ServiceContracts;
using LeaveManagementSystem.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace LeaveManagementSystem.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [Route("Admin/[controller]/[action]")]
    public class LeaveTypeController : Controller
    {
        private readonly ILeaveTypesAdderService _leaveTypesAdderService;
        private readonly ILeaveTypesGetterService _leaveTypesGetterService;
        private readonly ILeaveTypesUpdaterService _leaveTypesUpdaterService;
        private readonly ILeaveTypesDeleterService _leaveTypesDeleterService;

        public LeaveTypeController(ILeaveTypesAdderService leaveTypesAdderService, ILeaveTypesGetterService leaveTypesGetterService, ILeaveTypesUpdaterService leaveTypesUpdaterService, ILeaveTypesDeleterService leaveTypesDeleterService)
        {
            _leaveTypesAdderService = leaveTypesAdderService;
            _leaveTypesGetterService = leaveTypesGetterService;
            _leaveTypesUpdaterService = leaveTypesUpdaterService;
            _leaveTypesDeleterService = leaveTypesDeleterService;
        }

        public async Task<IActionResult> Index()
        {
            List<LeaveTypeResponse> leaveTypes = await _leaveTypesGetterService.GetAllLeaveTypes();
            return View(leaveTypes);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(LeaveTypeAddRequest leaveTypeAddRequest)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values
                    .SelectMany(value => value.Errors)
                    .SelectMany(error => error.ErrorMessage);
                return View();
            }

            LeaveTypeResponse leaveTypeResponse = await _leaveTypesAdderService.AddLeaveType(leaveTypeAddRequest);
            return RedirectToAction("Index", "LeaveType");
        }

        [HttpGet]
        [Route("{leaveTypeID}")]
        public async Task<IActionResult> Edit(Guid? leaveTypeID)
        {
            LeaveTypeResponse? leaveTypeResponse = await _leaveTypesGetterService.GetLeaveTypeByLeaveTypeID(leaveTypeID);

            if (leaveTypeResponse == null)
            {
                return RedirectToAction("Index");
            }

            LeaveTypeUpdateRequest leaveTypeUpdateRequest = leaveTypeResponse.ToLeaveTypeUpdateRequest();
            return View(leaveTypeUpdateRequest);
        }

        [HttpPost]
        [Route("{leaveTypeID}")]
        public async Task<IActionResult> Edit(LeaveTypeUpdateRequest leaveTypeUpdateRequest)
        {
            LeaveTypeResponse? leaveTypeResponse = await _leaveTypesGetterService.GetLeaveTypeByLeaveTypeID(leaveTypeUpdateRequest.LeaveTypeID);

            if (leaveTypeResponse == null)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                LeaveTypeResponse updateLeaveType = await _leaveTypesUpdaterService.UpdateLeaveType(leaveTypeUpdateRequest);
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Errors = ModelState.Values
                    .SelectMany(value => value.Errors)
                    .SelectMany(error => error.ErrorMessage);
                return View();
            }
        }

        [HttpGet]
        [Route("{leaveTypeID}")]
        public async Task<IActionResult> Delete(Guid? leaveTypeID)
        {
            LeaveTypeResponse? leaveTypeResponse = await _leaveTypesGetterService.GetLeaveTypeByLeaveTypeID(leaveTypeID);

            if (leaveTypeResponse == null)
            {
                return RedirectToAction("Index");
            }

            return View(leaveTypeResponse);
        }

        [HttpPost]
        [Route("{leaveTypeID}")]
        public async Task<IActionResult> Delete(LeaveTypeUpdateRequest leaveTypeUpdateRequest)
        {
            LeaveTypeResponse? leaveTypeResponse = await _leaveTypesGetterService.GetLeaveTypeByLeaveTypeID(leaveTypeUpdateRequest.LeaveTypeID);

            if (leaveTypeResponse == null)
            {
                return RedirectToAction("Index");
            }

            await _leaveTypesDeleterService.DeleteLeaveType(leaveTypeResponse.LeaveTypeID);
            return RedirectToAction("Index");
        }
    }
}
