//==================================================
// Copyright (c) Coalition Of Good-Hearted Engineers
// Free To Use To Find Comfort And Peace
//==================================================
using Moq;
using VideoTime.Models.Exceptions;
using VideoTime.Models.VideoMetadatas;

namespace VideoTime.Unit.Tests.Sevices.Foundations.VideoMetadatas
{
    public partial class VideoMetadataServiceTest
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfVideoMetadataIsNullAndLogItAsync()
        {
            //given
            VideoMetadata nullVideoMetadata = null;
            var nullVideoMetadataException =
              new NullVideoMetadataException(message: "Video metadata is null");

            var expectedVideoMetadataValidationException =
                new VideoMetadataValidationException(
                    message: "Video metadata Validation error occurred,fix the errors and try again",
                    innerException: nullVideoMetadataException);

            //when
            ValueTask<VideoMetadata> addVideoMetadataTask =
                this.videoMetadataService.AddVideoMetadataAsync(nullVideoMetadata);  //then
            await Assert.ThrowsAsync<VideoMetadataValidationException>(() =>
            addVideoMetadataTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
            broker.LogError(It.Is(SameExceptionAs(expectedVideoMetadataValidationException))),
            Times.Once());

            this.storageBrokerMock.Verify(broker =>
            broker.InsertVideoMetadataAsync(It.IsAny<VideoMetadata>()), Times.Never());


            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

