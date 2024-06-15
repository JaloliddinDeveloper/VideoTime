//==================================================
// Copyright (c) Coalition Of Good-Hearted Engineers
// Free To Use To Find Comfort And Peace
//==================================================
using VideoTime.Models.VideoMetadatas;

namespace VideoTime.Services.Foundations.VideoMetadatas
{
    public interface IVideoMetadataService
    {
        ValueTask<VideoMetadata> AddVideoMetadataAsync(VideoMetadata videoMetadata);
        IQueryable<VideoMetadata> RetrieveAllVideoMetadatas();
        ValueTask<VideoMetadata> RetrieveVideoMetadataByIdAsync(Guid videoMetadataId);
        ValueTask<VideoMetadata> ModifyVideoMetadataAsync(VideoMetadata videoMetadata);
    }
}
