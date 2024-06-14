//==================================================
// Copyright (c) Coalition Of Good-Hearted Engineers
// Free To Use To Find Comfort And Peace
//==================================================
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using VideoTime.Models.Exceptions;

namespace VideoTime.Unit.Tests.Sevices.Foundations.VideoMetadatas
{
    public partial class VideoMetadataServiceTest
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            //given
            SqlException sqlException = GetSqlException();

            var failedVideoMetadataStorageException =
                new FailedVideoMetadataStorageException(
                    "Failed Video Metadata storage error occured, please contact support",
                        sqlException);

            VideoMetadataDependencyException expectedVideoMetadataDependencyException =
                new VideoMetadataDependencyException(
                    "Video metadata error occured, fix the errors and try again",
                        failedVideoMetadataStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllVideoMetadatas()).Throws(sqlException);

            //when
            Action retrieveAllVideoMetadatasAction = () =>
                this.videoMetadataService.RetrieveAllVideoMetadatas();

            VideoMetadataDependencyException actualVideoMetadataDependencyException =
                Assert.Throws<VideoMetadataDependencyException>(retrieveAllVideoMetadatasAction);

            //then
            actualVideoMetadataDependencyException.Should().BeEquivalentTo(
                expectedVideoMetadataDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllVideoMetadatas(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedVideoMetadataDependencyException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAndLogItAsync()
        {
            //given
            string exceptionMessage = GetRandomString();
            var serviceException = new Exception(exceptionMessage);

            FailedVideoMetadataServiceException failedVideoMetadataServiceException =
                new FailedVideoMetadataServiceException(
                    "Unexpected error of Video Metadata occured.",
                        serviceException);

            VideoMetadataDependencyServiceException expectedVideoMetadataDependencyServiceException =
                new VideoMetadataDependencyServiceException(
                    "Unexpected service error occured. Contact support.",
                        failedVideoMetadataServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllVideoMetadatas()).Throws(serviceException);

            //when
            Action retrieveAllVideoMetadatasAction = () =>
                this.videoMetadataService.RetrieveAllVideoMetadatas();

            VideoMetadataDependencyServiceException actualVideoMetadataDependencyServiceException =
                Assert.Throws<VideoMetadataDependencyServiceException>(retrieveAllVideoMetadatasAction);

            //then
            actualVideoMetadataDependencyServiceException.Should().BeEquivalentTo(
                expectedVideoMetadataDependencyServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllVideoMetadatas(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedVideoMetadataDependencyServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
