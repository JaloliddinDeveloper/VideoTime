//==================================================
// Copyright (c) Coalition Of Good-Hearted Engineers
// Free To Use To Find Comfort And Peace
//==================================================
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using VideoTime.Models.Exceptions;
using VideoTime.Models.VideoMetadatas;

namespace VideoTime.Unit.Tests.Sevices.Foundations.VideoMetadatas
{
    public partial class VideoMetadataServiceTest
    {
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
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedDateNotSameAsCreatedDateAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            int randomMinutes = randomNumber;
            VideoMetadata randomVideoMetadata = CreateRandomVideoMetadata();
            VideoMetadata invalidVideoMetadata = randomVideoMetadata.DeepClone();
            VideoMetadata storageVideoMetadata = invalidVideoMetadata.DeepClone();
            storageVideoMetadata.CreatedDate = storageVideoMetadata.CreatedDate.AddMinutes(randomMinutes);

            var invalidVideoMetadataException =
                new InvalidVideoMetadataException(
                    message: "Video metadata is invalid");

            invalidVideoMetadataException.AddData(
              key: nameof(VideoMetadata.CreatedDate),
              values: $"Date is not same as {nameof(VideoMetadata.CreatedDate)}");

            var expectedVideoMetadataValidationException =
                new VideoMetadataValidationException(
                    message: "Video metadata Validation error occurred,fix the errors and try again",
                    innerException: invalidVideoMetadataException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectVideoMetadataByIdAsync(invalidVideoMetadata.Id))
                    .ReturnsAsync(storageVideoMetadata);

            // when
            ValueTask<VideoMetadata> modifyVideoMetadataTask =
                this.videoMetadataService.ModifyVideoMetadataAsync(invalidVideoMetadata);

            VideoMetadataValidationException actualVideoMetadataValidationException =
                await Assert.ThrowsAsync<VideoMetadataValidationException>(
                    modifyVideoMetadataTask.AsTask);

            // then
            actualVideoMetadataValidationException.Should().BeEquivalentTo(
                expectedVideoMetadataValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectVideoMetadataByIdAsync(
                    invalidVideoMetadata.Id), Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedVideoMetadataValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
