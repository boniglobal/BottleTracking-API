using Business.Services;
using Business.Utilities;
using Core.Constants;
using Data.Abstract;
using Entities;
using FluentAssertions;
using Moq;
using Xunit;
using static Core.DTOs.StationLog;

namespace Tests
{
    public class StationLogManagementTests
    {
        private readonly Mock<IStationLogRepository> _stationLogRepository;
        private readonly Mock<IBottleService> _bottleService;
        private readonly Mock<IStationService> _stationService;
        private readonly IStationLogService _stationLogService;
        private readonly StationLogAdd _addDto;

        public StationLogManagementTests()
        {
            _stationLogRepository = new Mock<IStationLogRepository>();
            _bottleService = new Mock<IBottleService>();
            _stationService = new Mock<IStationService>();
            _stationLogService = new StationLogService(_stationLogRepository.Object, _bottleService.Object, _stationService.Object);
            _addDto = new StationLogAdd();
        }

        [Theory]
        [InlineData(null)]
        public void StationLogService_Add_Should_Throw_Exception_With_UserNotAssignedToKiosk_Message(int? kioskId)
        {
            _stationLogService.Invoking(x => x.Add(_addDto, kioskId))
                              .Should().Throw<CustomException>()
                              .WithMessage(Messages.UserNotAssignedToKiosk);
        }

        [Theory]
        [InlineData(15265878, 1)]
        public void StationLogService_Add_Should_Throw_Exception_With_BottleNotFound_Message(
            long trackingId, int? kioskId)
        {
            _addDto.TrackingId = trackingId;
            _bottleService.Setup(x => x.GetByTrackingId(_addDto.TrackingId)).Returns((Bottle)null);
            _stationService.Setup(x => x.GetById(kioskId.GetValueOrDefault())).Returns(new Station());
            _stationLogService.Invoking(x => x.Add(_addDto, kioskId))
                              .Should().Throw<CustomException>()
                              .WithMessage(Messages.BottleNotFound);
        }

        [Theory]
        [InlineData(15265878, 1)]
        public void StationLogService_Add_Should_Not_Throw_Exception(long trackingId, int? kioskId)
        {
            _addDto.TrackingId = trackingId;
            _bottleService.Setup(x=>x.GetByTrackingId(_addDto.TrackingId)).Returns(new Bottle());
            _stationService.Setup(x=>x.GetById(kioskId.GetValueOrDefault())).Returns(new Station());
            _stationLogService.Invoking(x => x.Add(_addDto, kioskId))
                              .Should().NotThrow();
        }
    }
}
