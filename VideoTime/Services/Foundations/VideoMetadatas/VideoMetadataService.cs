﻿//==================================================
// Copyright (c) Coalition Of Good-Hearted Engineers
// Free To Use To Find Comfort And Peace
//==================================================
using VideoTime.Brokers.DateTimes;
using VideoTime.Brokers.Loggings;
using VideoTime.Brokers.Storages;
using VideoTime.Models.VideoMetadatas;

namespace VideoTime.Services.Foundations.VideoMetadatas
{
    public partial class VideoMetadataService : IVideoMetadataService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public VideoMetadataService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)

        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public ValueTask<VideoMetadata> AddVideoMetadataAsync(VideoMetadata videoMetadata) =>
           TryCatch(async () =>
           {
               ValidatevideoMetadataOnAdd(videoMetadata);
               return await this.storageBroker.InsertVideoMetadataAsync(videoMetadata);
           });

        public IQueryable<VideoMetadata> RetrieveAllVideoMetadatas() =>
            TryCatch(() =>
            {
                return this.storageBroker.SelectAllVideoMetadatas();
            });

        public ValueTask<VideoMetadata> RetrieveVideoMetadataByIdAsync(Guid videoMetadataId) =>
            TryCatch(async () =>

            {
                ValidateVideoMetadataId(videoMetadataId);
                VideoMetadata maybeVideoMetadata =
                    await this.storageBroker.SelectVideoMetadataByIdAsync(videoMetadataId);

                ValidateStorageVideoMetadata(maybeVideoMetadata, videoMetadataId);

                return maybeVideoMetadata;
            });
        public ValueTask<VideoMetadata> ModifyVideoMetadataAsync(VideoMetadata videoMetadata) =>
            TryCatch(async () =>
            {
                ValidateVideoMetadataOnModify(videoMetadata);
                VideoMetadata maybeVideoMetadata =
                 await this.storageBroker.SelectVideoMetadataByIdAsync(videoMetadata.Id);
                ValidateAgainstStorageOnModify(videoMetadata, maybeVideoMetadata);
                return await this.storageBroker.UpdateVideoMetadataAsync(videoMetadata);
            });
        public  ValueTask<VideoMetadata> RemoveVideoMetadataByIdAsync(Guid videoMetadataId)=>
            TryCatch(async()=>
        {
            ValidateVideoMetadataId(videoMetadataId);
            VideoMetadata maybeVideoMetadata =
                await this.storageBroker.SelectVideoMetadataByIdAsync(videoMetadataId);

            ValidateStorageVideoMetadata(maybeVideoMetadata, videoMetadataId);
            return await this.storageBroker.DeleteVideoMetadataAsync(maybeVideoMetadata);
        });
    }
}
