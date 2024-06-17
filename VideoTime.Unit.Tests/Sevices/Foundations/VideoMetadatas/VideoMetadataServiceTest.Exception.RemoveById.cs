//==================================================
// Copyright (c) Coalition Of Good-Hearted Engineers
// Free To Use To Find Comfort And Peace
//==================================================
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using VideoTime.Models.Exceptions;
using VideoTime.Models.VideoMetadatas;

namespace VideoTime.Unit.Tests.Sevices.Foundations.VideoMetadatas
{
    public partial class VideoMetadataServiceTest
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRemoveByIdIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            var sqlException = GetSqlException();

            var failedVideoMetadataStorageException =
                new FailedVideoMetadataStorageException(
                    message: "Failed video metadata error occured, cotact support",
                    innerException: sqlException);

            var expectedVideoMetadataDependencyException =
                new VideoMetadataDependencyException(
                    message: "Video metadata error occured, fix the errors and try again",
                    innerException: failedVideoMetadataStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectVideoMetadataByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<VideoMetadata> removeVideoMetadataTask =
                this.videoMetadataService.RemoveVideoMetadataByIdAsync(someId);

            VideoMetadataDependencyException actualVideoMetadataDependencyException =
                await Assert.ThrowsAsync<VideoMetadataDependencyException>(
                    removeVideoMetadataTask.AsTask);

            // then
            actualVideoMetadataDependencyException.Should().BeEquivalentTo(
                expectedVideoMetadataDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectVideoMetadataByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedVideoMetadataDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnRemoveByIdIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            var dbUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedVideoMetadataException =
                new LockedVideoMetadataException(
                    message: "Video Metadata is locked, please try again",
                    innerException: dbUpdateConcurrencyException);

            var expectedVideoMetadataDependencyValidationException =
                new VideoMetadataDependencyValidationException(
                    message: "Video metadata Dependency validation error occured,fix the errors and try again",
                    innerException: lockedVideoMetadataException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectVideoMetadataByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(dbUpdateConcurrencyException);

            // when
            ValueTask<VideoMetadata> removeVideoMetadataTask =
                this.videoMetadataService.RemoveVideoMetadataByIdAsync(someId);

            VideoMetadataDependencyValidationException actualVideoMetadataDependencyValidationException =
                await Assert.ThrowsAsync<VideoMetadataDependencyValidationException>(
                    removeVideoMetadataTask.AsTask);

            // then
            actualVideoMetadataDependencyValidationException.Should().BeEquivalentTo(
                expectedVideoMetadataDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectVideoMetadataByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedVideoMetadataDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
