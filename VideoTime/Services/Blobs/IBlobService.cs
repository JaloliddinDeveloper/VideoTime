//==================================================
// Copyright (c) Coalition Of Good-Hearted Engineers
// Free To Use To Find Comfort And Peace
//==================================================

using VideoTime.Models.FileResponses;

namespace VideoTime.Services.Blobs
{
    public interface IBlobService
    {
        Task<Guid> UploadAsync(Stream stream, string contentType);
        Task<FileResponse> DownloadAsync(Guid fileId);
        Task DeleteAsync(Guid fileId);
        Task<Stream> GetBlobStreamAsync(string blobName, string containerName);
    }
}
