using Business.Services;
using Business.Utilities;
using Core.Constants;
using Data.Abstract;
using FluentAssertions;
using Moq;
using Xunit;

namespace Tests
{
    public class StationLogManagementTests
    {
        private readonly IMock<IStationLogRepository> _stationLogRepository;
        private readonly IStationLogService _stationLogService;

        public StationLogManagementTests()
        {
            _stationLogRepository = new Mock<IStationLogRepository>();
            _stationLogService = new StationLogService(_stationLogRepository.Object);
        }

        [Theory]
        [InlineData(15265878, null)]
        public void StationLogService_Add_Should_Throw_Exception_With_UserNotAssignedToKiosk_Message(
            long trackingId, int? kioskId)
        {
            _stationLogService.Invoking(x => x.Add(trackingId, kioskId))
                              .Should().Throw<CustomException>()
                              .WithMessage(Messages.UserNotAssignedToKiosk);
        }

        [Theory]
        [InlineData(15265878, 1)]
        public void StationLogService_Add_Should_Not_Throw_Exception_With_UserNotAssignedToKiosk_Message(
            long trackingId, int? kioskId)
        {
            _stationLogService.Invoking(x => x.Add(trackingId, kioskId))
                              .Should().NotThrow();
        }
    }
}
