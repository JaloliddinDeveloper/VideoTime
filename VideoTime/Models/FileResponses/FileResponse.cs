//--------------------------------------------------
// Copyright (c) Coalition Of Good-Hearted Engineers
// Free To Use To Find Comfort And Peace
//--------------------------------------------------

namespace VideoTime.Models.FileResponses
{
    public record FileResponse
    {
        public Stream Stream { get; set; }
        public string  ContentType { get; set; }
    }
}
