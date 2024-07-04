//--------------------------------------------------
// Copyright (c) Coalition Of Good-Hearted Engineers
// Free To Use To Find Comfort And Peace
//--------------------------------------------------
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using VideoTime.Models.VideoMetadatas;

namespace VideoTime.Unit.Tests.Sevices.Foundations.VideoMetadatas
{
    public partial class VideoMetadataServiceTest
    {
        [Fact]
        public async Task ShouldRetrieveVideoMetadataByIdAsync()
        {
            //given
            Guid randomVideoMetadataId = Guid.NewGuid();
            Guid inputVideoMetadataId = randomVideoMetadataId;
            VideoMetadata randomVideoMetadata = CreateRandomVideoMetadata();
            VideoMetadata storageVideoMetadata = randomVideoMetadata;
            VideoMetadata expectedVideoMetadata = storageVideoMetadata.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectVideoMetadataByIdAsync(inputVideoMetadataId))
                    .ReturnsAsync(storageVideoMetadata);

            //when
            VideoMetadata actualVideoMetadata =
                await this.videoMetadataService.RetrieveVideoMetadataByIdAsync(inputVideoMetadataId);

            //then
            actualVideoMetadata.Should().BeEquivalentTo(expectedVideoMetadata);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectVideoMetadataByIdAsync(inputVideoMetadataId), Times.Once());

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}