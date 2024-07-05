//==================================================
// Copyright (c) Coalition Of Good-Hearted Engineers
// Free To Use To Find Comfort And Peace
//==================================================

namespace VideoTime.Models.FileResponses
{
    public record BlobContent
    {
        public Stream Content { get; set; }
        public string Name { get; set; }
        public string ContentType { get; set; }
    }
}
