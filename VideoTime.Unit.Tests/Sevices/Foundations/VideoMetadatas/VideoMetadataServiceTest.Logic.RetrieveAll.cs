//==================================================
// Copyright (c) Coalition Of Good-Hearted Engineers
// Free To Use To Find Comfort And Peace
//==================================================
using FluentAssertions;
using Moq;
using VideoTime.Models.VideoMetadatas;

namespace VideoTime.Unit.Tests.Sevices.Foundations.VideoMetadatas
{
    public partial class VideoMetadataServiceTest
    {
        [Fact]
        public void ShouldRetrieveAllVideoMetadatas()
        {
            //given
            IQueryable<VideoMetadata> randomVideoMetadatas = CreateRandomVideoMetadatas();
            IQueryable<VideoMetadata> storageVideoMetadatas = randomVideoMetadatas;
            IQueryable<VideoMetadata> expectedVideoMetadatas = storageVideoMetadatas;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllVideoMetadatas()).Returns(storageVideoMetadatas);

            //when
            IQueryable<VideoMetadata> actualVideoMetadatas =
                this.videoMetadataService.RetrieveAllVideoMetadatas();

            //then
            actualVideoMetadatas.Should().BeEquivalentTo(expectedVideoMetadatas);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllVideoMetadatas(), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}