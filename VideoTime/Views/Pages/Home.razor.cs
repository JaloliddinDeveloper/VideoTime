//==================================================
// Copyright (c) Coalition Of Good-Hearted Engineers
// Free To Use To Find Comfort And Peace
//==================================================
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using VideoTime.Models.VideoMetadatas;

namespace VideoTime.Views.Pages
{
    public partial class Home
    {
        private List<VideoMetadata> BlobPaths;
        private List<VideoMetadata> filteredBlobPaths;
        private string searchQuery;

        protected override async Task OnInitializedAsync()
        {
            string connectionString = Configuration.GetConnectionString("AzureBlobStorage");
            string videoContainerName = Configuration["AzureVideoContainer"];

            var blobServiceClient = new BlobServiceClient(connectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(videoContainerName);

            BlobPaths = new List<VideoMetadata>();

            await foreach (var blobItem in blobContainerClient.GetBlobsAsync())
            {
                var blobClient = blobContainerClient.GetBlobClient(blobItem.Name);

                BlobSasBuilder sasBuilder = new BlobSasBuilder()
                {
                    BlobContainerName = videoContainerName,
                    BlobName = blobItem.Name,
                    Resource = "b",
                    ExpiresOn = DateTimeOffset.UtcNow.AddHours(1)
                };

                sasBuilder.SetPermissions(BlobSasPermissions.Read);
                string sasToken = sasBuilder.ToSasQueryParameters(new StorageSharedKeyCredential(blobServiceClient.AccountName, Configuration["AzureBlobStorageKey"])).ToString();
                string BlobPath = $"{blobClient.Uri}?{sasToken}";

                BlobPaths.Add(new VideoMetadata { BlobPath = BlobPath, Title = blobItem.Name });
            }

            filteredBlobPaths = new List<VideoMetadata>(BlobPaths);
        }

        private void SearchVideos()
        {
            if (string.IsNullOrEmpty(searchQuery))
            {
                filteredBlobPaths = new List<VideoMetadata>(BlobPaths);
            }
            else
            {
                filteredBlobPaths = BlobPaths.Where(url => url.Title.Contains(
                    searchQuery, StringComparison.OrdinalIgnoreCase)).ToList();
            }
        }
    }
}