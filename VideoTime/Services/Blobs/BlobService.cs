//==================================================
// Copyright (c) Coalition Of Good-Hearted Engineers
// Free To Use To Find Comfort And Peace
//==================================================
//==================================================
// Copyright (c) Coalition Of Good-Hearted Engineers
// Free To Use To Find Comfort And Peace
//==================================================
using VideoTime.Brokers.Blobs;
using VideoTime.Models.FileResponses;

namespace VideoTime.Services.Blobs
{
    public class BlobService:IBlobBroker
    {
        private readonly IBlobBroker blobBroker;

        public BlobService(IBlobBroker blobBroker)
        {
            this.blobBroker = blobBroker;
        }

        public async Task DeleteAsync(Guid fileId)=>
            await this.blobBroker.DeleteAsync(fileId);
       
        public async Task<FileResponse> DownloadAsync(Guid fileId)=>
            await this.blobBroker.DownloadAsync(fileId);

        public async Task<Guid> UploadAsync(Stream stream, string contentType)=>
            await this.blobBroker.UploadAsync(stream, contentType);
    }
}
