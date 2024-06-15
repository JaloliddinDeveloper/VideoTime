//==================================================
// Copyright (c) Coalition Of Good-Hearted Engineers
// Free To Use To Find Comfort And Peace
//==================================================
using VideoTime.Models.Exceptions;
using VideoTime.Models.VideoMetadatas;

namespace VideoTime.Services.Foundations.VideoMetadatas
{
    public partial class VideoMetadataService
    {
        private void ValidatevideoMetadataOnAdd(VideoMetadata videoMetadata)
        {
            ValidationVideoMetadataNotNull(videoMetadata);

            Validate(
              (Rule: IsInvalid(videoMetadata.Id), Parameter: nameof(videoMetadata.Id)),
              (Rule: IsInvalid(videoMetadata.Title), Parameter: nameof(videoMetadata.Title)),
              (Rule: IsInvalid(videoMetadata.BlobPath), Parameter: nameof(videoMetadata.BlobPath)),
              (Rule: IsInvalid(videoMetadata.CreatedDate), Parameter: nameof(videoMetadata.CreatedDate)),
              (Rule: IsInvalid(videoMetadata.UpdatedDate), Parameter: nameof(videoMetadata.UpdatedDate)),
              (Rule: IsNotRecent(videoMetadata.CreatedDate), Parameter: nameof(VideoMetadata.CreatedDate)),

                (Rule: IsNotSame(
                    firstDate: videoMetadata.CreatedDate,
                    secondDate: videoMetadata.UpdatedDate,
                    secondDateName: nameof(videoMetadata.UpdatedDate)),

                Parameter: nameof(videoMetadata.CreatedDate)));
        }
        private void ValidationVideoMetadataNotNull(VideoMetadata videoMetadata)
        {
            if (videoMetadata is null)
            {
                throw new NullVideoMetadataException(message: "Video metadata is null");
            }
        }
        public void ValidateVideoMetadataId(Guid videoMetadataId) =>
            Validate((Rule: IsInvalid(videoMetadataId), Parameter: nameof(VideoMetadata.Id)));

        private static void ValidateStorageVideoMetadata(VideoMetadata maybeVideoMetadata, Guid videoMetadataId)
        {
            if (maybeVideoMetadata is null)
            {
                throw new NotFoundVidoeMetadataException(
                    $"Couldn't find video metadata with id {videoMetadataId}");
            }
        }
        private void ValidateVideoMetadataOnModify(VideoMetadata videoMetadata)
        {
            ValidationVideoMetadataNotNull(videoMetadata);

            Validate(
             (Rule: IsInvalid(videoMetadata.Id), Parameter: nameof(VideoMetadata.Id)),
             (Rule: IsInvalid(videoMetadata.Title), Parameter: nameof(VideoMetadata.Title)),
             (Rule: IsInvalid(videoMetadata.BlobPath), Parameter: nameof(VideoMetadata.BlobPath)),
             (Rule: IsInvalid(videoMetadata.CreatedDate), Parameter: nameof(VideoMetadata.CreatedDate)),
             (Rule: IsInvalid(videoMetadata.UpdatedDate), Parameter: nameof(VideoMetadata.UpdatedDate))
             );
        }
        private void ValidateAgainstStorageOnModify(VideoMetadata inputVideoMetadata, VideoMetadata maybeVideoMetadata)
        {
            ValidateStorageVideoMetadataExists(maybeVideoMetadata, inputVideoMetadata.Id);
        }

        private void ValidateStorageVideoMetadataExists(VideoMetadata storageVideoMetadata, Guid videoMetadataId)
        {
            if (storageVideoMetadata is null)
            {
                throw new NotFoundVidoeMetadataException(
                    message: $"Couldn't find video metadata with id {videoMetadataId}");
            }
        }
        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };
        private static dynamic IsInvalid(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };
        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };
        private static dynamic IsNotSame(
        DateTimeOffset firstDate,
        DateTimeOffset secondDate,
        string secondDateName) => new
        {
            Condition = firstDate != secondDate,
            Message = $"Date is not same as {secondDateName}"
        };

        private dynamic IsNotRecent(DateTimeOffset date) => new
        {
            Condition = IsDateNotRecent(date),
            Message = "Date is not recent"
        };

        private bool IsDateNotRecent(DateTimeOffset date)
        {
            DateTimeOffset currentDateTime = this.dateTimeBroker.GetCurrentDateTimeOffset();
            TimeSpan timeDifference = currentDateTime.Subtract(date);

            return timeDifference.TotalSeconds is > 60 or < 0;
        }
        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidVideoMetadataException =
                new InvalidVideoMetadataException(message: "Video metadata is invalid");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidVideoMetadataException.UpsertDataList(parameter, rule.Message);
                }
            }
            invalidVideoMetadataException.ThrowIfContainsErrors();
        }
    }
}

