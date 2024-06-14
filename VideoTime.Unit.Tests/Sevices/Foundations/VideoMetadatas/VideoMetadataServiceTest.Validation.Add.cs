//==================================================
// Copyright (c) Coalition Of Good-Hearted Engineers
// Free To Use To Find Comfort And Peace
//==================================================
using FluentAssertions;
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
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfVideoMetadataIsInvalidDataAndLogItAsync(string invalidData)
        {
            // given
            var invalidVideoMetadata = new VideoMetadata()
            {
                Title = invalidData
            };

            var invalidVideoMetadataException =
                new InvalidVideoMetadataException(message: "Video metadata is invalid");

            invalidVideoMetadataException.AddData(key: nameof(VideoMetadata.Id),
                values: "Id is required");

            invalidVideoMetadataException.AddData(key: nameof(VideoMetadata.Title),
                values: "Text is required");

            invalidVideoMetadataException.AddData(key: nameof(VideoMetadata.BlobPath),
                 values: "Text is required");

            invalidVideoMetadataException.AddData(key: nameof(VideoMetadata.CreatedDate),
                 values: "Data is required");
            invalidVideoMetadataException.AddData(key: nameof(VideoMetadata.UpdatedDate),
                 values: "Data is required");

            var expectedVideoMetadataValidationExpected =
                new VideoMetadataValidationException(
                    message: "Video metadata Validation error occurred,fix the errors and try again",
                innerException: invalidVideoMetadataException);

            // when
            ValueTask<VideoMetadata> addVideoMetadata =
               this.videoMetadataService.AddVideoMetadataAsync(invalidVideoMetadata);

            VideoMetadataValidationException actualvideoMetadataValidationException =
                await Assert.ThrowsAsync<VideoMetadataValidationException>(addVideoMetadata.AsTask);

            // then
            actualvideoMetadataValidationException.Should().BeEquivalentTo(expectedVideoMetadataValidationExpected);

            this.loggingBrokerMock.Verify(broker =>
              broker.LogError(It.Is(SameExceptionAs(expectedVideoMetadataValidationExpected))),
              Times.Once());

            this.storageBrokerMock.Verify(broker =>
              broker.InsertVideoMetadataAsync(It.IsAny<VideoMetadata>()),
              Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}

