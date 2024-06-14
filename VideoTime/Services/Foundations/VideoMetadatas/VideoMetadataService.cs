//==================================================
// Copyright (c) Coalition Of Good-Hearted Engineers
// Free To Use To Find Comfort And Peace
//==================================================
using VideoTime.Brokers.Storages;
using VideoTime.Models.VideoMetadatas;

namespace VideoTime.Services.Foundations.VideoMetadatas
{
    public class VideoMetadataService:IVideoMetadataService
    {
        private readonly IStorageBroker storageBroker;

        public VideoMetadataService(IStorageBroker storageBroker)=>
            this.storageBroker = storageBroker;

        public ValueTask<VideoMetadata> AddVideoMetadataAsync(VideoMetadata videoMetadata)=>
            this.storageBroker.InsertVideoMetadataAsync(videoMetadata);       
    }
}
