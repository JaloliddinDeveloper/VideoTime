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
              (Rule: IsInvalid(videoMetadata.UpdatedDate), Parameter: nameof(videoMetadata.UpdatedDate)));
        }
        private void ValidationVideoMetadataNotNull(VideoMetadata videoMetadata)
        {
            if (videoMetadata is null)
            {
                throw new NullVideoMetadataException(message: "Video metadata is null");
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
            Message = "Data is required"
        };
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
   
