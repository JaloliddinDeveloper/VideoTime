//--------------------------------------------------
// Copyright (c) Coalition Of Good-Hearted Engineers
// Free To Use To Find Comfort And Peace
//--------------------------------------------------
using Xeptions;

namespace VideoTime.Models.Exceptions
{
    public class VideoMetadataValidationException:Xeption
    {
        public VideoMetadataValidationException(string message, Xeption innerException)
           : base(message, innerException)
        { }
    }
}
