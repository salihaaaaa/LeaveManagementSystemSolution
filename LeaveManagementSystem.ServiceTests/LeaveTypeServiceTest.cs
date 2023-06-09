using AutoFixture;
using FluentAssertions;
using LeaveManagementSystem.Core.Domain.Entities;
using LeaveManagementSystem.Core.Domain.IdentityEntities;
using LeaveManagementSystem.Core.Domain.RepositoryContracts;
using LeaveManagementSystem.Core.DTO;
using LeaveManagementSystem.Core.ServiceContracts;
using LeaveManagementSystem.Core.Services;
using Moq;
using System;
using Xunit.Abstractions;

namespace LeaveManagementSystem.ServiceTests
{
    public class LeaveTypeServiceTest
    {
        private readonly IFixture _fixture;

        private readonly Mock<ILeaveTypesRepository> _leaveTypeRepositoryMock;
        private readonly ILeaveTypesRepository _leaveTypeRepository;

        private readonly ILeaveTypesGetterService _leaveTypesGetterService;
        private readonly ILeaveTypesAdderService _leaveTypesAdderService;
        private readonly ILeaveTypesUpdaterService _leaveTypesUpdaterService;
        private readonly ILeaveTypesDeleterService _leaveTypesDeleterService;

        private readonly ITestOutputHelper _testOutputHelper;

        public LeaveTypeServiceTest(ITestOutputHelper testOutputHelper)
        {
            _fixture = new Fixture();

            _leaveTypeRepositoryMock = new Mock<ILeaveTypesRepository>();
            _leaveTypeRepository = _leaveTypeRepositoryMock.Object;

            _leaveTypesGetterService = new LeaveTypesGetterService(_leaveTypeRepository);
            _leaveTypesAdderService = new LeaveTypesAdderService(_leaveTypeRepository);
            _leaveTypesUpdaterService = new LeaveTypesUpdaterService(_leaveTypeRepository);
            _leaveTypesDeleterService = new LeaveTypesDeleterService(_leaveTypeRepository);

            _testOutputHelper = testOutputHelper;
        }

        #region AddLeaveType
        [Fact]
        public async Task AddLeaveType_NullLeaveType_ToBeArgumentNullException()
        {
            //Arrange
            LeaveTypeAddRequest? leaveTypeAddRequest = null;

            //Act
            var action = async () =>
            {
                await _leaveTypesAdderService.AddLeaveType(leaveTypeAddRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task AddLeave_LeaveTypeNameIsNull_ToBeArgumentException()
        {
            //Arrange
            LeaveTypeAddRequest? leaveTypeAddRequest = _fixture
                .Build<LeaveTypeAddRequest>().With(temp => temp.LeaveTypeName, null as string)
                .Create();

            //Act
            var action = async () =>
            {
                await _leaveTypesAdderService.AddLeaveType(leaveTypeAddRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task AddLeaveType_FullLeaveTypeDetails_ToBeSuccessful()
        {
            //Arrange
            LeaveTypeAddRequest? leaveTypeAddRequest = _fixture.Create<LeaveTypeAddRequest>();

            LeaveType leaveType = leaveTypeAddRequest.ToLeaveType();
            LeaveTypeResponse leaveType_response_expected = leaveType.ToLeaveTypeResponse();

            _leaveTypeRepositoryMock
                .Setup(temp => temp.AddLeaveType(It.IsAny<LeaveType>()))
                .ReturnsAsync(leaveType);

            //Act
            LeaveTypeResponse leaveType_response_from_add = await _leaveTypesAdderService.AddLeaveType(leaveTypeAddRequest);
            leaveType_response_expected.LeaveTypeID = leaveType_response_from_add.LeaveTypeID;

            //Assert
            leaveType_response_from_add.LeaveTypeID.Should().NotBe(Guid.Empty);
            leaveType_response_from_add.Should().BeEquivalentTo(leaveType_response_expected);
        }
        #endregion

        #region GetAllLeaveTypes
        [Fact]
        public async Task GetAllLeaveTypes_ToBeEmptyList()
        {
            //Arrange
            var leaveTypes = new List<LeaveType>();

            _leaveTypeRepositoryMock
                .Setup(temp => temp.GetAlLeaveTypes())
                .ReturnsAsync(leaveTypes);

            //Act
            List<LeaveTypeResponse> leaveTypes_response_from_get = await _leaveTypesGetterService.GetAllLeaveTypes();

            //Assert
            leaveTypes_response_from_get.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllLeaveTypes_WithFewLeaveType_ToBeSuccessful()
        {
            //Arrange
            List<LeaveType> leaveTypes = new List<LeaveType>()
           {
               _fixture.Create<LeaveType>(),
               _fixture.Create<LeaveType>(),
               _fixture.Create<LeaveType>()
           };

            List<LeaveTypeResponse> leaveType_response_list_expected = leaveTypes.Select(temp => temp.ToLeaveTypeResponse()).ToList();

            //print leaveType_response_list_expected
            _testOutputHelper.WriteLine("Expected:");
            foreach (LeaveTypeResponse leaveType_response_expected in leaveType_response_list_expected)
            {
                _testOutputHelper.WriteLine(leaveType_response_expected.ToString());
            }

            _leaveTypeRepositoryMock
                .Setup(temp => temp.GetAlLeaveTypes())
                .ReturnsAsync(leaveTypes);

            //Act
            List<LeaveTypeResponse> leaveType_response_list_from_get = await _leaveTypesGetterService.GetAllLeaveTypes();

            //print leaveType_response_list_from_get
            _testOutputHelper.WriteLine("Actual:");
            foreach (LeaveTypeResponse leaveType_response_from_get in leaveType_response_list_from_get)
            {
                _testOutputHelper.WriteLine(leaveType_response_from_get.ToString());
            }

            //Assert
            leaveType_response_list_from_get.Should().BeEquivalentTo(leaveType_response_list_expected);
        }
        #endregion

        #region GetLeaveTypeByLeaveTypeID
        [Fact]
        public async Task GetLeaveTypeByLeaveTypeID_NullLeaveTypeID_ToBeNull()
        {
            //Arrange
            Guid? leaveTypeID = null;

            //Act
            LeaveTypeResponse? leaveType_response_from_get = await _leaveTypesGetterService.GetLeaveTypeByLeaveTypeID(leaveTypeID);

            //Assert
            leaveType_response_from_get.Should().BeNull();
        }

        [Fact]
        public async Task GetLeaveTypeByLeaveTypeID_WithLeaveTypeID_ToBeSuccessful()
        {
            //Arrange
            LeaveType leaveType = _fixture.Create<LeaveType>();

            LeaveTypeResponse leaveType_response_expected = leaveType.ToLeaveTypeResponse();

            _leaveTypeRepositoryMock
                .Setup(temp => temp.GetLeaveTypeByLeaveTypeID(It.IsAny<Guid>()))
                .ReturnsAsync(leaveType);

            //Act
            LeaveTypeResponse? leaveType_response_from_get = await _leaveTypesGetterService.GetLeaveTypeByLeaveTypeID(leaveType.LeaveTypeID);

            //Assert
            leaveType_response_from_get.Should().BeEquivalentTo(leaveType_response_expected);
        }
        #endregion

        #region UpdateLeaveType
        [Fact]
        public async Task UpdateLeaveType_NullLeaveType_ToBeArgumentNullException()
        {
            //Arrange
            LeaveTypeUpdateRequest? leaveType_update_request = null;

            //Act
            var action = async () =>
            {
                await _leaveTypesUpdaterService.UpdateLeaveType(leaveType_update_request);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task UpdateLeaveType_LeaveTypeNameIsNull_ToBeArgumentException()
        {
            //Arrange
            LeaveTypeUpdateRequest? leaveType_update_request = _fixture
                .Build<LeaveTypeUpdateRequest>()
                .With(temp => temp.LeaveTypeName, null as string)
                .Create();

            //Act
            var action = async () =>
            {
                await _leaveTypesUpdaterService.UpdateLeaveType(leaveType_update_request);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task UpdateLeaveType_FullLeaveTypeDetails_ToBeSuccessful()
        {
            //Arrange
            LeaveType leaveType = _fixture.Create<LeaveType>();

            LeaveTypeResponse leaveType_response_expected = leaveType.ToLeaveTypeResponse();

            LeaveTypeUpdateRequest leaveType_update_request = leaveType_response_expected.ToLeaveTypeUpdateRequest();

            _leaveTypeRepositoryMock
                .Setup(temp => temp.UpdateLeaveType(It.IsAny<LeaveType>()))
                .ReturnsAsync(leaveType);

            _leaveTypeRepositoryMock
                .Setup(temp => temp.GetLeaveTypeByLeaveTypeID(It.IsAny<Guid>()))
                .ReturnsAsync(leaveType);

            //Act
            LeaveTypeResponse leaveType_response_from_update = await _leaveTypesUpdaterService.UpdateLeaveType(leaveType_update_request);

            //Assert
            leaveType_response_from_update.Should().BeEquivalentTo(leaveType_response_expected);
        }
        #endregion

        #region DeleteLeaveType
        [Fact]
        public async Task DeleteLeaveType_ValidLeaveTypeID_ToBeSuccessful()
        {
            //Arrange 
            LeaveType leaveType = _fixture.Create<LeaveType>();

            _leaveTypeRepositoryMock
                .Setup(temp => temp.DeleteLeaveType(It.IsAny<Guid>()))
                .ReturnsAsync(true);

            _leaveTypeRepositoryMock
                .Setup(temp => temp.GetLeaveTypeByLeaveTypeID(It.IsAny<Guid>()))
                .ReturnsAsync(leaveType);

            //Act
            bool isDeleted = await _leaveTypesDeleterService.DeleteLeaveType(leaveType.LeaveTypeID);

            //Assert
            isDeleted.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteLeaveType_InvalidLeaveTypeID()
        {
            //Act
            bool isDeleted = await _leaveTypesDeleterService.DeleteLeaveType(Guid.NewGuid());

            //Assert
            isDeleted.Should().BeFalse();
        }
        #endregion
    }
}
