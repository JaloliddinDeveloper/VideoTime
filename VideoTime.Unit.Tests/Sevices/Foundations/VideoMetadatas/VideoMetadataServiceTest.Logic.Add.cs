//==================================================
// Copyright (c) Coalition Of Good-Hearted Engineers
// Free To Use To Find Comfort And Peace
//==================================================
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using VideoTime.Models.VideoMetadatas;

namespace VideoTime.Unit.Tests.Sevices.Foundations.VideoMetadatas
{
    public partial class VideoMetadataServiceTest
    {
        [Fact]
        public async Task ShouldAddVideoMetadataAsync()
        {
            //give
            DateTimeOffset randomDate = GetRandomDateTimeOffset();
            VideoMetadata randomVideoMetadata = CreateRandomVideoMetadata(randomDate);
            VideoMetadata inputVideoMetadata = randomVideoMetadata;
            VideoMetadata returningVideoMetadata = inputVideoMetadata;
            VideoMetadata expectedVideoMetadata = returningVideoMetadata.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
              broker.GetCurrentDateTimeOffset()).Returns(randomDate);
            this.storageBrokerMock.Setup(broker =>
            broker.InsertVideoMetadataAsync(inputVideoMetadata)).ReturnsAsync(expectedVideoMetadata);
            //when
            VideoMetadata actualVideoMetadata =
                await this.videoMetadataService.AddVideoMetadataAsync(inputVideoMetadata);
            //then
            actualVideoMetadata.Should().BeEquivalentTo(expectedVideoMetadata);
            this.dateTimeBrokerMock.Verify(broker =>
               broker.GetCurrentDateTimeOffset(), Times.Once);
            this.storageBrokerMock.Verify(broker =>
            broker.InsertVideoMetadataAsync(inputVideoMetadata), Times.Once());

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
