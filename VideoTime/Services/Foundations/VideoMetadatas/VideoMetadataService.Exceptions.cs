//==================================================
// Copyright (c) Coalition Of Good-Hearted Engineers
// Free To Use To Find Comfort And Peace
//==================================================
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using VideoTime.Models.Exceptions;
using VideoTime.Models.VideoMetadatas;
using Xeptions;

namespace VideoTime.Services.Foundations.VideoMetadatas
{
    public partial class VideoMetadataService
    {
        private delegate ValueTask<VideoMetadata> ReturningVideoMetadataFunction();
        private delegate IQueryable<VideoMetadata> ReturningVideoMetadatasFunction();
        private async ValueTask<VideoMetadata> TryCatch(ReturningVideoMetadataFunction returningVideoMetadataFunction)
        {
            try
            {
                return await returningVideoMetadataFunction();
            }
            catch (NullVideoMetadataException nullVideoMetadataException)
            {
                throw CreateAndLogValidationException(nullVideoMetadataException);
            }
            catch (InvalidVideoMetadataException invalidVideoMetadataException)
            {
                throw CreateAndLogValidationException(invalidVideoMetadataException);
            }
            catch (SqlException sqlException)
            {
                var failedVideoMetadataStorageException =
                new FailedVideoMetadataStorageException(
                    message: "Failed video metadata error occured, cotact support",
                    innerException: sqlException);
                throw CreateAndLogCriticalDependencyException(failedVideoMetadataStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistVideoMetadataException =
                 new AlreadyExistVideoMetadataException(
                     message: "VideoMetadata already exist",
                      innerException: duplicateKeyException);
                throw CreateAndDependencyValidationException(alreadyExistVideoMetadataException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                LockedVideoMetadataException lockedVideoMetadataException = new LockedVideoMetadataException(
                    "Video Metadata is locked, please try again",
                        dbUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(lockedVideoMetadataException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedVideoMetadataStorageException = new FailedVideoMetadataStorageException(
                    message: "Failed video metadata error occured, contact support",
                    innerException: databaseUpdateException);

                throw CreateAndLogDependencyException(failedVideoMetadataStorageException);
            }
            catch (NotFoundVideoMetadataException notFoundVidoeMetadataException)
            {
                throw CreateAndLogValidationException(notFoundVidoeMetadataException);
            }
            catch (Exception exception)
            {
                var failedVideoMetadataServiceException =
                    new FailedVideoMetadataServiceException(
                        message: "Failed VideoMetadata service error occurred,contact support",
                        innerException: exception);

                throw CreateAndLogServiseException(failedVideoMetadataServiceException);
            }
        }
        private IQueryable<VideoMetadata> TryCatch(ReturningVideoMetadatasFunction returningVideoMetadatasFunction)
        {
            try
            {
                return returningVideoMetadatasFunction();
            }
            catch (SqlException sqlException)
            {
                FailedVideoMetadataStorageException failedVideoMetadataStorageException =
                    new FailedVideoMetadataStorageException(
                        "Failed Video Metadata storage error occured, please contact support",
                            sqlException);

                throw CreateAndLogCriticalDependencyException(failedVideoMetadataStorageException);
            }
            catch (Exception exception)
            {
                FailedVideoMetadataServiceException failedVideoMetadataServiceException =
                    new FailedVideoMetadataServiceException(
                        message:"Unexpected error of Video Metadata occured",innerException: exception);
                     

                throw CreateAndLogVideoMetadataDependencyServiceErrorOccurs(failedVideoMetadataServiceException);
            }
        }

        private VideoMetadataDependencyServiceException CreateAndLogVideoMetadataDependencyServiceErrorOccurs(Xeption exception)
        {
            var videoMetadataDependencyServiceException =
                new VideoMetadataDependencyServiceException(
                    "Unexpected service error occured. Contact support",
                        exception);

            this.loggingBroker.LogError(videoMetadataDependencyServiceException);

            return videoMetadataDependencyServiceException;
        }

        private VideoMetadataServiceException CreateAndLogServiseException(Exception exception)
        {
            var videoMetadataServiceException =
                new VideoMetadataServiceException(message: "Video metadata service error occurred,contact support", innerException: exception);
            this.loggingBroker.LogError(videoMetadataServiceException);
            return videoMetadataServiceException;
        }
        private VideoMetadataDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var videoMetadataDependencyException =
              new VideoMetadataDependencyException(
                  message: "Video metadata error occured, fix the errors and try again",
                  innerException: exception);
            this.loggingBroker.LogCritical(videoMetadataDependencyException);
            throw videoMetadataDependencyException;
        }
        private VideoMetadataValidationException CreateAndLogValidationException(Xeption exception)
        {
            var videoMetadataValidationException =
                new VideoMetadataValidationException(
                    message: "Video metadata Validation error occurred,fix the errors and try again",
                    innerException: exception);

            this.loggingBroker.LogError(videoMetadataValidationException);
            return videoMetadataValidationException;
        }
        public VideoMetadataDependencyValidationException CreateAndDependencyValidationException(Xeption exception)
        {
            var videoMetadataDependencyValidationException =
                 new VideoMetadataDependencyValidationException(
                     message: "Video metadata Dependency validation error occured,fix the errors and try again",
                     innerException: exception);
            this.loggingBroker.LogError(videoMetadataDependencyValidationException);
            return videoMetadataDependencyValidationException;
        }
        private Exception CreateAndLogDependencyException(Xeption exception)
        {
            var videoMetadataDependencyException = new VideoMetadataDependencyException(
                message: "Video metadata dependency error occured, fix the errors and try again",
                innerException: exception);

            this.loggingBroker.LogError(videoMetadataDependencyException);

            return videoMetadataDependencyException;
        }
        private VideoMetadataDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var videoMetadataDependencyValidationException = new VideoMetadataDependencyValidationException(
                "Video metadata Dependency validation error occured,fix the errors and try again",
                    exception);

            this.loggingBroker.LogError(videoMetadataDependencyValidationException);

            return videoMetadataDependencyValidationException;
        }
    }
}
