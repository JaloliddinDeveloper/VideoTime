//==================================================
// Copyright (c) Coalition Of Good-Hearted Engineers
// Free To Use To Find Comfort And Peace
//==================================================
using Moq;
using Tynamix.ObjectFiller;
using VideoTime.Brokers.Storages;
using VideoTime.Models.VideoMetadatas;
using VideoTime.Services.Foundations.VideoMetadatas;

namespace VideoTime.Unit.Tests.Sevices.Foundations.VideoMetadatas
{
    public partial class VideoMetadataServiceTest
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly IVideoMetadataService videoMetadataService;

        public VideoMetadataServiceTest()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();

            this.videoMetadataService = new VideoMetadataService
                (storageBroker: this.storageBrokerMock.Object);
        }
        private static DateTimeOffset GetRandomDateTime() =>
           new DateTimeRange(earliestDate: DateTime.UnixEpoch).GetValue();

        private static VideoMetadata CreateRandomVideoMetadata() =>
         CreateVideoMetadataFiller(GetRandomDateTime()).Create();
        private static Filler<VideoMetadata> CreateVideoMetadataFiller(DateTimeOffset date)
        {
            var filler = new Filler<VideoMetadata>();

            filler.Setup()
               .OnType<DateTimeOffset>().Use(date);
            return filler;
        }
    }
}
