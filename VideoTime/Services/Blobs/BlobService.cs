//==================================================
// Copyright (c) Coalition Of Good-Hearted Engineers
// Free To Use To Find Comfort And Peace
//==================================================

using Azure.Storage.Blobs;
using VideoTime.Brokers.Blobs;
using VideoTime.Models.FileResponses;

namespace VideoTime.Services.Blobs
{
    public class BlobService : IBlobService
    {
        private readonly string BlobConnectionString;
        private const string BlobContainer = "blazorblob-container";
        private readonly BlobServiceClient blobServiceClient;

        public BlobService(BlobServiceClient blobServiceClient,
            IConfiguration configuration)
        {
            this.blobServiceClient = blobServiceClient;
            BlobConnectionString = configuration.GetConnectionString("BlobConnectionString");

        }
        public async Task<List<Blob>> GetBlobFiles()
        {
            var blobs = new List<Blob>();   
            var container = this.blobServiceClient.GetBlobContainerClient(BlobContainer);

            await foreach (var blob in container.GetBlobsAsync())
            {
                var blobDto = new Blob()
                {
                    Name = blob.Name,
                    FileUrl = container.Uri.AbsoluteUri + "/" + blob.Name,
                    ContentType = blob.Properties.ContentType
                };

                blobs.Add(blobDto);
            }

            return blobs;
        }

        public async Task<BlobContent> GetBlobFile(string name)
        {
            var container = new BlobContainerClient(BlobConnectionString, BlobContainer);
            var blob = container.GetBlobClient(name);

            if (await blob.ExistsAsync())
            {
                var a = await blob.DownloadAsync();
                var blobContent = new BlobContent()
                {
                    Content = a.Value.Content,
                    ContentType = a.Value.ContentType,
                    Name = name
                };

                return blobContent;
            }

            return null;
        }
    }
}
