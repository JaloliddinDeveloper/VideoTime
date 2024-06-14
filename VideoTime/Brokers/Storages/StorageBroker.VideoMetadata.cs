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
    }
}
