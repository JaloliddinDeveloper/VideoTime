//==================================================
// Copyright (c) Coalition Of Good-Hearted Engineers
// Free To Use To Find Comfort And Peace
//==================================================
using VideoTime.Brokers.Loggings;
using VideoTime.Brokers.Storages;
using VideoTime.Models.VideoMetadatas;

namespace VideoTime.Services.Foundations.VideoMetadatas
{
    public class VideoMetadataService:IVideoMetadataService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;

        public VideoMetadataService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
        }

        public async ValueTask<VideoMetadata> AddVideoMetadataAsync(VideoMetadata videoMetadata)=>
          await this.storageBroker.InsertVideoMetadataAsync(videoMetadata);       
    }
}
