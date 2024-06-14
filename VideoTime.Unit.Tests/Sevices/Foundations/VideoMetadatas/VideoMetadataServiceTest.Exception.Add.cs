//==================================================
// Copyright (c) Coalition Of Good-Hearted Engineers
// Free To Use To Find Comfort And Peace
//==================================================
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using VideoTime.Models.Exceptions;
using VideoTime.Models.VideoMetadatas;
using Xunit.Abstractions;

namespace VideoTime.Unit.Tests.Sevices.Foundations.VideoMetadatas
{
    public partial class VideoMetadataServiceTest
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            //given
            VideoMetadata someVideoMetadata = CreateRandomVideoMetadata();
            SqlException sqlException = GetSqlException();

            FailedVideoMetadataStorageException failedVideoMetadataStorageException =
                new("Failed Video Metadata storage error occured, please contact support",
                    sqlException);

            VideoMetadataDependencyException expectedVideoMetadataDependencyException =
                new("Video Metadata dependency exception error occured, please contact support",
                    failedVideoMetadataStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertVideoMetadataAsync(someVideoMetadata))
                    .ThrowsAsync(sqlException);

            //when
            ValueTask<VideoMetadata> AddVideoMetadataTask =
                this.videoMetadataService.AddVideoMetadataAsync(someVideoMetadata);

            VideoMetadataDependencyException actualVideoMetadataDependencyException =
                await Assert.ThrowsAsync<VideoMetadataDependencyException>(AddVideoMetadataTask.AsTask);

            //then
            actualVideoMetadataDependencyException.Should().BeEquivalentTo(expectedVideoMetadataDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertVideoMetadataAsync(It.IsAny<VideoMetadata>()),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedVideoMetadataDependencyException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
        [Fact]
        public async Task ShouldThrowExceptionAddIfDublicateKeyErrorOccurs()
        {
            //given
            VideoMetadata someVideoMetadata = CreateRandomVideoMetadata();
            string someString = GetRandomString();

            var duplicateKeyException = new DuplicateKeyException(someString);

            AlreadyExistVideoMetadataException alreadyExistVideoMetadataException =
                new("Video Metadata already exist, please try again.",
                innerException:duplicateKeyException);

            VideoMetadataDependencyValidationException expectedVideoMetadataDependencyValidationException
                = new("Video Metadata dependency error occured. Fix errors and try again.",
                innerException: alreadyExistVideoMetadataException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertVideoMetadataAsync(someVideoMetadata))
                    .ThrowsAsync(duplicateKeyException);

            //when
            ValueTask<VideoMetadata> addVideoMetadataTask =
                this.videoMetadataService.AddVideoMetadataAsync(someVideoMetadata);

            VideoMetadataDependencyValidationException actualVideoMetadataDependencyValidationException =
                await Assert.ThrowsAnyAsync<VideoMetadataDependencyValidationException>(addVideoMetadataTask.AsTask);

            //then
            actualVideoMetadataDependencyValidationException.Should().BeEquivalentTo(
                expectedVideoMetadataDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertVideoMetadataAsync(It.IsAny<VideoMetadata>()),
                    Times.Once());
            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedVideoMetadataDependencyValidationException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
