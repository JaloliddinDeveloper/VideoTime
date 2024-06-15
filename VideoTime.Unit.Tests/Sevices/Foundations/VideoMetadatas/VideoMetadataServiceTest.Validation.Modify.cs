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
        public async Task ShouldThrowValidationExceptionOnModifyIfVideoMetadataIsNullAndLogItAsync()
        {
            // given
            VideoMetadata nullVideoMetadata = null;
            var nullVideoMetadataException = new NullVideoMetadataException("Video metadata is null");

            var expectedVideoMetadataValidationException =
                new VideoMetadataValidationException(
                    message: "Video metadata Validation error occurred,fix the errors and try again",
                    innerException: nullVideoMetadataException);

            // when
            ValueTask<VideoMetadata> modifyVideoMetadataTask =
                this.videoMetadataService.ModifyVideoMetadataAsync(nullVideoMetadata);

            VideoMetadataValidationException actualVideoMetadataValidationException =
                await Assert.ThrowsAsync<VideoMetadataValidationException>(
                    modifyVideoMetadataTask.AsTask);

            // then
            actualVideoMetadataValidationException.Should().BeEquivalentTo(
                expectedVideoMetadataValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedVideoMetadataValidationException))),
                    Times.Once());

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfVideoMetadataIsInvalidAndLogItAsync(
           string invalidText)
        {
            // given
            VideoMetadata invalidVideoMetadata = new VideoMetadata
            {
                Title = invalidText
            };

            var invalidVideoMetadataException =
                new InvalidVideoMetadataException(message: "Video metadata is invalid");

            invalidVideoMetadataException.AddData(
                key: nameof(VideoMetadata.Id),
                values: "Id is required");

            invalidVideoMetadataException.AddData(
                key: nameof(VideoMetadata.Title),
                values: "Text is required");

            invalidVideoMetadataException.AddData(
                key: nameof(VideoMetadata.BlobPath),
                values: "Text is required");

            invalidVideoMetadataException.AddData(
                key: nameof(VideoMetadata.CreatedDate),
                values: "Date is required");

            invalidVideoMetadataException.AddData(
                key: nameof(VideoMetadata.UpdatedDate),
                values: "Date is required");

            var expectedVideoMetadataValidationException =
                new VideoMetadataValidationException(
                    message: "Video metadata Validation error occurred,fix the errors and try again",
                    innerException: invalidVideoMetadataException);

            // when
            ValueTask<VideoMetadata> modifyVideoMetadataTask =
                this.videoMetadataService.ModifyVideoMetadataAsync(invalidVideoMetadata);

            VideoMetadataValidationException actualVideoMetadataValidationException =
                await Assert.ThrowsAsync<VideoMetadataValidationException>(
                    modifyVideoMetadataTask.AsTask);

            // then
            actualVideoMetadataValidationException.Should().BeEquivalentTo(
                expectedVideoMetadataValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedVideoMetadataValidationException))),
                    Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateVideoMetadataAsync(It.IsAny<VideoMetadata>()),
                    Times.Never());

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfVideoMetadataDoesNotExistAndLogItAsync()
        {
            // given
            VideoMetadata randomVideoMetadata = CreateRandomVideoMetadata();
            VideoMetadata nonExistVideoMetadata = randomVideoMetadata;
            VideoMetadata nullVideoMetadata = null;

            var notFoundVideoMetadataException =
                new NotFoundVidoeMetadataException(
                    message: $"Couldn't find video metadata with id {nonExistVideoMetadata.Id}");

            var expectedVideoMetadataValidationException =
                new VideoMetadataValidationException(
                    message: "Video metadata Validation error occurred,fix the errors and try again",
                    innerException: notFoundVideoMetadataException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectVideoMetadataByIdAsync(nonExistVideoMetadata.Id))
                    .ReturnsAsync(nullVideoMetadata);

            // when
            ValueTask<VideoMetadata> modifyVideoMetadataTask =
                this.videoMetadataService.ModifyVideoMetadataAsync(nonExistVideoMetadata);

            VideoMetadataValidationException actualVideoMetadataValidationException =
                await Assert.ThrowsAsync<VideoMetadataValidationException>(
                    modifyVideoMetadataTask.AsTask);

            // then
            actualVideoMetadataValidationException.Should().BeEquivalentTo(
                expectedVideoMetadataValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectVideoMetadataByIdAsync(nonExistVideoMetadata.Id),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedVideoMetadataValidationException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
