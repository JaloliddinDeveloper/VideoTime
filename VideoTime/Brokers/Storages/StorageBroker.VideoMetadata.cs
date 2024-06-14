//==================================================
// Copyright (c) Coalition Of Good-Hearted Engineers
// Free To Use To Find Comfort And Peace
//==================================================
using Microsoft.EntityFrameworkCore;
using VideoTime.Models.VideoMetadatas;

namespace VideoTime.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<VideoMetadata> VideoMetadatas { get; set; }
        public async ValueTask<VideoMetadata> InsertVideoMetadataAsync(VideoMetadata videoMetadata) =>
      await InsertAsync(videoMetadata);

        public IQueryable<VideoMetadata> SelectAllVideoMetadatas() =>
           SelectAll<VideoMetadata>();

        public async ValueTask<VideoMetadata> SelectVideoMetadataByIdAsync(Guid videoMetadataId) =>
            await SelectAsync<VideoMetadata>(videoMetadataId);

        public async ValueTask<VideoMetadata> UpdateVideoMetadataAsync(VideoMetadata videoMetadata) =>
          await UpdateAsync(videoMetadata);

        public async ValueTask<VideoMetadata> DeleteVideoMetadataAsync(VideoMetadata videoMetadata) =>
           await DeleteAsync(videoMetadata);
    }
}
