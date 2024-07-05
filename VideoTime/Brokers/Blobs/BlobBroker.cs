//==================================================
// Copyright (c) Coalition Of Good-Hearted Engineers
// Free To Use To Find Comfort And Peace
//==================================================

using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using VideoTime.Models.FileResponses;

namespace VideoTime.Brokers.Blobs
{
    public class BlobBroker : IBlobBroker
    {
        private readonly BlobServiceClient blobServiceClient;
        private const string ContainerName = "";

        public BlobBroker(BlobServiceClient blobServiceClient)
        {
            this.blobServiceClient = blobServiceClient;
        }

        public async Task DeleteAsync(Guid fileId)
        {
            BlobContainerClient containerClient =
                blobServiceClient.GetBlobContainerClient(ContainerName);

            BlobClient blobClient = containerClient.GetBlobClient(fileId.ToString());

            await blobClient.DeleteAsync();
        }

        public async Task<BlobContent> DownloadAsync(Guid fileId)
        {
            BlobContainerClient containerClient =
                blobServiceClient.GetBlobContainerClient(ContainerName);

            BlobClient blobClient = containerClient.GetBlobClient(fileId.ToString());

            Response<BlobDownloadResult> response =
                await blobClient.DownloadContentAsync();

            BlobContent fileResponse = new BlobContent()
                {
                    Content = response.Value.Content.ToStream(),
                    ContentType = response.Value.Details.ContentType
                };

            return fileResponse;
         }

        public async Task<Guid> UploadAsync(Stream stream, string contentType)
        {
            BlobContainerClient containerClient = 
                blobServiceClient.GetBlobContainerClient(ContainerName);

            var fileId = Guid.NewGuid();
            BlobClient blobClient = containerClient.GetBlobClient(fileId.ToString());

            await blobClient.UploadAsync(
                stream,
                new BlobHttpHeaders { ContentType = contentType });

            return fileId;
        }
    }
}
