using AutoFixture;
using FluentAssertions;
using LeaveManagementSystem.Core.Domain.IdentityEntities;
using LeaveManagementSystem.Core.DTO;
using LeaveManagementSystem.Core.Enums;
using LeaveManagementSystem.Core.ServiceContracts;
using LeaveManagementSystem.UI.Areas.Admin.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LeaveManagementSystem.ControllerTests
{
    public class LeaveControllerTest
    {
        private readonly IFixture _fixture;

        private readonly ILeaveTypesGetterService _leaveTypesGetterService;
        private readonly ILeavesAdderService _leavesAdderService;
        private readonly ILeavesGetterService _leavesGetterService;
        private readonly ILeavesUpdaterService _leavesUpdaterService;
        private readonly ILeavesDeleterService _leavesDeleterService;

        private readonly Mock<ILeaveTypesGetterService> _leaveTypeGetterServiceMock;
        private readonly Mock<ILeavesAdderService> _leavesAdderServiceMock;
        private readonly Mock<ILeavesGetterService> _leavesGetterServiceMock;
        private readonly Mock<ILeavesUpdaterService> _leavesUpdaterServiceMock;
        private readonly Mock<ILeavesDeleterService> _leavesDeleterServiceMock;

        public LeaveControllerTest()
        {
            _fixture = new Fixture();

            _leaveTypeGetterServiceMock = new Mock<ILeaveTypesGetterService>();
            _leavesAdderServiceMock = new Mock<ILeavesAdderService>();
            _leavesGetterServiceMock = new Mock<ILeavesGetterService>();
            _leavesUpdaterServiceMock = new Mock<ILeavesUpdaterService>();
            _leavesDeleterServiceMock = new Mock<ILeavesDeleterService>();

            _leaveTypesGetterService = _leaveTypeGetterServiceMock.Object;
            _leavesAdderService = _leavesAdderServiceMock.Object;
            _leavesGetterService = _leavesGetterServiceMock.Object;
            _leavesUpdaterService = _leavesUpdaterServiceMock.Object;
            _leavesDeleterService = _leavesDeleterServiceMock.Object;
        }

        #region Index
        [Fact]
        public async Task Index_ShouldReturnIndexViewWithLeavesList()
        {
            //Arrange
            var mockUserManager = new Mock<MockUserManager<ApplicationUser>>();

            List<LeaveResponse> leave_response_list = _fixture.Create<List<LeaveResponse>>();

            LeaveController leaveController = new LeaveController(
                mockUserManager.Object, 
                _leaveTypesGetterService, 
                _leavesAdderService, 
                _leavesGetterService, 
                _leavesUpdaterService, 
                _leavesDeleterService);

            _leavesGetterServiceMock
                .Setup(temp => temp.GetAllLeaves())
                .ReturnsAsync(leave_response_list);

            //Act
            IActionResult result = await leaveController.Index();

            //Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            viewResult.ViewData.Model.Should().BeAssignableTo<List<LeaveResponse>>();
            viewResult.ViewData.Model.Should().Be(leave_response_list);
        }
        #endregion

        #region Add
        [Fact]
        public async Task Add_IfModelError_ToReturnAddView()
        {
            //Arrange
            LeaveAddRequest leaveAddRequest = _fixture.Create<LeaveAddRequest>();

            LeaveResponse leaveResponse = _fixture.Create<LeaveResponse>();

            List<LeaveTypeResponse> leaveTypes = _fixture.Create<List<LeaveTypeResponse>>();

            var mockUserManager = new Mock<MockUserManager<ApplicationUser>>();

            _leaveTypeGetterServiceMock
                .Setup(temp => temp.GetAllLeaveTypes())
                .ReturnsAsync(leaveTypes);

            _leavesAdderServiceMock
                .Setup(temp => temp.AddLeave(It.IsAny<LeaveAddRequest>()))
                .ReturnsAsync(leaveResponse);

            LeaveController leaveController = new LeaveController(
                mockUserManager.Object,
                _leaveTypesGetterService,
                _leavesAdderService,
                _leavesGetterService,
                _leavesUpdaterService,
                _leavesDeleterService);

            //Act
            leaveController.ModelState.AddModelError("StartDate", "Please select Start Date");

            IActionResult result = await leaveController.Add(leaveAddRequest);

            //Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);

            viewResult.ViewData.Model.Should().BeAssignableTo<LeaveAddRequest>();

            viewResult.ViewData.Model.Should().Be(leaveAddRequest);
        }

        [Fact]
        public async Task Add_IfNoModelError_ToReturnRedirectToIndex()
        {
            //Arrange
            LeaveAddRequest leaveAddRequest = _fixture.Create<LeaveAddRequest>();

            LeaveResponse leaveResponse = _fixture.Create<LeaveResponse>();

            List<LeaveTypeResponse> leaveTypes = _fixture.Create<List<LeaveTypeResponse>>();

            var mockUserManager = new Mock<MockUserManager<ApplicationUser>>();

            _leaveTypeGetterServiceMock
                .Setup(temp => temp.GetAllLeaveTypes())
                .ReturnsAsync(leaveTypes);

            _leavesAdderServiceMock
                .Setup(temp => temp.AddLeave(It.IsAny<LeaveAddRequest>()))
                .ReturnsAsync(leaveResponse);

            LeaveController leaveController = new LeaveController(
                mockUserManager.Object,
                _leaveTypesGetterService,
                _leavesAdderService,
                _leavesGetterService,
                _leavesUpdaterService,
                _leavesDeleterService);

            //Act
            IActionResult result = await leaveController.Add(leaveAddRequest);

            //Assert
            RedirectToActionResult redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);

            redirectToActionResult.ActionName.Should().Be("Index");
        }
        #endregion

        #region Edit
        [Fact]
        public async Task Edit_IfLeaveIDIsNull_ToReturnRedirectToIndex()
        {
            //Arrange
            LeaveUpdateRequest leaveUpdateRequest = _fixture.Create<LeaveUpdateRequest>();

            LeaveResponse leaveResponse = _fixture.Create<LeaveResponse>();

            List<LeaveTypeResponse> leaveTypes = _fixture.Create<List<LeaveTypeResponse>>();

            var mockUserManager = new Mock<MockUserManager<ApplicationUser>>();

            _leaveTypeGetterServiceMock
                .Setup(temp => temp.GetAllLeaveTypes())
                .ReturnsAsync(leaveTypes);

            _leavesUpdaterServiceMock
                .Setup(temp => temp.UpdateLeave(It.IsAny<LeaveUpdateRequest>()))
                .ReturnsAsync(leaveResponse);

            LeaveController leaveController = new LeaveController(
                mockUserManager.Object,
                _leaveTypesGetterService,
                _leavesAdderService,
                _leavesGetterService,
                _leavesUpdaterService,
                _leavesDeleterService);

            //Act
            leaveController.ModelState.AddModelError("LeaveID", "Leave ID not exist!");
            IActionResult result = await leaveController.Edit(leaveUpdateRequest.LeaveID);

            //Assert
            RedirectToActionResult redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            redirectToActionResult.ActionName.Should().Be("Index");
        }

        [Fact]
        public async Task Edit_IfModelError_ToReturnEditView()
        {
            //Arrange
            LeaveUpdateRequest leaveUpdateRequest = _fixture.Create<LeaveUpdateRequest>();

            LeaveResponse leaveResponse = _fixture.Create<LeaveResponse>();

            List<LeaveTypeResponse> leaveTypes = _fixture.Create<List<LeaveTypeResponse>>();

            var mockUserManager = new Mock<MockUserManager<ApplicationUser>>();

            _leaveTypeGetterServiceMock
                .Setup(temp => temp.GetAllLeaveTypes())
                .ReturnsAsync(leaveTypes);

            _leavesUpdaterServiceMock
                .Setup(temp => temp.UpdateLeave(It.IsAny<LeaveUpdateRequest>()))
                .ReturnsAsync(leaveResponse);

            LeaveController leaveController = new LeaveController(
                mockUserManager.Object,
                _leaveTypesGetterService,
                _leavesAdderService,
                _leavesGetterService,
                _leavesUpdaterService,
                _leavesDeleterService);

            //Act
            leaveController.ModelState.AddModelError("LeaveType", "Leave Type is required");
            IActionResult result = await leaveController.Edit(leaveUpdateRequest.LeaveID);

            //Assert
            RedirectToActionResult redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            redirectToActionResult.ActionName.Should().Be("Index");
        }

        [Fact]
        public async Task Edit_IfNoModelError_ToReturnRedirectToIndex()
        {
            //Arrange
            LeaveUpdateRequest leaveUpdateRequest = _fixture.Create<LeaveUpdateRequest>();

            LeaveResponse leaveResponse = _fixture.Create<LeaveResponse>();

            List<LeaveTypeResponse> leaveTypes = _fixture.Create<List<LeaveTypeResponse>>();

            var mockUserManager = new Mock<MockUserManager<ApplicationUser>>();

            _leaveTypeGetterServiceMock
                .Setup(temp => temp.GetAllLeaveTypes())
                .ReturnsAsync(leaveTypes);

            _leavesUpdaterServiceMock
                .Setup(temp => temp.UpdateLeave(It.IsAny<LeaveUpdateRequest>()))
                .ReturnsAsync(leaveResponse);

            LeaveController leaveController = new LeaveController(
                mockUserManager.Object,
                _leaveTypesGetterService,
                _leavesAdderService,
                _leavesGetterService,
                _leavesUpdaterService,
                _leavesDeleterService);

            //Act
            IActionResult result = await leaveController.Edit(leaveUpdateRequest.LeaveID);

            //Assert
            RedirectToActionResult redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);

            redirectToActionResult.ActionName.Should().Be("Index");
        }
        #endregion

        #region Delete
        [Fact]
        public async Task Delete_IfLeaveIDIsNull_ToReturnRedirectToIndex()
        {
            //Arrange
            LeaveUpdateRequest leaveUpdateRequest = _fixture.Create<LeaveUpdateRequest>();

            LeaveResponse leaveResponse = _fixture.Create<LeaveResponse>();

            var mockUserManager = new Mock<MockUserManager<ApplicationUser>>();

            _leavesGetterServiceMock
                .Setup(temp => temp.GetLeaveByLeaveID(It.IsAny<Guid>()))
                .ReturnsAsync(leaveResponse);

            _leavesDeleterServiceMock
                .Setup(temp => temp.DeleteLeave(It.IsAny<Guid>()));

            LeaveController leaveController = new LeaveController(
                mockUserManager.Object,
                _leaveTypesGetterService,
                _leavesAdderService,
                _leavesGetterService,
                _leavesUpdaterService,
                _leavesDeleterService);

            //Act
            leaveController.ModelState.AddModelError("LeaveID", "Leave ID not exist!");
            IActionResult result = await leaveController.Delete(leaveUpdateRequest);

            //Assert
            RedirectToActionResult redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            redirectToActionResult.ActionName.Should().Be("Index");
        }

        [Fact]
        public async Task Delete_IfNoError_ToReturnRedirectToIndex()
        {
            //Arrange
            LeaveUpdateRequest leaveUpdateRequest = _fixture.Create<LeaveUpdateRequest>();

            LeaveResponse leaveResponse = _fixture.Create<LeaveResponse>();

            var mockUserManager = new Mock<MockUserManager<ApplicationUser>>();

            _leavesGetterServiceMock
                .Setup(temp => temp.GetLeaveByLeaveID(It.IsAny<Guid>()))
                .ReturnsAsync(leaveResponse);

            _leavesDeleterServiceMock
                .Setup(temp => temp.DeleteLeave(It.IsAny<Guid>()));

            LeaveController leaveController = new LeaveController(
                mockUserManager.Object,
                _leaveTypesGetterService,
                _leavesAdderService,
                _leavesGetterService,
                _leavesUpdaterService,
                _leavesDeleterService);

            //Act
            IActionResult result = await leaveController.Delete(leaveUpdateRequest);

            //Assert
            RedirectToActionResult redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);

            redirectToActionResult.ActionName.Should().Be("Index");
        }
        #endregion
    }
}