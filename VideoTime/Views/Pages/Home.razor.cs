using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using VideoTime.Models.VideoMetadatas;

namespace VideoTime.Views.Pages
{
    public partial class Home
    {
        private List<VideoMetadata> BlobPaths;
        private List<VideoMetadata> filteredBlobPaths;
        private string searchQuery;
        private IBrowserFile selectedFile;

        protected override async Task OnInitializedAsync()
        {
            await LoadVideos();
        }

        private async Task LoadVideos()
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

                string sasToken = sasBuilder.ToSasQueryParameters(new StorageSharedKeyCredential(
                    blobServiceClient.AccountName, Configuration["AzureBlobStorageKey"])).ToString();

                string BlobPath = $"{blobClient.Uri}?{sasToken}";

                BlobPaths.Add(new VideoMetadata { BlobPath = BlobPath, Title = blobItem.Name });
            }

            filteredBlobPaths = new List<VideoMetadata>(BlobPaths);
        }

        //private void SearchVideos()
        //{
        //    if (string.IsNullOrEmpty(searchQuery))
        //    {
        //        filteredBlobPaths = new List<VideoMetadata>(BlobPaths);
        //    }
        //    else
        //    {
        //        filteredBlobPaths = BlobPaths.Where(url => url.Title.Contains(
        //            searchQuery, StringComparison.OrdinalIgnoreCase)).ToList();
        //    }
        //}
        private void SearchVideos(ChangeEventArgs e)
        {
            var searchTerm = e.Value.ToString().ToLower();
            filteredBlobPaths = filteredBlobPaths.Where(video => video.Title.ToLower().Contains(searchTerm)).ToList();
        }

        private void HandleFileSelected(InputFileChangeEventArgs e)
        {
            selectedFile = e.File;
        }

        private async Task UploadVideo()
        {
            if (selectedFile != null)
            {
                    string connectionString = Configuration.GetConnectionString("AzureBlobStorage");
                    string videoContainerName = Configuration["AzureVideoContainer"];

                    var blobServiceClient = new BlobServiceClient(connectionString);
                    var blobContainerClient = blobServiceClient.GetBlobContainerClient(videoContainerName);

                    var blobClient = blobContainerClient.GetBlobClient(selectedFile.Name);

                    using (var stream = selectedFile.OpenReadStream(500000000)) // Increase the limit if necessary
                    {
                        await blobClient.UploadAsync(stream, true);
                    }

                    await LoadVideos();
                    StateHasChanged();
            }
        }
    }
}
