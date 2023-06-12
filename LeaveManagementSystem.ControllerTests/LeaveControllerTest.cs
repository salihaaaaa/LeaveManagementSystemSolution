using AutoFixture;
using FluentAssertions;
using LeaveManagementSystem.Core.Domain.IdentityEntities;
using LeaveManagementSystem.Core.DTO;
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
    }
}